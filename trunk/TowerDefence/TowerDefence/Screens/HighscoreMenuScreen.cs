#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
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
    /// The highscore screen is brought up over the top of the main menu
    /// screen, and gives the user a overview of the best Players
    /// </summary>
    class HighscoreMenuScreen : MenuScreen
    {

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HighscoreMenuScreen()
            : base("Beste Zombiekiller")
        {
            // Create our menu entries.
            MenuEntry oneEntry = new MenuEntry("01 - Mr Racer - 932");
            MenuEntry twoEntry = new MenuEntry("02 - Schoof - 921");
            MenuEntry threeEntry = new MenuEntry("03 - Frost - 911");
            MenuEntry fourEntry = new MenuEntry("04 - Schoof - 907");
            MenuEntry fiveEntry = new MenuEntry("05 - Frost - 901");
            MenuEntry sixEntry = new MenuEntry("06 - Schoof - 846");
            MenuEntry sevenEntry = new MenuEntry("07 - Frost - 817");
            MenuEntry eightEntry = new MenuEntry("08 - Schoof - 601");
            MenuEntry nineEntry = new MenuEntry("09 - Schoof - 511");
            MenuEntry tenEntry = new MenuEntry("10 - Schoof - 509");

            // Hook up menu event handlers.
            oneEntry.Selected += OnCancel;
            twoEntry.Selected += OnCancel;
            threeEntry.Selected += OnCancel;
            fourEntry.Selected += OnCancel;
            fiveEntry.Selected += OnCancel;
            sixEntry.Selected += OnCancel;
            sevenEntry.Selected += OnCancel;
            eightEntry.Selected += OnCancel;
            nineEntry.Selected += OnCancel;
            tenEntry.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(oneEntry);
            MenuEntries.Add(twoEntry);
            MenuEntries.Add(threeEntry);
            MenuEntries.Add(fourEntry);
            MenuEntries.Add(fiveEntry);
            MenuEntries.Add(sixEntry);
            MenuEntries.Add(sevenEntry);
            MenuEntries.Add(eightEntry);
            MenuEntries.Add(nineEntry);
            MenuEntries.Add(tenEntry);
        }



        #endregion
    }
}
