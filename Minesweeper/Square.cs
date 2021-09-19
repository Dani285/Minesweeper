using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
    class Square : Button              //the class Square inherited(Derived) by the Button class 
    {
        private bool flagged = false;           //A bool value(true or false) for store the status of the square object
        public static bool gameOver = false;    //another bool value(true or false) for checking whether the game is over or not
        private bool mine = false;              //another bool value(true or false) for store the status of the square object
        private bool revealed = false;          //another bool value(true or false) for store the status of the square object
        public int NumberOfMinesAround { get; set; } = 0;       //Calculating the numberofminesaround every game square and store it in variable NumberOfMinesAround

        public Square(int x, int y)          //constructor of the class Square with two parameter
        {
            this.Size = new Size(40,40);
            this.Location = new Point(x,y);
            this.MouseDown += new MouseEventHandler(MarkingWithRightMouse);
            this.Initialization();
        }
        public bool GetFlag()       //If the square contains a flag the GetFlag function return true, otherwise return false
        {
            return this.flagged;
        }
        public bool GetMine()      //If the square contains a mine the GetMine function return true and the game is over, otherwise return false
        {
            return this.mine;
        }
        public bool GetRevealed()  //Gets the revealed fields
        {
            return this.revealed;
        }
        public void Initialization()    //Setting the square object's status to the default
        {
            this.Text = "";         //this is not necessary but you can add this.Text ='',because when you start the game this function initialize the base condition of the button
            this.SetFlag(false);
            this.SetMine(false);
            this.SetRevealed(false);
            this.NumberOfMinesAround = 0;
        }
        public void SetFlag(bool flagged)  
        {
            this.flagged = flagged;
            if (this.flagged) {
                this.Image = Properties.Resources.flag;
            }
            else
            {
                this.Image = null;
            }
                                
        }
        public void SetMine(bool mine)        //Place 10 mines randomly
        {
            this.mine = mine;
        }
        public void SetRevealed(bool revealed)      //the parameter revealed, it's used to reveal the fields, true or false value
        {
            this.revealed = revealed;
            if (this.revealed)
            {
                this.BackColor = Color.White;
                if (this.mine)
                {
                    if (Square.gameOver)
                    {
                        this.Image = Properties.Resources.mine;        //using the resources
                    }
                    else
                    {
                        this.Image = Properties.Resources.mine_exploded;   //using the resources
                    }
                }
                else
                {
                    if (Square.gameOver && this.flagged)
                    {
                        this.Image = Properties.Resources.no_mine;
                    }
                    else
                    {
                        this.Image = null;
                        if(this.NumberOfMinesAround > 0)
                        {
                            this.Text = this.NumberOfMinesAround.ToString();
                        }
                    }
                }
            }
            else
            {
                this.BackColor = Color.DarkGray;
            }
        }
        private void MarkingWithRightMouse(object sender, MouseEventArgs ev)          //this is used for marking the flag on the board
        {
            if (ev.Button == MouseButtons.Right &&!Square.gameOver &&!this.revealed)
            {
                this.SetFlag(!this.flagged);
            }
        }
    }
}