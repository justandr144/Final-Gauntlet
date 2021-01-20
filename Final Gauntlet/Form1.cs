using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Final_Gauntlet
{
    public partial class Form1 : Form
    {
        /*The Gauntlet
         * Justin Andrews
         * January 19 2021
         */

        //Global Variables
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
        int playerDamage = 0;
        int enemyDamage = 0;
        int firebolt = 5;
        int healAmount = 0;
        int wolfSpecial = 0;
        int orcSpecial = 0;
        int necromancerSpecial = 0;
        int lichSpecial = 0;
        int bleedCancel = 0;
        int stunCancel = 0;
        int stunLimit = 0;

        int enemyX = 360;
        int enemyY = 235;

        List<int> enemyMaxHP = new List<int>(new int[] { 50, 100, 160, 0, 0, 0 });
        List<int> enemyCurrentHP = new List<int>(new int[] { 50, 100, 160, 0, 0, 0 });
        List<int> enemyMaxAttack = new List<int>(new int[] { 6, 14, 8, 0, 0, 0 });
        List<int> enemyMinAttack = new List<int>(new int[] { 2, 3, 5, 0, 0, 0 });
        List<int> enemyMaxSP = new List<int>(new int[] { 0, 10, 9, 0, 0, 0 });
        List<int> enemyCurrentSP = new List<int>(new int[] { 0, 10, 9, 0, 0, 0 });
        int fightState = 0;

        int exitX = 300;
        int exitY = 0;
        int exitWidth = 150;
        int exitHeight = 20;

        int musicCounter = 10000;

        string state = "startScreen";
        string mode = "attack";

        bool upDown = false;
        bool downDown = false;
        bool leftDown = false;
        bool rightDown = false;
        bool bDown = false;
        bool nDown = false;
        bool spaceDown = false;
        bool falseExit = false;
        bool coward = false;
        bool noSP = false;
        bool bleedEffect = false;
        bool stunEffect = false;
        bool summonLimit = false;

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush darkSlateGrayBrush = new SolidBrush(Color.DarkSlateGray);
        SolidBrush lightBlueBrush = new SolidBrush(Color.LightBlue);
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Font screenFont = new Font("Consolas", 12);

        System.Windows.Media.MediaPlayer music;
        System.Windows.Media.MediaPlayer gameover;
        System.Windows.Media.MediaPlayer victory;

        Random randGen = new Random();
        public Form1()
        {
            InitializeComponent();

            music = new System.Windows.Media.MediaPlayer();
            music.Open(new Uri(Application.StartupPath + "/Resources/backgroundmusic.mp3"));

            gameover = new System.Windows.Media.MediaPlayer();
            gameover.Open(new Uri(Application.StartupPath + "/Resources/gameover.mp3"));

            victory = new System.Windows.Media.MediaPlayer();
            victory.Open(new Uri(Application.StartupPath + "/Resources/victory.mp3"));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)          //Key press setup
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
            switch (e.KeyCode)              //Key press setup
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

            if (musicCounter >= 3500)           //music loop
            {
                music.Play();
                musicCounter = 0;
            }
            musicCounter++;

            testLabel.Text = $"{musicCounter}";
            Rectangle playerRec = new Rectangle(playerX, playerY, playerSize, playerSize);              //Intersect rectangles
            Rectangle enemyRec = new Rectangle(enemyX, enemyY, playerSize, playerSize);
            Rectangle exitRec = new Rectangle(exitX, exitY, exitWidth, exitHeight);

            falseExit = false;

            switch (state)                                  //Instructions for each room and fight
            {
                case "startScreen":
                    if (spaceDown == true)
                    {
                        state = "firstRoom";
                    }
                    break;
                case "firstRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 0;
                    Movement();

                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "firstFight";
                    }
                    if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] <= 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "secondRoom";
                    }
                    else if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] > 0)
                    {
                        falseExit = true;
                    }
                    break;
                case "secondRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 1;
                    Movement();
                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "secondFight";
                    }
                    if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] <= 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "thirdRoom";
                    }
                    else if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] > 0)
                    {
                        falseExit = true;
                    }
                    break;
                case "thirdRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 2;
                    Movement();

                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "thirdFight";
                    }
                    if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] <= 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fourthRoom";
                    }
                    else if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] > 0)
                    {
                        falseExit = true;
                    }
                    break;
                case "fourthRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 3;
                    Movement();

                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fifthRoom";
                    }
                    break;
                case "fifthRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 4;
                    Movement();

                    if (playerRec.IntersectsWith(exitRec))
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "finalRoom";
                    }
                    break;
                case "finalRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 5;
                    Movement();
                    break;
                case "firstFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;
                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0)
                    {
                        state = "firstRoom";
                        maxHealth += 20;
                        currentHealth = maxHealth;
                        maxSP += 3;
                        currentSP = maxSP;
                        maxAttack += 1;
                        minAttack += 1;
                        outputLabel.Text = "";
                        enemyLabel.Text = "";
                        var enemyDeath = new System.Windows.Media.MediaPlayer();
                        enemyDeath.Open(new Uri(Application.StartupPath + "/Resources/enemydeath.mp3"));
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        gameover.Play();
                    }
                    break;
                case "secondFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0)
                    {
                        state = "secondRoom";
                        maxHealth += 20;
                        currentHealth = maxHealth;
                        maxSP += 3;
                        currentSP = maxSP;
                        maxAttack += 1;
                        minAttack += 1;
                        outputLabel.Text = "";
                        enemyLabel.Text = "";
                        var enemyDeath = new System.Windows.Media.MediaPlayer();
                        enemyDeath.Open(new Uri(Application.StartupPath + "/Resources/enemydeath.mp3"));
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        gameover.Play();
                        state = "loseScreen";
                    }
                    break;
                case "thirdFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0)
                    {
                        state = "thirdRoom";
                        maxHealth += 20;
                        currentHealth = maxHealth;
                        maxSP += 3;
                        currentSP = maxSP;
                        maxAttack += 1;
                        minAttack += 1;
                        outputLabel.Text = "";
                        enemyLabel.Text = "";
                        var enemyDeath = new System.Windows.Media.MediaPlayer();
                        enemyDeath.Open(new Uri(Application.StartupPath + "/Resources/enemydeath.mp3"));
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        gameover.Play();
                        state = "loseScreen";
                    }
                    break;
                case "fourthFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    break;
                case "fifthFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    break;
                case "finalFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    break;
                case "endScreen":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    victory.Play();

                    break;
                case "loseScreen":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;

                    break;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (falseExit == true)          //prevent early exits
            {
                e.Graphics.DrawString("You must defeat the enemy in this room before progressing", screenFont, whiteBrush, 115, 100);
            }

            switch (state)                      //drawing different screens
            {
                case "startScreen":
                    e.Graphics.DrawString("Welcome to the game!\nIn this game you control the orange square and take part in awesome \nbattles!\n\nYou move and change menu options with the arrow keys.\nTo select an option, press B and to go back, press N.\n\nTo initiate battle, interact with the enemy icon in the area.\n\nThe white space at the top is the exit. This will be unlocked once the enemy \nis defeated.\n\nMake your way through the dungeon and defeat the boss at the end!", screenFont, whiteBrush, 50, 50);
                    e.Graphics.DrawString("Press Space to Begin!", screenFont, whiteBrush, 277, 400);
                    break;
                case "firstRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    }
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("First Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "secondRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    }
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Second Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "thirdRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    }
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
                    e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 340, 320);

                    MenuPaint();
                    break;
                case "secondFight":
                    e.Graphics.DrawImageUnscaled(Properties.Resources.detailedwerewolf__1_, 303, 110);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);
                    e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 340, 320);

                    MenuPaint();
                    break;
                case "thirdFight":
                    e.Graphics.DrawImageUnscaled(Properties.Resources.orc1, 240, 70);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);
                    e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 330, 320);

                    MenuPaint();
                    break;
                case "fourthFight":

                    break;
                case "fifthFight":

                    break;
                case "finalFight":

                    break;
                case "endScreen":

                    break;
                case "loseScreen":
                    if (coward == false)
                    {
                        e.Graphics.DrawString("Game Over!", screenFont, whiteBrush, 320, 140);
                        e.Graphics.DrawString("You have fallen to the army of monsters, however this is a game so you \n                  may retry as may times as you want.", screenFont, whiteBrush, 50, 220);
                        e.Graphics.DrawString("Press Space to Try Again!", screenFont, whiteBrush, 255, 320);
                    }
                    else
                    {
                        e.Graphics.DrawString("Game Over!", screenFont, whiteBrush, 320, 140);
                        e.Graphics.DrawString("I warned you. You coward", screenFont, whiteBrush, 150, 220);
                        e.Graphics.DrawString("Press Space to Try Again!", screenFont, whiteBrush, 255, 320);
                    }
                    break;
            }

            void MenuPaint()                //fight menu method
            {
                switch (mode)
                {
                    case "attack":
                        e.Graphics.FillRectangle(blueBrush, 240, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);

                        e.Graphics.DrawString("Attack", screenFont, blackBrush, 313, 420);
                        e.Graphics.DrawString("Skill", screenFont, blackBrush, 318, 460);
                        e.Graphics.DrawString("Defend", screenFont, blackBrush, 542, 420);
                        e.Graphics.DrawString("Run", screenFont, blackBrush, 555, 460);
                        break;
                    case "skill":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 240, 455, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);

                        e.Graphics.DrawString("Attack", screenFont, blackBrush, 313, 420);
                        e.Graphics.DrawString("Skill", screenFont, blackBrush, 318, 460);
                        e.Graphics.DrawString("Defend", screenFont, blackBrush, 542, 420);
                        e.Graphics.DrawString("Run", screenFont, blackBrush, 555, 460);
                        break;
                    case "defend":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 455, 200, 30);

                        e.Graphics.DrawString("Attack", screenFont, blackBrush, 313, 420);
                        e.Graphics.DrawString("Skill", screenFont, blackBrush, 318, 460);
                        e.Graphics.DrawString("Defend", screenFont, blackBrush, 542, 420);
                        e.Graphics.DrawString("Run", screenFont, blackBrush, 555, 460);
                        break;
                    case "run":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 415, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 455, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 415, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 455, 200, 30);

                        e.Graphics.DrawString("Attack", screenFont, blackBrush, 313, 420);
                        e.Graphics.DrawString("Skill", screenFont, blackBrush, 318, 460);
                        e.Graphics.DrawString("Defend", screenFont, blackBrush, 542, 420);
                        e.Graphics.DrawString("Run", screenFont, blackBrush, 555, 460);
                        break;
                    case "attackMenuYes":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Attack?", screenFont, whiteBrush, 423, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "attackMenuNo":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Attack?", screenFont, whiteBrush, 423, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "skillMenu1":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Skill", screenFont, whiteBrush, 386, 407);
                        e.Graphics.DrawString("Firebolt 3SP", screenFont, blackBrush, 286, 440);
                        e.Graphics.DrawString("Heal 5SP", screenFont, blackBrush, 531, 440);
                        if (noSP == true)
                        {
                            e.Graphics.DrawString("Not enough SP for that move!", screenFont, whiteBrush, 325, 473);
                        }
                        break;
                    case "skillMenu2":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Skill", screenFont, whiteBrush, 386, 407);
                        e.Graphics.DrawString("Firebolt 3SP", screenFont, blackBrush, 286, 440);
                        e.Graphics.DrawString("Heal 5SP", screenFont, blackBrush, 531, 440);
                        if (noSP == true)
                        {
                            e.Graphics.DrawString("Not enough SP for that move!", screenFont, whiteBrush, 325, 473);
                        }
                        break;
                    case "skillMenu1Yes":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Use Firebolt?", screenFont, whiteBrush, 392, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "skillMenu1No":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Use Firebolt?", screenFont, whiteBrush, 392, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "skillMenu2Yes":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Use Heal?", screenFont, whiteBrush, 415, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "skillMenu2No":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Use Heal?", screenFont, whiteBrush, 415, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "defendMenuYes":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Defend?", screenFont, whiteBrush, 425, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "defendMenuNo":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Defend?", screenFont, whiteBrush, 425, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "runMenuYes":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Run?", screenFont, whiteBrush, 435, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                    case "runMenuNo":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Run?", screenFont, whiteBrush, 435, 407);
                        e.Graphics.DrawString("Yes", screenFont, blackBrush, 328, 440);
                        e.Graphics.DrawString("No", screenFont, blackBrush, 560, 440);
                        break;
                }
            }
        }
        void Movement()             //room movement method
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
        void FightMoves()                   //movement through fight menu
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
                    if (bDown == true)
                    {
                        if (stunEffect == false)
                        {
                            mode = "attackMenuYes";
                            bDown = false;
                        }
                        else
                        {
                            mode = "stun";
                            bDown = false;
                        }
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
                    if (bDown == true)
                    {
                        if (stunEffect == false)
                        {
                            mode = "skillMenu1";
                            bDown = false;
                        }
                        else
                        {
                            mode = "stun";
                            bDown = false;
                        }
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
                    if (bDown == true)
                    {
                        if (stunEffect == false)
                        {
                            mode = "defendMenuYes";
                            bDown = false;
                        }
                        else
                        {
                            mode = "stun";
                            bDown = false;
                        }
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
                    if (bDown == true)
                    {
                        if (stunEffect == false)
                        {
                            mode = "runMenuYes";
                            bDown = false;
                        }
                        else
                        {
                            mode = "stun";
                            bDown = false;
                        }
                    }
                    break;
                case ("attackMenuYes"):
                    if (rightDown == true)
                    {
                        mode = "attackMenuNo";
                    }
                    if (bDown == true)
                    {
                        playerDamage = randGen.Next(minAttack, maxAttack);
                        outputLabel.Text = $"Player does {playerDamage} damage";
                        enemyCurrentHP[fightState] -= playerDamage;
                        var attack = new System.Windows.Media.MediaPlayer();
                        attack.Open(new Uri(Application.StartupPath + "/Resources/attack.mp3"));
                        attack.Play();

                        EnemyAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                    }
                    break;
                case ("attackMenuNo"):
                    if (leftDown == true)
                    {
                        mode = "attackMenuYes";
                    }
                    if (bDown == true)
                    {
                        mode = "attack";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                    }
                    break;
                case ("skillMenu1"):
                    if (rightDown == true)
                    {
                        mode = "skillMenu2";
                        noSP = false;
                    }
                    if (bDown == true && currentSP >= 3)
                    {
                        mode = "skillMenu1Yes";
                        bDown = false;
                    }
                    else if (bDown == true && currentSP < 3)
                    {
                        noSP = true;
                    }
                    if (nDown == true)
                    {
                        mode = "skill";
                        noSP = false;
                    }
                    break;
                case ("skillMenu2"):
                    if (leftDown == true)
                    {
                        mode = "skillMenu1";
                        noSP = false;
                    }
                    if (bDown == true && currentSP >= 5)
                    {
                        mode = "skillMenu2Yes";
                        bDown = false;
                    }
                    else if (bDown == true && currentSP < 5)
                    {
                        noSP = true;
                    }
                    if (nDown == true)
                    {
                        mode = "skill";
                        noSP = false;
                    }
                    break;
                case ("skillMenu1Yes"):
                    if (rightDown == true)
                    {
                        mode = "skillMenu1No";
                    }
                    if (bDown == true)
                    {
                        currentSP -= 3;
                        playerDamage = randGen.Next(minAttack + firebolt, maxAttack + firebolt);
                        outputLabel.Text = $"Player does {playerDamage} damage";
                        enemyCurrentHP[fightState] -= playerDamage;
                        var fire = new System.Windows.Media.MediaPlayer();
                        fire.Open(new Uri(Application.StartupPath + "/Resources/fire.mp3"));
                        fire.Play();

                        EnemyAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "skillMenu1";
                        nDown = false;
                    }
                    break;
                case ("skillMenu1No"):
                    if (leftDown == true)
                    {
                        mode = "skillMenu1Yes";
                    }
                    if (bDown == true)
                    {
                        mode = "skillMenu1";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        mode = "skillMenu1";
                        nDown = false;
                    }
                    break;
                case ("skillMenu2Yes"):
                    if (rightDown == true)
                    {
                        mode = "skillMenu2No";
                    }
                    if (bDown == true)
                    {
                        currentSP -= 5;
                        healAmount = randGen.Next(15, 31);
                        currentHealth += healAmount;
                        if (currentHealth > maxHealth)
                        {
                            currentHealth = maxHealth;
                        }
                        outputLabel.Text = $"Player heals {healAmount} HP";
                        var heal = new System.Windows.Media.MediaPlayer();
                        heal.Open(new Uri(Application.StartupPath + "/Resources/heal.mp3"));
                        heal.Play();

                        EnemyAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "skillMenu1";
                        nDown = false;
                    }
                    break;
                case ("skillMenu2No"):
                    if (leftDown == true)
                    {
                        mode = "skillMenu2Yes";
                    }
                    if (bDown == true)
                    {
                        mode = "skillMenu2";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        mode = "skillMenu1";
                        nDown = false;
                    }
                    break;
                case ("defendMenuYes"):                         //Quick note, enemies don't use specials when player is blocking so they don't waste SP and it prevent the player from just letting them use all of their specials for an easy fight
                    if (rightDown == true)
                    {
                        mode = "defendMenuNo";
                    }
                    if (bDown == true)
                    {
                        enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                        if (enemyDamage % 2 == 0)
                        {
                            enemyDamage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            bDown = false;
                            mode = "attack";
                        }
                        else
                        {
                            enemyDamage -= 1;
                            enemyDamage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            bDown = false;
                            mode = "attack";
                        }
                        var defend = new System.Windows.Media.MediaPlayer();
                        defend.Open(new Uri(Application.StartupPath + "/Resources/defend.mp3"));
                        defend.Play();
                    }
                    if (nDown == true)
                    {
                        mode = "defend";
                    }
                    break;
                case ("defendMenuNo"):
                    if (leftDown == true)
                    {
                        mode = "defendMenuYes";
                    }
                    if (bDown == true)
                    {
                        mode = "defend";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        mode = "defend";
                    }
                    break;
                case ("runMenuYes"):
                    if (rightDown == true)
                    {
                        mode = "runMenuNo";
                    }
                    if (bDown == true)
                    {
                        switch (fightState)
                        {
                            case 0:
                                state = "firstRoom";
                                mode = "attack";
                                playerX = 360;
                                playerY = 420;
                                currentHealth = maxHealth;
                                currentSP = maxSP;
                                enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                                enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 1:
                                state = "secondRoom";
                                mode = "attack";
                                playerX = 360;
                                playerY = 420;
                                currentHealth = maxHealth;
                                currentSP = maxSP;
                                enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                                enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 2:
                                state = "thirdRoom";
                                mode = "attack";
                                playerX = 360;
                                playerY = 420;
                                currentHealth = maxHealth;
                                currentSP = maxSP;
                                enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                                enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 3:
                                state = "fourthRoom";
                                mode = "attack";
                                playerX = 360;
                                playerY = 420;
                                currentHealth = maxHealth;
                                currentSP = maxSP;
                                enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                                enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 4:
                                state = "fifthRoom";
                                mode = "attack";
                                playerX = 360;
                                playerY = 420;
                                currentHealth = maxHealth;
                                currentSP = maxSP;
                                enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                                enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 5:
                                if (coward == false)
                                {
                                    outputLabel.Text = "You can not run from this fight";
                                    coward = true;
                                    mode = "attack";
                                }
                                else if (coward == true)
                                {
                                    currentHealth = 0;
                                }
                                break;
                        }
                    }
                    if (nDown == true)
                    {
                        mode = "run";
                    }
                    break;
                case ("runMenuNo"):
                    if (leftDown == true)
                    {
                        mode = "runMenuYes";
                    }
                    if (bDown == true)
                    {
                        mode = "run";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        mode = "run";
                    }
                    break;
                case "stun":
                    EnemyAttack();
                    mode = "attack";
                    break;
            }
        }

        void Bleed()                    //bleed effect method
        {
            if (bleedEffect == true)
            {
                bleedCancel = randGen.Next(1, 5);

                if (bleedCancel < 4)
                {
                    int bleedDamage = randGen.Next(2, 11);
                    currentHealth -= bleedDamage;
                    enemyLabel.Text += $"\n{bleedDamage} damage done by bleed";
                }
                else
                {
                    enemyLabel.Text += "\nBleed has worn off";
                    bleedEffect = false;
                }
            }
        }

        void Stun()
        {
            if (stunEffect == true)
            {
                stunCancel = randGen.Next(1, 4);

                if (stunCancel < 3 && stunLimit < 3)
                {
                    stunEffect = true;
                    enemyLabel.Text += "\nYou are stunned and can't attack this turn";
                    stunLimit++;
                }
                else if (stunCancel == 3 && stunLimit == 0)
                {
                    stunEffect = true;
                    enemyLabel.Text += "\nYou are stunned and can't attack this turn";
                    stunLimit++;
                }
                else
                {
                    enemyLabel.Text += "\nThe stun effect has worn off";
                    stunEffect = false;
                    stunLimit = 0;
                }
            }
        }
        void EnemyAttack()                  //determine enemy damage
        {
            if (enemyCurrentHP[fightState] > 0)
            {
                switch (fightState)
                {
                    case 0:
                        enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                        enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                        currentHealth -= enemyDamage;
                        break;
                    case 1:
                        wolfSpecial = randGen.Next(1, 11);

                        if (wolfSpecial <= 2 && enemyCurrentSP[fightState] >= 5 && bleedEffect == false)
                        {
                            enemyDamage = randGen.Next(enemyMinAttack[fightState] + 3, enemyMaxAttack[fightState] + 3);
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            currentHealth -= enemyDamage;
                            enemyCurrentSP[fightState] -= 5;
                            bleedEffect = true;
                            Bleed();
                        }
                        else
                        {
                            enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            currentHealth -= enemyDamage;
                            Bleed();
                        }
                        break;
                    case 2:
                        orcSpecial = randGen.Next(1, 4);

                        if (orcSpecial == 2 && enemyCurrentSP[fightState] >= 3 && stunEffect == false)
                        {
                            enemyDamage = randGen.Next(enemyMinAttack[fightState] + 1, enemyMaxAttack[fightState] + 1);
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            currentHealth -= enemyDamage;
                            enemyCurrentSP[fightState] -= 3;
                            stunEffect = true;
                            Stun();
                        }
                        else
                        {
                            enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                            enemyLabel.Text = $"Enemy does {enemyDamage} damage";
                            currentHealth -= enemyDamage;
                            Stun();
                        }
                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                }
            }
        }
    }
}
