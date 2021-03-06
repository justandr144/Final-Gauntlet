﻿/* January 2021
 * Justin Andrews
 * A gauntlet style game with turnbased combat
 */

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

        //Global Variables
        int playerX = 360;
        int playerY = 420;
        int playerSize = 30;
        int playerSpeed = 9;

        int maxHealth = 100; //100
        int currentHealth = 100; //100
        int maxAttack = 8; //8
        int minAttack = 4; //4
        int maxSP = 10; //10
        int currentSP = 10; //10
        int playerDamage = 0;
        int enemyDamage = 0;
        int skeleton1Damage = 0;
        int skeleton2Damage = 0;
        int firebolt = 5;
        int healAmount = 0;
        int wolfSpecial = 0;
        int orcSpecial = 0;
        int necromancerSpecial = 0;
        int lichSpecial = 0;
        int bleedCancel = 0;
        int bleedLimit = 0;
        int stunCancel = 0;
        int stunLimit = 0;

        int enemyX = 360;
        int enemyY = 235;

        List<int> enemyMaxHP = new List<int>(new int[] { 50, 100, 150, 45, 90, 120 });
        List<int> enemyCurrentHP = new List<int>(new int[] { 50, 100, 150, 45, 90, 120 });
        List<int> enemyMaxAttack = new List<int>(new int[] { 6, 14, 8, 6, 10, 9 });
        List<int> enemyMinAttack = new List<int>(new int[] { 2, 3, 5, 2, 5, 4 });
        List<int> enemyMaxSP = new List<int>(new int[] { 0, 10, 9, 0, 50, 70 });
        List<int> enemyCurrentSP = new List<int>(new int[] { 0, 10, 9, 0, 50, 70 });
        int fightState = 0;

        List<int> skeletonMaxHP = new List<int>(new int[] { 45, 45, 45, 45, 45, 45, 0 });
        List<int> skeletonCurrentHP = new List<int>(new int[] { 45, 45, 45, 45, 45, 45, 0 });
        List<int> skeletonMaxAttack = new List<int>(new int[] { 9, 9, 6, 6, 6, 6, 0 });
        List<int> skeletonMinAttack = new List<int>(new int[] { 3, 3, 3, 3, 3, 3, 0 });
        int activeSkeleton1 = 0;
        int activeSkeleton2 = 0;

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
        bool drainEffect = false;
        bool normal = false;
        bool hard = false;

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush darkSlateGrayBrush = new SolidBrush(Color.DarkSlateGray);
        SolidBrush lightBlueBrush = new SolidBrush(Color.LightBlue);
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen greenPen = new Pen(Color.Green, 3);
        Pen redPen = new Pen(Color.Red, 3);
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

            if (musicCounter >= 3600)           //music loop
            {
                music.Stop();
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
                        state = "normalDifficulty";
                        spaceDown = false;
                    }
                    break;
                case "firstRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 0;
                    activeSkeleton1 = 6;
                    activeSkeleton2 = 6;
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
                    activeSkeleton1 = 6;
                    activeSkeleton2 = 6;
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
                    activeSkeleton1 = 6;
                    activeSkeleton2 = 6;
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
                    activeSkeleton1 = 0;
                    activeSkeleton2 = 1;
                    Movement();

                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fourthFight";
                    }
                    if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] <= 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fifthRoom";
                    }
                    else if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] > 0)
                    {
                        falseExit = true;
                    }
                    break;
                case "fifthRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 4;
                    activeSkeleton1 = 2;
                    activeSkeleton2 = 3;
                    Movement();

                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "fifthFight";
                    }
                    if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] <= 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "finalRoom";
                    }
                    else if (playerRec.IntersectsWith(exitRec) && enemyCurrentHP[fightState] > 0)
                    {
                        falseExit = true;
                    }
                    break;
                case "finalRoom":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    fightState = 5;
                    activeSkeleton1 = 4;
                    activeSkeleton2 = 5;
                    Movement();

                    if (playerRec.IntersectsWith(enemyRec) && enemyCurrentHP[fightState] > 0)
                    {
                        playerX = 360;
                        playerY = 420;
                        state = "finalFight";
                    }
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
                        enemyDeath.Stop();
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
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
                        enemyDeath.Stop();
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
                        gameover.Play();
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
                        enemyDeath.Stop();
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
                        gameover.Play();
                    }
                    break;
                case "fourthFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton1] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton1] <= 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                    {
                        state = "fourthRoom";
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
                        enemyDeath.Stop();
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
                        gameover.Play();
                    }
                    break;
                case "fifthFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton1] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton1] <= 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                    {
                        state = "fifthRoom";
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
                        enemyDeath.Stop();
                        enemyDeath.Play();
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
                        gameover.Play();
                    }
                    break;
                case "finalFight":
                    outputLabel.Visible = true;
                    enemyLabel.Visible = true;

                    if (currentHealth > 0 && enemyCurrentHP[fightState] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton1] > 0 || currentHealth > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        FightMoves();
                    }
                    else if (enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton1] <= 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                    {
                        state = "endScreen";
                        outputLabel.Text = "";
                        enemyLabel.Text = "";
                    }
                    else if (currentHealth <= 0)
                    {
                        state = "loseScreen";
                        music.Stop();
                        musicCounter = 0;
                        gameover.Stop();
                        gameover.Play();
                    }
                    break;
                case "endScreen":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;
                    music.Stop();
                    victory.Play();
                    musicCounter = 0;

                    if (spaceDown == true)
                    {
                        playerX = 360;
                        playerY = 420;
                        maxHealth = 100;
                        currentHealth = maxHealth;
                        maxAttack = 8;
                        minAttack = 4;
                        maxSP = 10;
                        currentSP = maxSP;
                        fightState = 0;
                        activeSkeleton1 = 6;
                        activeSkeleton2 = 6;
                        state = "normalDifficulty";
                        mode = "attack";
                        coward = false;
                        bleedEffect = false;
                        stunEffect = false;
                        normal = false;
                        hard = false;
                        for (int i = 0; i < 6; i++)
                        {
                            enemyCurrentHP[i] = enemyMaxHP[i];
                            enemyCurrentSP[i] = enemyMaxSP[i];
                            skeletonCurrentHP[i] = skeletonMaxHP[i];
                        }
                        musicCounter = 0;
                        music.Play();
                    }
                    break;
                case "loseScreen":
                    outputLabel.Visible = false;
                    enemyLabel.Visible = false;

                    if (spaceDown == true)
                    {
                        if (hard == true)
                        {
                            playerX = 360;
                            playerY = 420;
                            maxHealth = 100;
                            currentHealth = maxHealth;
                            maxAttack = 8;
                            minAttack = 4;
                            maxSP = 10;
                            currentSP = maxSP;
                            outputLabel.Text = "";
                            enemyLabel.Text = "";
                            fightState = 0;
                            activeSkeleton1 = 6;
                            activeSkeleton2 = 6;
                            state = "firstRoom";
                            mode = "attack";
                            coward = false;
                            bleedEffect = false;
                            stunEffect = false;
                            for (int i = 0; i < 6; i++)
                            {
                                enemyCurrentHP[i] = enemyMaxHP[i];
                                enemyCurrentSP[i] = enemyMaxSP[i];
                                skeletonCurrentHP[i] = skeletonMaxHP[i];
                            }
                            musicCounter = 0;
                            music.Play();
                        }
                        else if (normal == true)
                        {
                            playerX = 360;
                            playerY = 420;
                            currentHealth = maxHealth;
                            currentSP = maxSP;
                            outputLabel.Text = "";
                            enemyLabel.Text = "";
                            if (fightState == 0)
                            {
                                state = "firstRoom";
                            }
                            else if (fightState == 1)
                            {
                                state = "secondRoom";
                            }
                            else if (fightState == 2)
                            {
                                state = "thirdRoom";
                            }
                            else if (fightState == 3)
                            {
                                state = "fourthRoom";
                            }
                            else if (fightState == 4)
                            {
                                state = "fifthRoom";
                            }
                            else if (fightState == 5)
                            {
                                state = "finalRoom";
                            }
                            mode = "attack";
                            coward = false;
                            bleedEffect = false;
                            stunEffect = false;
                            enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                            enemyCurrentSP[fightState] = enemyMaxSP[fightState];
                            skeletonCurrentHP[fightState] = skeletonMaxHP[fightState];
                            musicCounter = 0;
                            music.Play();
                        }
                    }
                    break;
                case "normalDifficulty":
                    if (rightDown == true)
                    {
                        state = "hardDifficulty";
                    }
                    if (bDown == true)
                    {
                        state = "normalDifficultyYes";
                        bDown = false;
                    }
                    break;
                case "hardDifficulty":
                    if (leftDown == true)
                    {
                        state = "normalDifficulty";
                    }
                    if (bDown == true)
                    {
                        state = "hardDifficultyYes";
                        bDown = false;
                    }
                    break;
                case "normalDifficultyYes":
                    if (rightDown == true)
                    {
                        state = "normalDifficultyNo";
                    }
                    if (bDown == true)
                    {
                        normal = true;
                        state = "firstRoom";
                    }
                    if (nDown == true)
                    {
                        state = "normalDifficulty";
                    }
                    break;
                case "normalDifficultyNo":
                    if (leftDown == true)
                    {
                        state = "normalDifficultyYes";
                    }
                    if (bDown == true)
                    {
                        state = "normalDifficulty";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        state = "normalDifficulty";
                    }
                    break;
                case "hardDifficultyYes":
                    if (rightDown == true)
                    {
                        state = "hardDifficultyNo";
                    }
                    if (bDown == true)
                    {
                        hard = true;
                        state = "firstRoom";
                    }
                    if (nDown == true)
                    {
                        state = "hardDifficulty";
                    }
                    break;
                case "hardDifficultyNo":
                    if (leftDown == true)
                    {
                        state = "hardDifficultyYes";
                    }
                    if (bDown == true)
                    {
                        state = "hardDifficulty";
                        bDown = false;
                    }
                    if (nDown == true)
                    {
                        state = "hardDifficulty";
                    }
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
                    if (enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton1] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    }
                    e.Graphics.FillRectangle(whiteBrush, exitX, exitY, exitWidth, exitHeight);
                    e.Graphics.DrawString("Fourth Room", screenFont, whiteBrush, 5, 478);
                    break;
                case "fifthRoom":
                    e.Graphics.FillRectangle(orangeBrush, playerX, playerY, playerSize, playerSize);
                    if (enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton1] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        e.Graphics.FillRectangle(redBrush, enemyX, enemyY, playerSize, playerSize);
                    }
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
                    if (skeletonCurrentHP[activeSkeleton1] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonRight, 157, 100);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton1]}/{skeletonMaxHP[activeSkeleton1]}", screenFont, whiteBrush, 190, 320);
                    }
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonLeft, 307, 100);
                        e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 340, 320);
                    }
                    if (skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonLeft, 457, 100);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton2]}/{skeletonMaxHP[activeSkeleton2]}", screenFont, whiteBrush, 490, 320);
                    }
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);

                    MenuPaint();
                    break;
                case "fifthFight":
                    if (skeletonCurrentHP[activeSkeleton1] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonRight, 157, 100);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton1]}/{skeletonMaxHP[activeSkeleton1]}", screenFont, whiteBrush, 190, 320);
                    }
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.necromancer, 307, 100);
                        e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 340, 320);
                    }
                    if (skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonLeft, 457, 100);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton2]}/{skeletonMaxHP[activeSkeleton2]}", screenFont, whiteBrush, 490, 320);
                    }
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);

                    MenuPaint();
                    break;
                case "finalFight":
                    if (skeletonCurrentHP[activeSkeleton1] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonRight, 60, 130);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton1]}/{skeletonMaxHP[activeSkeleton1]}", screenFont, whiteBrush, 85, 320);
                    }
                    if (enemyCurrentHP[fightState] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.lichking, 243, 30);
                        e.Graphics.DrawString($"HP {enemyCurrentHP[fightState]}/{enemyMaxHP[fightState]}", screenFont, whiteBrush, 340, 320);
                    }
                    if (skeletonCurrentHP[activeSkeleton2] > 0)
                    {
                        e.Graphics.DrawImageUnscaled(Properties.Resources.SuperSkeletonLeft, 550, 130);
                        e.Graphics.DrawString($"HP {skeletonCurrentHP[activeSkeleton2]}/{skeletonMaxHP[activeSkeleton2]}", screenFont, whiteBrush, 585, 320);
                    }
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 0, 400, 750, 200);
                    e.Graphics.DrawString($"HP {currentHealth}/{maxHealth}", screenFont, whiteBrush, 50, 420);
                    e.Graphics.DrawString($"SP {currentSP}/{maxSP}", screenFont, whiteBrush, 50, 460);

                    MenuPaint();
                    break;
                case "endScreen":
                    e.Graphics.DrawString("Congratulations for beating the game!", screenFont, whiteBrush, 207, 140);
                    e.Graphics.DrawString("You can play again if you like!", screenFont, whiteBrush, 234, 220);
                    e.Graphics.DrawString("Press Space to Play Again!", screenFont, whiteBrush, 260, 300);
                    break;
                case "loseScreen":
                    if (coward == false)
                    {
                        e.Graphics.DrawString("Game Over!", screenFont, whiteBrush, 324, 140);
                        e.Graphics.DrawString("You have fallen to the army of monsters, however this is a game so you \n                  may retry as may times as you want.", screenFont, whiteBrush, 50, 220);
                        e.Graphics.DrawString("Press Space to Try Again!", screenFont, whiteBrush, 255, 320);
                    }
                    else
                    {
                        e.Graphics.DrawString("Game Over!", screenFont, whiteBrush, 324, 140);
                        e.Graphics.DrawString("I warned you, you coward", screenFont, whiteBrush, 257, 220);
                        e.Graphics.DrawString("Press Space to Try Again!", screenFont, whiteBrush, 255, 320);
                    }
                    break;
                case "normalDifficulty":
                    e.Graphics.DrawRectangle(greenPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(redPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 133, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Normal: When retrying, player only restarts the current room", screenFont, whiteBrush, 100, 225);
                    e.Graphics.DrawString("Normal", screenFont, whiteBrush, 202, 366);
                    e.Graphics.DrawString("Hard", screenFont, whiteBrush, 502, 366);
                    break;
                case "hardDifficulty":
                    e.Graphics.DrawRectangle(greenPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(redPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 423, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Hard: When retrying, player restarts at the very beginning", screenFont, whiteBrush, 107, 225);
                    e.Graphics.DrawString("Normal", screenFont, whiteBrush, 202, 366);
                    e.Graphics.DrawString("Hard", screenFont, whiteBrush, 502, 366);
                    break;
                case "normalDifficultyYes":
                    e.Graphics.DrawRectangle(greenPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(greenPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 133, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Are you sure you want to select Normal?", screenFont, whiteBrush, 197, 225);
                    e.Graphics.DrawString("Yes", screenFont, whiteBrush, 216, 366);
                    e.Graphics.DrawString("No", screenFont, whiteBrush, 512, 366);
                    break;
                case "normalDifficultyNo":
                    e.Graphics.DrawRectangle(greenPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(greenPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 423, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Are you sure you want to select Normal?", screenFont, whiteBrush, 197, 225);
                    e.Graphics.DrawString("Yes", screenFont, whiteBrush, 216, 366);
                    e.Graphics.DrawString("No", screenFont, whiteBrush, 512, 366);
                    break;
                case "hardDifficultyYes":
                    e.Graphics.DrawRectangle(redPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(redPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 133, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Are you sure you want to select Hard?", screenFont, whiteBrush, 206, 225);
                    e.Graphics.DrawString("Yes", screenFont, whiteBrush, 216, 366);
                    e.Graphics.DrawString("No", screenFont, whiteBrush, 512, 366);
                    break;
                case "hardDifficultyNo":
                    e.Graphics.DrawRectangle(redPen, 131, 350, 200, 50);
                    e.Graphics.DrawRectangle(redPen, 421, 350, 200, 50);
                    e.Graphics.FillRectangle(darkSlateGrayBrush, 423, 352, 197, 47);

                    e.Graphics.DrawString("Please Select a Difficulty", screenFont, whiteBrush, 255, 120);
                    e.Graphics.DrawString("Are you sure you want to select Hard?", screenFont, whiteBrush, 206, 225);
                    e.Graphics.DrawString("Yes", screenFont, whiteBrush, 216, 366);
                    e.Graphics.DrawString("No", screenFont, whiteBrush, 512, 366);
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
                    case "threeEnemiesLeft":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "threeEnemiesMid":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(blueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "threeEnemiesRight":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(blueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "twoLeftEnemiesLeft":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 543, 440);
                        break;
                    case "twoLeftEnemiesMid":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 543, 440);
                        break;
                    case "twoRightEnemiesMid":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 313, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoRightEnemiesRight":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 313, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoSplitEnemiesLeft":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoSplitEnemiesRight":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "threeEnemiesLeftS":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "threeEnemiesMidS":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(blueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "threeEnemiesRightS":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 133, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 403, 435, 133, 30);
                        e.Graphics.FillRectangle(blueBrush, 566, 435, 133, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 399, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 288, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 440, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 608, 440);
                        break;
                    case "twoLeftEnemiesLeftS":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 543, 440);
                        break;
                    case "twoLeftEnemiesMidS":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 543, 440);
                        break;
                    case "twoRightEnemiesMidS":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 313, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoRightEnemiesRightS":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Middle", screenFont, blackBrush, 313, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoSplitEnemiesLeftS":
                        e.Graphics.FillRectangle(blueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(lightBlueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
                        break;
                    case "twoSplitEnemiesRightS":
                        e.Graphics.FillRectangle(lightBlueBrush, 240, 435, 200, 30);
                        e.Graphics.FillRectangle(blueBrush, 470, 435, 200, 30);

                        e.Graphics.DrawString("Choose a Target", screenFont, whiteBrush, 385, 407);
                        e.Graphics.DrawString("Left", screenFont, blackBrush, 322, 440);
                        e.Graphics.DrawString("Right", screenFont, blackBrush, 548, 440);
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
                        if (activeSkeleton1 == 6)
                        {
                            playerDamage = randGen.Next(minAttack, maxAttack);
                            outputLabel.Text = $"Player does {playerDamage} damage";
                            enemyCurrentHP[fightState] -= playerDamage;
                            var attack = new System.Windows.Media.MediaPlayer();
                            attack.Open(new Uri(Application.StartupPath + "/Resources/attack.mp3"));
                            attack.Stop();
                            attack.Play();

                            EnemyAttack();
                            mode = "attack";
                            bDown = false;
                        }

                        if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "threeEnemiesLeft";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                        {
                            bDown = false;
                            mode = "twoLeftEnemiesLeft";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "twoSplitEnemiesLeft";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "twoRightEnemiesMid";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                        {
                            bDown = false;
                            mode = "leftEnemy";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] <= 0 && activeSkeleton1 < 6)
                        {
                            bDown = false;
                            mode = "midEnemy";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "rightEnemy";
                        }
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
                        if (activeSkeleton1 == 6)
                        {
                            currentSP -= 3;
                            playerDamage = randGen.Next(minAttack + firebolt, maxAttack + firebolt);
                            outputLabel.Text = $"Player does {playerDamage} damage";
                            enemyCurrentHP[fightState] -= playerDamage;
                            var fire = new System.Windows.Media.MediaPlayer();
                            fire.Open(new Uri(Application.StartupPath + "/Resources/fire.mp3"));
                            fire.Stop();
                            fire.Play();

                            EnemyAttack();
                            bDown = false;
                            mode = "attack";
                        }
                        if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "threeEnemiesLeftS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                        {
                            bDown = false;
                            mode = "twoLeftEnemiesLeftS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "twoSplitEnemiesLeftS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "twoRightEnemiesMidS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] <= 0)
                        {
                            bDown = false;
                            mode = "leftEnemyS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] > 0 && skeletonCurrentHP[activeSkeleton2] <= 0 && activeSkeleton1 < 6)
                        {
                            bDown = false;
                            mode = "midEnemyS";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] <= 0 && enemyCurrentHP[fightState] <= 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            bDown = false;
                            mode = "rightEnemyS";
                        }
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
                        healAmount = randGen.Next(20, 36);
                        currentHealth += healAmount;
                        if (currentHealth > maxHealth)
                        {
                            currentHealth = maxHealth;
                        }
                        bleedEffect = false;
                        outputLabel.Text = $"Player heals {healAmount} HP";
                        var heal = new System.Windows.Media.MediaPlayer();
                        heal.Open(new Uri(Application.StartupPath + "/Resources/heal.mp3"));
                        heal.Stop();
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
                        enemyLabel.Text = "Enemy does ";

                        skeleton1Damage = randGen.Next(skeletonMinAttack[activeSkeleton1], skeletonMaxAttack[activeSkeleton1]);
                        if (skeleton1Damage % 2 == 0 && skeletonCurrentHP[activeSkeleton1] > 0)
                        {
                            skeleton1Damage /= 2;
                            currentHealth -= skeleton1Damage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{skeleton1Damage}, ";
                            bDown = false;
                            mode = "attack";
                        }
                        else if (skeletonCurrentHP[activeSkeleton1] > 0)
                        {
                            skeleton1Damage -= 1;
                            skeleton1Damage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{skeleton1Damage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                        if (enemyDamage % 2 == 0 && enemyCurrentHP[fightState] > 0)
                        {
                            enemyDamage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{enemyDamage}, ";
                            bDown = false;
                            mode = "attack";
                        }
                        else if (enemyCurrentHP[fightState] > 0)
                        {
                            enemyDamage -= 1;
                            enemyDamage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{enemyDamage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        skeleton2Damage = randGen.Next(skeletonMinAttack[activeSkeleton2], skeletonMaxAttack[activeSkeleton2]);
                        if (skeleton2Damage % 2 == 0 && skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            skeleton2Damage /= 2;
                            currentHealth -= skeleton2Damage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{skeleton2Damage}";
                            bDown = false;
                            mode = "attack";
                        }
                        else if (skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            skeleton2Damage -= 1;
                            skeleton2Damage /= 2;
                            currentHealth -= enemyDamage;
                            outputLabel.Text = "Player blocked half of the incoming damage";
                            enemyLabel.Text += $"{skeleton2Damage}";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyLabel.Text += " damage";
                        var defend = new System.Windows.Media.MediaPlayer();
                        defend.Open(new Uri(Application.StartupPath + "/Resources/defend.mp3"));
                        defend.Stop();
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
                                skeletonCurrentHP[activeSkeleton1] = skeletonMaxHP[activeSkeleton1];
                                skeletonCurrentHP[activeSkeleton2] = skeletonMaxHP[activeSkeleton2];
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
                                skeletonCurrentHP[activeSkeleton1] = skeletonMaxHP[activeSkeleton1];
                                skeletonCurrentHP[activeSkeleton2] = skeletonMaxHP[activeSkeleton2];
                                outputLabel.Text = "";
                                enemyLabel.Text = "";
                                break;
                            case 5:
                                if (coward == false)
                                {
                                    outputLabel.Text = "You can not run from this fight";
                                    coward = true;
                                    mode = "attack";
                                    bDown = false;
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
                case "threeEnemiesLeft":
                    if (rightDown == true)
                    {
                        mode = "threeEnemiesMid";
                        rightDown = false;
                    }
                    if (bDown == true)
                    {
                        LeftAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "threeEnemiesMid":
                    if (rightDown == true)
                    {
                        mode = "threeEnemiesRight";
                        rightDown = false;
                    }
                    if (leftDown == true)
                    {
                        mode = "threeEnemiesLeft";
                        leftDown = false;
                    }
                    if (bDown == true)
                    {
                        MidAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "threeEnemiesRight":
                    if (leftDown == true)
                    {
                        mode = "threeEnemiesMid";
                        leftDown = false;
                    }
                    if (bDown == true)
                    {
                        RightAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoLeftEnemiesLeft":
                    if (rightDown == true)
                    {
                        mode = "twoLeftEnemiesMid";
                    }
                    if (bDown == true)
                    {
                        LeftAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoLeftEnemiesMid":
                    if (leftDown == true)
                    {
                        mode = "twoLeftEnemiesLeft";
                    }
                    if (bDown == true)
                    {
                        MidAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoRightEnemiesMid":
                    if (rightDown == true)
                    {
                        mode = "twoRightEnemiesRight";
                    }
                    if (bDown == true)
                    {
                        MidAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoRightEnemiesRight":
                    if (leftDown == true)
                    {
                        mode = "twoRightEnemiesMid";
                    }
                    if (bDown == true)
                    {
                        RightAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoSplitEnemiesLeft":
                    if (rightDown == true)
                    {
                        mode = "twoSplitEnemiesRight";
                    }
                    if (bDown == true)
                    {
                        LeftAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoSplitEnemiesRight":
                    if (leftDown == true)
                    {
                        mode = "twoSplitEnemiesLeft";
                    }
                    if (bDown == true)
                    {
                        RightAttack();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "leftEnemy":
                    LeftAttack();
                    mode = "attack";
                    break;
                case "rightEnemy":
                    RightAttack();
                    mode = "attack";
                    break;
                case "midEnemy":
                    MidAttack();
                    mode = "attack";
                    break;
                case "threeEnemiesLeftS":
                    if (rightDown == true)
                    {
                        mode = "threeEnemiesMidS";
                        rightDown = false;
                    }
                    if (bDown == true)
                    {
                        LeftAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "threeEnemiesMidS":
                    if (rightDown == true)
                    {
                        mode = "threeEnemiesRightS";
                        rightDown = false;
                    }
                    if (leftDown == true)
                    {
                        mode = "threeEnemiesLeftS";
                        leftDown = false;
                    }
                    if (bDown == true)
                    {
                        MidAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "threeEnemiesRightS":
                    if (leftDown == true)
                    {
                        mode = "threeEnemiesMidS";
                        leftDown = false;
                    }
                    if (bDown == true)
                    {
                        RightAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoLeftEnemiesLeftS":
                    if (rightDown == true)
                    {
                        mode = "twoLeftEnemiesMidS";
                    }
                    if (bDown == true)
                    {
                        LeftAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoLeftEnemiesMidS":
                    if (leftDown == true)
                    {
                        mode = "twoLeftEnemiesLeftS";
                    }
                    if (bDown == true)
                    {
                        MidAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoRightEnemiesMidS":
                    if (rightDown == true)
                    {
                        mode = "twoRightEnemiesRightS";
                    }
                    if (bDown == true)
                    {
                        MidAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoRightEnemiesRightS":
                    if (leftDown == true)
                    {
                        mode = "twoRightEnemiesMidS";
                    }
                    if (bDown == true)
                    {
                        RightAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoSplitEnemiesLeftS":
                    if (rightDown == true)
                    {
                        mode = "twoSplitEnemiesRightS";
                    }
                    if (bDown == true)
                    {
                        LeftAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "twoSplitEnemiesRightS":
                    if (leftDown == true)
                    {
                        mode = "twoSplitEnemiesLeftS";
                    }
                    if (bDown == true)
                    {
                        RightAttackS();
                        bDown = false;
                        mode = "attack";
                    }
                    if (nDown == true)
                    {
                        mode = "attack";
                        nDown = false;
                    }
                    break;
                case "leftEnemyS":
                    LeftAttackS();
                    mode = "attack";
                    break;
                case "rightEnemyS":
                    RightAttackS();
                    mode = "attack";
                    break;
                case "midEnemyS":
                    MidAttackS();
                    mode = "attack";
                    break;

            }
        }

        void Bleed()                    //bleed effect method
        {
            if (bleedEffect == true)
            {
                bleedCancel = randGen.Next(1, 5);

                if (bleedCancel < 4 && bleedLimit < 3)
                {
                    int bleedDamage = randGen.Next(2, 11);
                    currentHealth -= bleedDamage;
                    enemyLabel.Text += $"\n{bleedDamage} damage done by bleed";
                    bleedLimit++;
                }
                else if (bleedCancel == 4 && bleedLimit == 0)
                {
                    int bleedDamage = randGen.Next(2, 11);
                    currentHealth -= bleedDamage;
                    enemyLabel.Text += $"\n{bleedDamage} damage done by bleed";
                    bleedLimit++;
                }
                else
                {
                    enemyLabel.Text += "\nBleed has worn off";
                    bleedLimit = 0;
                    bleedEffect = false;
                }
            }
        }

        void Stun()
        {
            if (stunEffect == true)
            {
                stunCancel = randGen.Next(1, 3);

                if (stunCancel < 2 && stunLimit < 3)
                {
                    stunEffect = true;
                    enemyLabel.Text += "\nYou are stunned and can't attack";
                    stunLimit++;
                }
                else if (stunCancel == 3 && stunLimit == 0)
                {
                    stunEffect = true;
                    enemyLabel.Text += "\nYou are stunned and can't attack";
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
        void Drain()
        {
            if (drainEffect == true)
            {
                enemyLabel.Text += $"\nMiddle Enemy healed {enemyDamage}";
                enemyCurrentHP[fightState] += enemyDamage;
                if (enemyCurrentHP[fightState] > enemyMaxHP[fightState])
                {
                    enemyCurrentHP[fightState] = enemyMaxHP[fightState];
                }
            }
        }
        void EnemyAttack()                  //determine enemy damage
        {
            if (enemyCurrentHP[fightState] > 0 || skeletonCurrentHP[activeSkeleton1] > 0 || skeletonCurrentHP[activeSkeleton2] > 0)
            {
                lichSpecial = randGen.Next(1, 9);
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
                        orcSpecial = randGen.Next(1, 5);

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
                        enemyLabel.Text = "Enemy does ";

                        skeleton1Damage = randGen.Next(skeletonMinAttack[activeSkeleton1], skeletonMaxAttack[activeSkeleton1]);
                        if (skeletonCurrentHP[activeSkeleton1] > 0)
                        {
                            currentHealth -= skeleton1Damage;
                            enemyLabel.Text += $"{skeleton1Damage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        if (enemyCurrentHP[fightState] > 0)
                        {
                            enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                            currentHealth -= enemyDamage;
                            enemyLabel.Text += $"{enemyDamage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        skeleton2Damage = randGen.Next(skeletonMinAttack[activeSkeleton2], skeletonMaxAttack[activeSkeleton2]);
                        if (skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            currentHealth -= skeleton2Damage;
                            enemyLabel.Text += $"{skeleton2Damage} ";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyLabel.Text += "damage";
                        break;
                    case 4:
                        enemyLabel.Text = "Enemy does ";

                        skeleton1Damage = randGen.Next(skeletonMinAttack[activeSkeleton1], skeletonMaxAttack[activeSkeleton1]);
                        if (skeletonCurrentHP[activeSkeleton1] > 0)
                        {
                            currentHealth -= skeleton1Damage;
                            enemyLabel.Text += $"{skeleton1Damage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                        if (enemyCurrentHP[fightState] > 0)
                        {
                            necromancerSpecial = randGen.Next(1, 5);
                            if (necromancerSpecial == 3 && enemyCurrentSP[fightState] >= 10 && enemyCurrentHP[fightState] < enemyMaxHP[fightState])
                            {
                                enemyCurrentSP[fightState] -= 10;
                                enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                drainEffect = true;
                                bDown = false;
                                mode = "attack";
                            }
                            else
                            {
                                enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                drainEffect = false;
                                bDown = false;
                                mode = "attack";
                            }
                        }

                        skeleton2Damage = randGen.Next(skeletonMinAttack[activeSkeleton2], skeletonMaxAttack[activeSkeleton2]);
                        if (skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            currentHealth -= skeleton2Damage;
                            enemyLabel.Text += $"{skeleton2Damage} ";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyLabel.Text += "damage";
                        Drain();
                        break;
                    case 5:
                        enemyLabel.Text = "Enemy does ";

                        skeleton1Damage = randGen.Next(skeletonMinAttack[activeSkeleton1], skeletonMaxAttack[activeSkeleton1]);
                        if (skeletonCurrentHP[activeSkeleton1] > 0)
                        {
                            currentHealth -= skeleton1Damage;
                            enemyLabel.Text += $"{skeleton1Damage}, ";
                            bDown = false;
                            mode = "attack";
                        }

                        if (enemyCurrentHP[fightState] > 0)
                        {
                            if (lichSpecial == 1 && enemyCurrentSP[fightState] >= 10 && bleedEffect == false && stunEffect == false || lichSpecial == 2 && enemyCurrentSP[fightState] >= 10 && bleedEffect == false && stunEffect == false) //bleed
                            {
                                enemyDamage = randGen.Next(enemyMinAttack[fightState] + 3, enemyMaxAttack[fightState] + 3);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                bDown = false;
                                mode = "attack";
                                enemyCurrentSP[fightState] -= 10;
                                bleedEffect = true;
                                drainEffect = false;
                            }
                            else if (lichSpecial == 3 && enemyCurrentSP[fightState] >= 5 && stunEffect == false && bleedEffect == false || lichSpecial == 4 && enemyCurrentSP[fightState] >= 5 && stunEffect == false && bleedEffect == false) //stun
                            {
                                enemyDamage = randGen.Next(enemyMinAttack[fightState] + 1, enemyMaxAttack[fightState] + 1);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                bDown = false;
                                mode = "attack";
                                enemyCurrentSP[fightState] -= 5;
                                stunEffect = true;
                                drainEffect = false;
                            }
                            else if (lichSpecial == 5 && enemyCurrentSP[fightState] >= 20 && enemyCurrentHP[fightState] < enemyMaxHP[fightState] && bleedEffect == false && stunEffect == false) //drain
                            {
                                enemyCurrentSP[fightState] -= 20;
                                enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                drainEffect = true;
                                bDown = false;
                                mode = "attack";
                            }
                            else
                            {
                                enemyDamage = randGen.Next(enemyMinAttack[fightState], enemyMaxAttack[fightState]);
                                currentHealth -= enemyDamage;
                                enemyLabel.Text += $"{enemyDamage}, ";
                                bDown = false;
                                mode = "attack";
                            }
                        }

                        skeleton2Damage = randGen.Next(skeletonMinAttack[activeSkeleton2], skeletonMaxAttack[activeSkeleton2]);
                        if (skeletonCurrentHP[activeSkeleton2] > 0)
                        {
                            currentHealth -= skeleton2Damage;
                            enemyLabel.Text += $"{skeleton2Damage} ";
                            bDown = false;
                            mode = "attack";
                        }

                        enemyLabel.Text += "damage";
                        Bleed();
                        Stun();
                        Drain();
                        break;
                }
            }
        }
        void LeftAttack()
        {
            playerDamage = randGen.Next(minAttack, maxAttack);
            outputLabel.Text = $"Player does {playerDamage} damage";
            skeletonCurrentHP[activeSkeleton1] -= playerDamage;
            var attack = new System.Windows.Media.MediaPlayer();
            attack.Open(new Uri(Application.StartupPath + "/Resources/attack.mp3"));
            attack.Stop();
            attack.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
        void MidAttack()
        {
            playerDamage = randGen.Next(minAttack, maxAttack);
            outputLabel.Text = $"Player does {playerDamage} damage";
            enemyCurrentHP[fightState] -= playerDamage;
            var attack = new System.Windows.Media.MediaPlayer();
            attack.Open(new Uri(Application.StartupPath + "/Resources/attack.mp3"));
            attack.Stop();
            attack.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
        void RightAttack()
        {
            playerDamage = randGen.Next(minAttack, maxAttack);
            outputLabel.Text = $"Player does {playerDamage} damage";
            skeletonCurrentHP[activeSkeleton2] -= playerDamage;
            var attack = new System.Windows.Media.MediaPlayer();
            attack.Open(new Uri(Application.StartupPath + "/Resources/attack.mp3"));
            attack.Stop();
            attack.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
        void LeftAttackS()
        {
            currentSP -= 3;
            playerDamage = randGen.Next(minAttack + firebolt, maxAttack + firebolt);
            outputLabel.Text = $"Player does {playerDamage} damage";
            skeletonCurrentHP[activeSkeleton1] -= playerDamage;
            var fire = new System.Windows.Media.MediaPlayer();
            fire.Open(new Uri(Application.StartupPath + "/Resources/fire.mp3"));
            fire.Stop();
            fire.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
        void MidAttackS()
        {
            currentSP -= 3;
            playerDamage = randGen.Next(minAttack + firebolt, maxAttack + firebolt);
            outputLabel.Text = $"Player does {playerDamage} damage";
            enemyCurrentHP[fightState] -= playerDamage;
            var fire = new System.Windows.Media.MediaPlayer();
            fire.Open(new Uri(Application.StartupPath + "/Resources/fire.mp3"));
            fire.Stop();
            fire.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
        void RightAttackS()
        {
            currentSP -= 3;
            playerDamage = randGen.Next(minAttack + firebolt, maxAttack + firebolt);
            outputLabel.Text = $"Player does {playerDamage} damage";
            skeletonCurrentHP[activeSkeleton2] -= playerDamage;
            var fire = new System.Windows.Media.MediaPlayer();
            fire.Open(new Uri(Application.StartupPath + "/Resources/fire.mp3"));
            fire.Stop();
            fire.Play();

            EnemyAttack();
            bDown = false;
            mode = "attack";
        }
    }
}
