using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Square[,] game_board = new Square[10, 10];
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void Init()                    //ititialize the board
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game_board[i, j] = new Square(i * 50, j * 70);
                    game_board[i, j].Location = new Point(i * 50, j * 50);
                    game_board[i, j].Size = new Size(50, 50); 
                    game_board[i, j].Click += new EventHandler(LeftMouseClick);         //this is for the clicking to the square with a left mouse, and when it's happen then The LeftMouseClick function is executed
                    this.Controls.Add(game_board[i, j]);
                }
            }
            this.ClientSize = new Size(650, 500);
            StartGame();
            Button restart = new Button();                        //restart button for restart the game
            restart.Click += new EventHandler(Restart_Click);
            restart.Text = "Restart";
            restart.Location = new Point(550, 90);
            restart.Visible = true;
            restart.Size = new Size(50, 20);
            this.Controls.Add(restart);
        }
        
        private void LeftMouseClick(object sender, EventArgs e)
        {
            if (!Square.gameOver)
            {
                Square square = (Square)sender;
                square.SetRevealed(true);
                if (square.GetMine())              //if we clicked to the mine field then end the game               
                {
                    EndGame();
                }
                else
                {
                    if (square.NumberOfMinesAround == 0)        //check if numberofminesaround is set to 0 first and then reveal fields
                    {
                        int p1 = 0, p2 = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (game_board[i, j] == square)
                                {
                                    p1 = i;
                                    p2 = j;
                                }
                            }
                        }
                        RevealCells(p1, p2);
                    }
                }
                int numberof = 0;        //all places is revealed, except the mines
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (!game_board[i,j].GetRevealed())
                        {
                            numberof++;
                        }
                    }
                }
                if (numberof == 10)                      //if we found all the mines, then end the game, and congratulate to the player
                {
                    EndGame();
                    MessageBox.Show("Congratulation you have found all the mines!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void StartGame()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game_board[i, j].Initialization();         //initialize the board
                }
            }
            int mine = 0;    //randomly set 10 mines into the board
            do
            {
                int r = random.Next(10);
                int c = random.Next(10);
                if (!game_board[r, c].GetMine())
                {
                    game_board[r, c].SetMine(true);
                    mine++;
                }
            } while (mine < 10);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game_board[i, j].NumberOfMinesAround = MinesAround(i,j);
                }
            }
            Square.gameOver = false;
        }

        private void RevealCells(int row, int column)       //recursive function to reveal fields
        {
            game_board[row, column].SetRevealed(true);
            if (game_board[row, column].NumberOfMinesAround == 0)
            {
                int[] xr = { -1, 0, 1, -1, 1, -1, 0, 1 };
                int[] yc = { -1, -1, -1, 0, 0, 1, 1, 1 };
                for (int i = 0; i < 8; i++)
                {
                    if (row + xr[i] >= 0 && row + xr[i] < 10 && column + yc[i] >= 0 && column + yc[i] < 10)
                    {
                        if (!game_board[row + xr[i], column + yc[i]].GetRevealed())
                        {
                            RevealCells(row + xr[i], column + yc[i]);
                        }
                    }
                }
            }
        }

        private void Restart_Click(object sender,EventArgs e) {
            StartGame();                               //this is for the restart the game
        }

        private int MinesAround(int x , int y)                //count the mines around the field
        {
            int[] xi = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] yi = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int numberof = 0;
            for(int i = 0; i < 8; i++)
            {
                if(x + xi[i] >= 0 && x + xi[i] < 10 && y + yi[i] >= 0 && y + yi[i] < 10)       
                {
                    if (game_board[x + xi[i], y + yi[i]].GetMine())
                    {
                        numberof++;
                    }
                }
            }
            return numberof;
        }
        private void EndGame()            //This is for ending the game we just set the gameOver to true and loop throught all the elements and if it's revealed and mined or flagged, then setting the revealed and mine to true(Revealing the mine fields)
        {
            Square.gameOver = true;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (!game_board[i, j].GetRevealed() && game_board[i,j].GetMine()|| game_board[i,j].GetFlag())
                    {
                        game_board[i, j].SetRevealed(true);                                  //revealing all mine field
                        game_board[i, j].SetMine(true);
                    }
                }
            }
            if(Square.gameOver == true)        //if gameOver is true we hit the mine, in this case the game is over and we output the message to inform the player
            {
                DialogResult result = MessageBox.Show("You hit the mine! Do you want to play again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    StartGame();
                }
                else if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            
        }
    }
}
