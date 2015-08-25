#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TowerLibrary;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        SpriteBatch spriteBatch;

        KeyboardState keystate;
        KeyboardState prevKeystate;

        MouseState mouseState;
        MouseState oldState;

        Level level;

        Tower tower;
        TowerPreview towerPreview;

        Camera camera = new Camera();

        bool towerSelected = false;
        Vector2 selectedTowerPos;
        int selectedType = 0;
        Tower selectedTower;

        int money = 500;
        int lives = 30;

        int nukularcount = 0;

        #region Textures
        Texture2D cursor;
        Texture2D bullet;
        Texture2D enemy;
        Texture2D gui;

        Texture2D waveButton;
        Texture2D waveButtonPressed;

        Texture2D arrowTower;
        Texture2D arrowButton;
        Texture2D arrowButtonPressed;
        Texture2D arrowRange;

        Texture2D spikeTower;
        Texture2D spikeButton;
        Texture2D spikeButtonPressed;
        Texture2D spikeRange;

        Texture2D bombTower;
        Texture2D bombButton;
        Texture2D bombButtonPressed;

        Texture2D oneandOneTower;
        Texture2D oneandOneButton;
        Texture2D oneandOneButtonPressed;

        Texture2D ofLoveTower;
        Texture2D ofLoveButton;
        Texture2D ofLoveButtonPressed;
        Texture2D ofLoveRange;

        Texture2D nuclearButton;

        Texture2D window;
        #endregion

        SpriteFont arial;
        SpriteFont infoFont;

        TowerManager towerManager;
        WaveManager waveManager;
        GuiManager guiManager;

        int cellX, cellY;
        int tileX, tileY;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            spriteBatch = ScreenManager.SpriteBatch;
            arial = content.Load<SpriteFont>("Arial");
            infoFont = content.Load<SpriteFont>("Info");

            level = content.Load<Level>("cool");

            cursor = content.Load<Texture2D>("cursor");
            bullet = content.Load<Texture2D>("bullet");
            enemy = content.Load<Texture2D>("enemy");
            gui = content.Load<Texture2D>("gui");

            waveButton = content.Load<Texture2D>("wave button");
            waveButtonPressed = content.Load<Texture2D>("wave button hover");

            window = content.Load<Texture2D>("window");

            LoadTowerTextures();

            SetUpWaves();

            SetUpGui();

            audioEngine = new AudioEngine("Content/xactProject3.xgs");
            waveBank = new WaveBank(audioEngine, "Content/myWaveBank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/mySoundBank.xsb");
        }

        private void SetUpGui()
        {

            guiManager = new GuiManager(gui, new Vector2(level.WidthInPixels, 0), arial, infoFont);
            float left = level.WidthInPixels + 20;
            float middle = level.WidthInPixels + (gui.Width / 2) - (spikeButton.Width / 2);
            float right = level.WidthInPixels + gui.Width - 20 - spikeButton.Width;
            float bottom = level.HeightInPixels - 20 - waveButton.Height;
            float onebottom = level.HeightInPixels - 360 - waveButton.Height;

            float y = 150;

            guiManager.AddButton(arrowButton, arrowButtonPressed, new Vector2(left, y), "arrow");
            guiManager.AddButton(spikeButton, spikeButtonPressed, new Vector2(middle, y), "spike");
            guiManager.AddButton(bombButton, bombButtonPressed, new Vector2(right, y), "bomb");
            guiManager.AddButton(waveButton, waveButtonPressed, new Vector2(left, bottom), "wave");
            guiManager.AddButton(oneandOneButton, oneandOneButtonPressed, new Vector2(left, onebottom), "oneandone");
            guiManager.AddButton(ofLoveButton, ofLoveButtonPressed, new Vector2(middle, onebottom), "oflove");
            guiManager.AddButton(nuclearButton, nuclearButton, new Vector2(right, onebottom), "nuclear");

            foreach (Button button in guiManager.ButtonList)
            {
                button.OnLeftMouseClick += new Button.FiredEvent(ButtonPressed);

                if (button.Name != "wave")
                {
                    guiManager.AddWindow(window, new Vector2(left, 202), button.Name);
                }
            }
        }

        private void ButtonPressed(object sender)
        {
            Button button = sender as Button;

            if (button != null)
            {
                switch (button.Name)
                {
                    case "arrow":
                        tower = new ArrowTower(arrowTower, Vector2.Zero, bullet, 1, soundBank);
                        if (tower.Cost <= money)
                        {
                            towerPreview.ChangeTowerTexture(arrowTower);
                            towerPreview.ChangeRangeTexture(arrowRange, tower.Range);
                        }

                        else tower = null;

                        break;

                    case "spike":
                        tower = new SpikeTower(spikeTower, Vector2.Zero, bullet, 1, soundBank);
                        if (tower.Cost <= money)
                        {
                            towerPreview.ChangeTowerTexture(spikeTower);
                            towerPreview.ChangeRangeTexture(spikeRange, tower.Range);
                        }

                        else tower = null;
                        break;

                    case "bomb":
                        tower = new BombTower(bombTower, Vector2.Zero, bullet, 1, soundBank);
                        if (tower.Cost <= money)
                        {
                            towerPreview.ChangeTowerTexture(bombTower);
                            towerPreview.ChangeRangeTexture(arrowRange, tower.Range);
                        }

                        else tower = null;
                        break;

                    // button der 1und1 tower baut
                    case "oneandone":
                        tower = new OneandOneTower(oneandOneTower, Vector2.Zero, bullet, 1, soundBank);
                        if (tower.Cost <= money)
                        { //towerPreview.ChangeRangeTexture(arrowTower);
                            towerPreview.ChangeRangeTexture(arrowRange, tower.Range);
                        }
                        else tower = null;
                        break;

                    // Tower of  Love ersteller
                    case "oflove":
                        tower = new ofLoveTower(ofLoveTower, Vector2.Zero, bullet, 1, soundBank);
                        if (tower.Cost <= money)
                        {
                            towerPreview.ChangeTowerTexture(ofLoveTower);
                            towerPreview.ChangeRangeTexture(ofLoveRange, tower.Range);
                        }
                        else tower = null;
                        break;

                    case "nuclear":
                        if (100 <= money && nukularcount <= 2)
                        {
                            money -= 100;
                            nukularcount++;
                            soundBank.PlayCue("nukular3");

                            foreach (Enemy enemy in waveManager.GetEnemies())
                            {
                                enemy.IsDead = true;
                            }
                        }
                        break;


                    case "wave":
                        waveManager.StartNextWave();
                        button.Visible = false;
                        break;
                }
            }
        }

        private void SetUpWaves()
        {
            waveManager = new WaveManager(enemy, 10, level.Waypoints);
        }

        private void LoadTowerTextures()
        {
            arrowTower = content.Load<Texture2D>("Towers\\MachineGun\\machinegun tower");
            arrowButton = content.Load<Texture2D>("Towers\\MachineGun\\machinegun");
            arrowButtonPressed = content.Load<Texture2D>("Towers\\MachineGun\\machinegun pressed");

            spikeTower = content.Load<Texture2D>("Towers\\Flame\\flame tower");
            spikeButton = content.Load<Texture2D>("Towers\\Flame\\flame");
            spikeButtonPressed = content.Load<Texture2D>("Towers\\Flame\\flame pressed");

            bombTower = content.Load<Texture2D>("Towers\\Rocket\\rocket tower");
            bombButton = content.Load<Texture2D>("Towers\\Rocket\\rocket");
            bombButtonPressed = content.Load<Texture2D>("Towers\\Rocket\\rocket pressed");


            /// <summary> 1und1 Tower  </summary>
            oneandOneTower = content.Load<Texture2D>("Towers\\1and1\\1and1 tower");
            oneandOneButton = content.Load<Texture2D>("Towers\\1and1\\1and1 tower");
            oneandOneButtonPressed = content.Load<Texture2D>("Towers\\1and1\\1and1 tower");

            /// <summary> 1und1 Tower  </summary>
            ofLoveTower = content.Load<Texture2D>("Towers\\OfLove\\oflove");
            ofLoveButton = content.Load<Texture2D>("Towers\\OfLove\\oflove");
            ofLoveButtonPressed = content.Load<Texture2D>("Towers\\OfLove\\oflove");

            ofLoveRange = content.Load<Texture2D>("Towers\\range");

            nuclearButton = content.Load<Texture2D>("nuclear");

            arrowRange = content.Load<Texture2D>("Towers\\range");
            spikeRange = content.Load<Texture2D>("Towers\\spikeRange");

            towerManager = new TowerManager();
            towerPreview = new TowerPreview(arrowTower, Vector2.Zero, arrowRange);
        }

        private bool IsInBounds(int cellX, int cellY)
        {
            if (cellX >= 0 && cellY >= 0 && cellX < level.Width && cellY < level.Height)
                return true;
            return false;
        }
        private void CreateTower()
        {
            Rectangle tempRect = new Rectangle(tileX, tileY, Engine.TileWidth, Engine.TileHeight);

            if (!towerManager.IsOnTower(tileX, tileY))
            {
                if (tower is ArrowTower)
                {
                    ArrowTower arrow = new ArrowTower(arrowTower,
                        new Vector2(tileX, tileY), bullet, 1, soundBank);
                    towerManager.AddTower(arrow);
                    money -= tower.Cost;
                    tower = null;
                }

                else if (tower is SpikeTower)
                {
                    SpikeTower spike = new SpikeTower(spikeTower,
                        new Vector2(tileX, tileY), bullet, 1, soundBank);
                    towerManager.AddTower(spike);
                    //money -= tower.Cost;
                    money -= 1;
                    tower = null;
                }

                else if (tower is BombTower)
                {
                    BombTower bomb = new BombTower(bombTower,
                        new Vector2(tileX, tileY), bullet, 1, soundBank);
                    towerManager.AddTower(bomb);
                    //money -= tower.Cost;
                    money -= 1;
                    tower = null;
                }

                else if (tower is OneandOneTower)
                {
                    OneandOneTower oneandone = new OneandOneTower(bombTower,
                        new Vector2(tileX, tileY), bullet, 1, soundBank);
                    towerManager.AddTower(oneandone);
                    money -= tower.Cost;
                    tower = null;
                }
                if (tower is ofLoveTower)
                {
                    ofLoveTower love = new ofLoveTower(ofLoveTower,
                        new Vector2(tileX, tileY), bullet, 1, soundBank);
                    towerManager.AddTower(love);
                    money -= tower.Cost;
                    soundBank.PlayCue("liebe2");
                    tower = null;
                }

            }
        }

        private void HandleInput()
        {
            if (keystate.IsKeyDown(Keys.A) && prevKeystate.IsKeyUp(Keys.A))
            {
                tower = new ArrowTower(arrowTower, Vector2.Zero, bullet, 1, soundBank);
                if (tower.Cost <= money)
                {
                    towerPreview.ChangeTowerTexture(arrowTower);
                    towerPreview.ChangeRangeTexture(arrowRange, tower.Range);
                }

                else tower = null;
            }

            if (keystate.IsKeyDown(Keys.S) && prevKeystate.IsKeyUp(Keys.S))
            {
                tower = new SpikeTower(spikeTower, Vector2.Zero, bullet, 1, soundBank);
                if (tower.Cost <= money)
                {
                    towerPreview.ChangeTowerTexture(spikeTower);
                    towerPreview.ChangeRangeTexture(spikeRange, tower.Range);
                }

                else tower = null;
            }

            if (keystate.IsKeyDown(Keys.B) && prevKeystate.IsKeyUp(Keys.B))
            {
                tower = new BombTower(bombTower, Vector2.Zero, bullet, 1, soundBank);
                if (tower.Cost <= money)
                {
                    towerPreview.ChangeTowerTexture(bombTower);
                    towerPreview.ChangeRangeTexture(arrowRange, tower.Range);
                }

                else tower = null;
            }

            if (keystate.IsKeyDown(Keys.Space) && prevKeystate.IsKeyUp(Keys.Space))
            {
                Button waveButton = guiManager.GetButton("wave");

                if (waveButton.Visible)
                    waveManager.StartNextWave();
                waveButton.Visible = false;
            }
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            mouseState = Mouse.GetState();

            keystate = Keyboard.GetState();

            //this.Window.Title = "X : " + mouseState.X.ToString() + "    Y  : " + mouseState.Y.ToString();

            cellX = (int)(mouseState.X / Engine.TileWidth);
            cellY = (int)(mouseState.Y / Engine.TileHeight);

            tileX = cellX * Engine.TileWidth;
            tileY = cellY * Engine.TileHeight;

            HandleInput();

            waveManager.Update(gameTime);

            if (waveManager.CurrentWave.EnemyReachedEnd)
            {
                lives--;
                waveManager.CurrentWave.EnemyReachedEnd = false;
            }

            if (waveManager.waveFinished)
            {
                Button waveButton = guiManager.GetButton("wave");
                waveButton.Visible = true;
            }

            guiManager.Update(gameTime);

            if (mouseState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                tower = null;
                towerSelected = false;
            }

            if (IsInBounds(cellX, cellY) && this.IsActive && level.GetCellIndex(cellX, cellY) != 1)
            {
                if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
                {
                    selectedTower = towerManager.IsTowerSelected(tileX, tileY);
                    if (selectedTower != null)
                    {
                        selectedTower.Selected = true;
                        towerSelected = true;
                        selectedTowerPos = selectedTower.Position;

                        if (selectedTower is ArrowTower || selectedTower is BombTower)
                        {
                            selectedType = 1;
                        }

                        else
                        {
                            selectedType = 2;
                        }
                    }

                    else
                    {
                        towerSelected = false;
                        CreateTower();
                    }
                }
            }

            if (towerSelected)
            {
                if (keystate.IsKeyDown(Keys.U) && prevKeystate.IsKeyUp(Keys.U))
                {
                    if (money >= selectedTower.CostNextLevel && selectedTower.Tier != 3)
                    {
                        money -= selectedTower.CostNextLevel;
                        selectedTower.Upgrade();
                    }
                }
            }

            towerManager.Update(gameTime, waveManager.GetEnemies());

            foreach (Enemy enemy in waveManager.GetEnemies())
            {
                if (enemy.IsDead)
                {
                    money += enemy.BountyGiven;
                }
            }

            prevKeystate = keystate;
            oldState = mouseState;



            audioEngine.Update();

        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise Game

            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate,
                            SaveStateMode.None, camera.TransformMatrix);

            level.Draw(spriteBatch);

            DrawCursor();

            waveManager.Draw(spriteBatch);
            towerManager.Draw(spriteBatch);

            int round = waveManager.Round;

            guiManager.Draw(spriteBatch, round, money, lives);

            if (towerSelected)
            {
                Rectangle rect = Engine.CreateRectForCell(new Point((int)((selectedTowerPos.X + 16) / Engine.TileWidth),
                                                                    (int)(selectedTowerPos.Y + 16) / Engine.TileHeight));

                Vector2 center = new Vector2(rect.X + rect.Width / 2,
                                             rect.Y + rect.Height / 2);

                if (selectedType == 1)
                    spriteBatch.Draw(arrowRange, Vector2.Subtract(center, new Vector2(arrowRange.Width / 2)),
                        new Color(new Vector4(0, 0, 0, 0.5f)));
                else if (selectedType == 2)
                    spriteBatch.Draw(spikeRange, Vector2.Subtract(center, new Vector2(spikeRange.Width / 2)),
                        new Color(new Vector4(0, 0, 0, 0.5f)));
            }

            if (tower != null)
                towerPreview.Draw(spriteBatch, cellX, cellY);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawCursor()
        {
            if (IsInBounds(cellX, cellY))
            {
                Rectangle rect = Engine.CreateRectForCell(new Point(cellX, cellY));

                if (level.GetCellIndex(cellX, cellY) != 0)
                    spriteBatch.Draw(cursor, rect, Color.Red);
                else
                    spriteBatch.Draw(cursor, rect, Color.White);
            }

            // If the game is transitioning on or off, fade it out to black.
            //if (TransitionPosition > 0)
              //  ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
