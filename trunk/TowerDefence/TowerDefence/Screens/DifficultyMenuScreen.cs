#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class DifficultyMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public DifficultyMenuScreen()
            : base("Zombifizierungsgrad")
        {
            // Create our menu entries.
            MenuEntry easyMenuEntry = new MenuEntry("Heiter bis zombig");
            MenuEntry normalMenuEntry = new MenuEntry("Ziemlich Zombifiziert");
            MenuEntry hardMenuEntry = new MenuEntry("Zombie Apokalypse");
            MenuEntry backMenuEntry = new MenuEntry("Zurueck");

            // Hook up menu event handlers.
            easyMenuEntry.Selected += easyEntrySelected;
            normalMenuEntry.Selected += normalEntrySelected;
            hardMenuEntry.Selected += hardEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(easyMenuEntry);
            MenuEntries.Add(normalMenuEntry);
            MenuEntries.Add(hardMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void easyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void normalEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void hardEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        #endregion
    }
}
