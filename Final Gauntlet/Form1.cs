using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Final_Gauntlet
{
    public partial class Form1 : Form
    {
        int playerX = 360;
        int playerY = 420;
        int playerSize = 30;
        int playerSpeed = 9;

        int maxHealth = 100;
        int currentHealth = 100;
        int maxAttack = 8;
        int minAttack = 4;
        int maxSP = 10;
        int currentSP = 10;

        int enemyX = 360;
        int enemyY = 235;

        int exitX = 300;
        int exitY = 0;
        int exitWidth = 150;
        int exitHeight = 20;

        int musicCounter = 3800;

        string state = "startScreen";
        string mode = "attack";

        bool upDown = false;
        bool downDown = false;
        bool leftDown = false;
        bool rightDown = false;
        bool bDown = false;
        bool nDown = false;
        bool spaceDown = false;

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush darkSlateGrayBrush = new SolidBrush(Color.DarkSlateGray);
        SolidBrush lightBlueBrush = new SolidBrush(Color.LightBlue);
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Font screenFont = new Font("Consolas", 12);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.B:
                    bDown = true;
                    break;
                case Keys.N:
                    nDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.B:
                    bDown = false;
                    break;
                case Keys.N:
                    nDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            SoundPlayer music = new SoundPlayer(Properties.Resources.backgroundMusic);

            if (musicCounter >= 3800)
            {
                music.Play();
                musicCounter = 0;
            }
            musicCounter++;

            Rectangle playerRec = new Rectangle(playerX, playerY, playerSize, playerSize);
            Rectangle enemyRec = new Rectangle(enemyX, enemyY, playerSize, playerSize);
            Rectangle exitRec = new Rectangle(exitX, exitY, exitWidth, exitHeight);

            switch (state)
            {
                case "startScreen":
                    if (spaceDown == true)
                    {
                        state = "firstRoom";
                    }
                    break;
                case "firstRoom":
                    Movement();
                    if (playerRec.IntersectsWith(enemyRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "firstFight";
                    }
                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "secondRoom";
                    }
                    break;
                case "secondRoom":
                    Movement();
                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "thirdRoom";
                    }
                    break;
                case "thirdRoom":
                    Movement();
                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fourthRoom";
                    }
                    break;
                case "fourthRoom":
                    Movement(); if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fifthRoom";
                    }
                    break;
                case "fifthRoom":
                    Movement();
                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "finalRoom";
                    }
                    break;
                case "finalRoom":
                    Movement();
                    break;
                case "firstFight":
                    FightMoves();
                    break;
                case "secondFight":

                    break;
                case "thirdFight":

                    break;
                case "fourthFight":

                    break;
                case "fifthFight":

                    break;
                case "finalFight":

                    break;
                case "endScreen":

                    break;
                case "skillScreen":

                    break;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            switch (state)
            {
                case "startScreen":
                    e.Graphics.DrawString("Welcome to the game!\nIn this game you control the square and take part in awesome battles!\n\nYou move and change menu options with the arrow keys.\nTo select an option, press B and to go back, press N.\n\nTo initiate battle, interact with the enemy icon in the area.\n\nThe white space at the top is the exit. This will be unlocked once the enemy \nis defeated.\n\nMake your way through the dungeon and defeat the boss at the end!", screenFont, whiteBrush, 50, 50);
                    e.Graphics.DrawString("Press Space to Begin!", screenFont, whiteBrush, 277, 400);
                    break;
                case "firstRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("First Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "secondRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Second Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "thirdRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Third Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "fourthRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Fourth Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "fifthRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Fifth Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "finalRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    e.Graphics.DrawString("Final Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "firstFight":
                    e.Graphics.DrawImageUnscaled(Properties.Resources.slime, 307, 200);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);
                    switch (mode)
                    {
                        case "attack":
                            e.Graphics.FillRectangle(blueBrush, 240, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);
                            break;
                        case "skill":
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                            e.Graphics.FillRectangle(blueBrush, 240, 455, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);
                            break;
                        case "defend":
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                            e.Graphics.FillRectangle(blueBrush, 470, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);
                            break;
                        case "run":
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                            e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                            e.Graphics.FillRectangle(blueBrush, 470, 455, 200, 30);
                            break;
                    }
                    e.Graphics.DrawString("Attack", screenFont, blackBrush, 313, 420);
                    e.Graphics.DrawString("Skill", screenFont, blackBrush, 318, 460);
                    e.Graphics.DrawString("Defend", screenFont, blackBrush, 542, 420);
                    e.Graphics.DrawString("Run", screenFont, blackBrush, 555, 460);
                    break;
                case "secondFight":

                    break;
                case "thirdFight":

                    break;
                case "fourthFight":

                    break;
                case "fifthFight":

                    break;
                case "finalFight":

                    break;
                case "endScreen":

                    break;
            }

        }
        void Movement()
        {
            if (upDown == true && playerY > 0)
            {
                playerY -= playerSpeed;
            }
            if (leftDown == true && playerX > 0)
            {
                playerX -= playerSpeed;
            }
            if (downDown == true && playerY < this.Height - playerSize)
            {
                playerY += playerSpeed;
            }
            if (rightDown == true && playerX < this.Width - playerSize)
            {
                playerX += playerSpeed;
            }
        }
        void FightMoves()
        {
            switch (mode)
            {
                case ("attack"):
                    if (rightDown == true)
                    {
                        mode = "defend";
                    }
                    if (downDown == true)
                    {
                        mode = "skill";
                    }
                    break;
                case ("skill"):
                    if (upDown == true)
                    {
                        mode = "attack";
                    }
                    if (rightDown == true)
                    {
                        mode = "run";
                    }
                    break;
                case ("defend"):
                    if (leftDown == true)
                    {
                        mode = "attack";
                    }
                    if (downDown == true)
                    {
                        mode = "run";
                    }
                    break;
                case ("run"):
                    if (upDown == true)
                    {
                        mode = "defend";
                    }
                    if (leftDown == true)
                    {
                        mode = "skill";
                    }
                    break;
            }
        }
    }
}
