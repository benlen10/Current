using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Properties

        public static IGame CurrentGame;
        public static IUser CurrentUser;
        static IConsole CurrentConsole;
        static IConsole CurrentEmulator;
        public static string DefaultUsername;
        public static int ShowSplashScreen;
        public static int ScanOnStartup;
        public static int RestrictESRB;
        public static int RequireLogin;
        public static int PerferCmdInterface;
        public static int ShowLoadingScreen;
        public static int PayPerPlayEnabled;
        public static int CoinsRequired;
        public static int Playtime;
        public static int LaunchOptions;
        public static int DisplayEsrbWhileBrowsing;
        public static int PasswordProtection;
        public static int EnforceFileExtensions;

        #endregion

        #region Class Methods

        /// <summary>
        /// Public constructor for the SettingsWindowClass
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            Populate();
        }

        /// <summary>
        /// Called on window close event
        /// </summary>
        void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            MainWindow.ReHookKeys();
        }

        /// <summary>
        /// Populate settings window fields under all tabs
        /// </summary>
        private void Populate()
        {
            //Populate console list with the currently active games
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                GamesTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
                EmulatorsTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
            }

            //Set initial selected indexes
            if (EmulatorsTab_Listbox_ConsoleList.HasItems)
            {
                EmulatorsTab_Listbox_ConsoleList.SelectedIndex = 0;
            }

            //Poplate ESRB dropdown combo boxes
            GlobalTab_Dropdown_AllowedESRB.Items.Add("Everyone");
            GlobalTab_Dropdown_AllowedESRB.Items.Add("Everyone 10+");
            GlobalTab_Dropdown_AllowedESRB.Items.Add("Teen");
            GlobalTab_Dropdown_AllowedESRB.Items.Add("Mature");
            GlobalTab_Dropdown_AllowedESRB.Items.Add("Adults Only (AO)");
            GlobalTab_Dropdown_AllowedESRB.Items.Add("None");
            UsersTab_Dropdown_AllowedESRB.Items.Add("Everyone");
            UsersTab_Dropdown_AllowedESRB.Items.Add("Everyone 10+");
            UsersTab_Dropdown_AllowedESRB.Items.Add("Teen");
            UsersTab_Dropdown_AllowedESRB.Items.Add("Mature");
            UsersTab_Dropdown_AllowedESRB.Items.Add("Adults Only (AO)");
            UsersTab_Dropdown_AllowedESRB.Items.Add("None");

            //Load UniCade Logo images within the settings window
            AboutTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            CloudTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            EmulatorsTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            WebTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));

            //Populate the 'Allowed ESRB' combo box with the specified rating
            if (RestrictESRB == 0)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "None";
            }
            else if (RestrictESRB == 1)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "Everyone";
            }
            else if (RestrictESRB == 2)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "Everyone 10+";
            }
            else if (RestrictESRB == 3)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "Teen";
            }
            else if (RestrictESRB == 4)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "Mature";
            }
            else if (RestrictESRB == 5)
            {
                GlobalTab_Dropdown_AllowedESRB.Text = "Adults Only (AO)";
            }

            if (DisplayEsrbWhileBrowsing > 0)
            {
                GamesTab_CheckBox__GlobalFavorite.IsChecked = true;
            }

            //Disable editing userinfo unless logged in
            UsersTab_Textbox_Username.IsEnabled = false;
            UsersTab_Textbox_Email.IsEnabled = false;
            UsersTab_Textbox_UserInfo.IsEnabled = false;

            //Set specific textboxes as readonly
            UsersTab_Textbox_LoginCount.IsEnabled = false;
            UsersTab_Textbox_LaunchCount.IsEnabled = false;
            GlobalTab_Textbox_Coins.IsEnabled = false;
            GlobalTab_Textbox_Playtime.IsEnabled = false;

            //Populate features textbox under the About tab
            AboutTab_Textbox_SoftwareInfo.Text = TextFiles.features + "\n\n\n\n\n\n" + TextFiles.instructions;
            AboutTab_Textbox_SoftwareInfo.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            AboutTab_Textbox_SoftwareInfo.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;

            //Populate textbox fields
            GlobalTab_Textbox_Password.Password = PasswordProtection.ToString();
            GlobalTab_Textbox_DatabasePath.Text = Program.DatabasePath;
            GlobalTab_Textbox_EmulatorDirectory.Text = Program.EmulatorPath;
            GlobalTab_Textbox_MedaDirectory.Text = Program.MediaPath;
            GlobalTab_Textbox_ROMDirectory.Text = Program.RomPath;

            //Check specified boxes under the Web tab
            if (WebOps.releaseDate > 0)
            {
                WebTab_Checkbox_ReleaseDate.IsChecked = true;
            }

            if (WebOps.critic > 0)
            {
                WebTab_Checkbox_CriticScore.IsChecked = true;
            }

            if (WebOps.publisher > 0)
            {
                WebTab_Checkbox_Publisher.IsChecked = true;
            }

            if (WebOps.developer > 0)
            {
                WebTab_Checkbox_Developer.IsChecked = true;
            }

            if (WebOps.esrb > 0)
            {
                WebTab_Checkbox_ESRBRating.IsChecked = true;
            }

            if (WebOps.esrbDescriptor > 0)
            {
                WebTab_Checkbox_ESRBDescriptor.IsChecked = true;
            }

            if (WebOps.players > 0)
            {
                WebTab_Checkbox_Players.IsChecked = true;
            }

            if (WebOps.description > 0)
            {
                WebTab_Checkbox_Description.IsChecked = true;
            }

            if (WebOps.boxFront > 0)
            {
                WebTab_Checkbox_BoxFront.IsChecked = true;
            }

            if (WebOps.boxBack > 0)
            {
                WebTab_Checkbox_BoxBack.IsChecked = true;
            }

            if (WebOps.screenshot > 0)
            {
                WebTab_Checkbox_Screenshot.IsChecked = true;
            }

            if (WebOps.metac > 0)
            {
                WebTab_Checkbox_Metacritic.IsChecked = true;
            }

            if (WebOps.mobyg > 0)
            {
                WebTab_Checkbox_Mobygames1.IsChecked = true;
            }

            //Populate Global Settings checkboxes
            if (ShowSplashScreen > 0)
            {
                GlobalTab_Checkbox_DisplaySplash.IsChecked = true;
            }

            if (ShowLoadingScreen > 0)
            {
                GlobalTab_Checkbox_DisplayLoadingScreen.IsChecked = true;
            }

            if (RequireLogin > 0)
            {
                GlobalTab_Checkbox_RequireLogin.IsChecked = true;
            }

            if (ScanOnStartup > 0)
            {
                GlobalTab_Checkbox_RescanAllLibraries.IsChecked = true;
            }

            if (EnforceFileExtensions > 0)
            {
                EmulatorsTab_Checkbox_EnforceFileExtension.IsChecked = true;
            }

            if (DisplayEsrbWhileBrowsing == 1)
            {
                GlobalTab_Checkbox_DisplayESRB.IsChecked = true;
            }

            if (PayPerPlayEnabled > 0)
            {
                GlobalTab_Checkbox_EnablePayPerPlay.IsChecked = true;
                GlobalTab_Textbox_Coins.IsEnabled = true;
                GlobalTab_Textbox_Playtime.IsEnabled = true;
            }

            //Populate payPerPlay fields
            GlobalTab_Textbox_Coins.Text = CoinsRequired.ToString();
            GlobalTab_Textbox_Playtime.Text = Playtime.ToString();

            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                UsersTab_Listbox_CurrentUser.Items.Add(user.Username);
            }

            //Refresh the global favorites list
            RefreshGlobalFavs();

            //Populate user license info
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + Program.UserLicenseName;
            if (Program.IsLicenseValid)
            {
                AboutTab_Label_Edition.Content = "License Status: Full Version";
            }
            else
            {
                AboutTab_Label_Edition.Content = "License Status: Invalid";
            }
            AboutTab_Label_LicenseKey.Content = "License Key: " + Program.UserLicenseKey;
        }

        #endregion

        #region Games Tab

        /// <summary>
        /// Download game button
        /// Download metadata for the selected game from UniCade Cloud
        /// </summary>
        private void GamesTab_DownloadGameButton_Click(object sender, RoutedEventArgs e)
        {
            //Check if a UniCade Cloud user is currently active
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            //Invalid input checks
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to download");
                return;
            }

            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }

            if (CurrentGame == null)
            {
                MessageBox.Show("Must select a game");
                return;
            }
            IGame game = null;
            game = SQLclient.GetSingleGame(CurrentGame.ConsoleName, CurrentGame.Title);
            if (game != null)
            {
                for (int i = 0; i < CurrentConsole.GameList.Count; i++)
                {
                    IGame g = (IGame)CurrentConsole.GameList[i];
                    if (game.FileName.Equals(g.FileName))
                    {
                        CurrentConsole.GameList[i] = game;
                        RefreshGameInfo(game);
                        MessageBox.Show("Game Metadata Downloaded");
                        return;
                    }
                }
            }
            MessageBox.Show("Download successful");
        }

        /// <summary>
        /// Upload console button
        /// Upload all games from the selected console to UniCade Cloud
        /// </summary>
        private void GamesTab_UploadConsoleButton_Click(object sender, EventArgs e)
        {
            //Invalid input checks
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to upload");
                return;
            }

            //Upload all games if all initial checks are passed
            foreach (IGame g in CurrentConsole.GameList)
            {
                SQLclient.UploadGame(g);
                MessageBox.Show("Console Uploaded");
            }
        }

        /// <summary>
        /// Download console info button
        /// Downloads all game metadata for the current console from the current user's Unicade Cloud account
        /// </summary>
        private void GamesTab_DownloadConsoleButton_Click(object sender, EventArgs e)
        {
            //Invalid input checks
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to delete");
                return;
            }
            if (CurrentConsole == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            for (int i = 0; i < CurrentConsole.GameList.Count; i++)
            {
                IGame game1 = (IGame)CurrentConsole.GameList[i];
                IGame game2 = null;
                game2 = SQLclient.GetSingleGame(game1.ConsoleName, game1.Title);
                if (game2 != null)
                {
                    if (game2.FileName.Length > 3)
                    {
                        CurrentConsole.GameList[i] = game2;
                    }
                }
            }

            //Refresh the current game info
            MessageBox.Show("Download successful");
            RefreshGameInfo(CurrentGame);
        }

        /// <summary>
        /// Called when the select index is changed. Update the proper game info in the details fields. 
        /// </summary>
        private void GamesTab_GamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_GamesList.SelectedItem == null) { return; }
            string currentGame = GamesTab_Listbox_GamesList.SelectedItem.ToString();
            foreach (IGame g in CurrentConsole.GameList)
            {
                if (g.Title.Equals(currentGame))
                {
                    SettingsWindow.CurrentGame = g;
                }
            }
            RefreshGameInfo(SettingsWindow.CurrentGame);
            RefreshEsrbIcon(SettingsWindow.CurrentGame);
        }

        /// <summary>
        /// Called when the select index is changed for the console listbox. Update the games list for the selected console. 
        /// </summary>
        private void GamesTab_ConsoleListBox__SelectedIndexChanged(object sender, EventArgs e)
        {
            if(GamesTab_Listbox_ConsoleList.SelectedItem == null) { return; }
            string curItem = GamesTab_Listbox_ConsoleList.SelectedItem.ToString();
            GamesTab_Listbox_GamesList.Items.Clear();
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                if (console.ConsoleName.Equals(curItem))
                {
                    CurrentConsole = console;
                    GamesTab_Textbox_GamesForConsole.Text = console.GameCount.ToString();
                    GamesTab_Textbox_TotalGames.Text = Program.ActiveDatabase.TotalGameCount.ToString();
                    if (console.GameCount > 0)
                    {
                        foreach (IGame g in console.GameList)
                        {
                            GamesTab_Listbox_GamesList.Items.Add(g.Title);
                        }
                    }
                }
            }
            if (GamesTab_Listbox_GamesList.Items.Count > 0)
            {
                GamesTab_Listbox_GamesList.SelectedIndex = 0;
                foreach (IGame g in CurrentConsole.GameList)
                {
                    if (g.Title.Equals(GamesTab_Listbox_GamesList.SelectedItem.ToString()))
                    {
                        CurrentGame = g;
                    }
                }
            }
            else
            {
                RefreshGameInfo(null);
            }
        }

        /// <summary>
        /// Rescrape game info button.
        /// Rescrapes info the the specified game from the web
        /// </summary>
        private void GamesTab_RescrapeGameButton_Click(object sender, EventArgs e)
        {
            //Require that a user select a valid game to rescrape
            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }

            //Scrape info and populate local fields
            WebOps.ScrapeInfo(CurrentGame);
            GamesTab_Textbox_Title.Text = CurrentGame.Title;
            GamesTab_Textbox_Console.Text = CurrentGame.ConsoleName;
            GamesTab_Textbox_ReleaseDate.Text = CurrentGame.ReleaseDate;
            GamesTab_Textbox_CriticScore.Text = CurrentGame.CriticScore;
            GamesTab_Textbox_Publisher.Text = CurrentGame.Publisher;
            GamesTab_Textbox_Developer.Text = CurrentGame.Developer;
            GamesTab_Textbox_ESRB.Text = CurrentGame.Esrb;
            GamesTab_Textbox_Players.Text = CurrentGame.Players;
            GamesTab_Textbox_ESRBDescriptor.Text = CurrentGame.EsrbDescriptor;
            GamesTab_Textbox_Description.Text = CurrentGame.Description;
            RefreshEsrbIcon(CurrentGame);
        }

        /// <summary>
        /// Save database button
        /// Save all active info to the text databse
        /// </summary>
        private void GamesTab_SaveToDatabaseButton_Click(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            SaveGameInfo();
            MessageBox.Show("Success");
        }

        /// <summary>
        /// Save game info button
        /// </summary>
        private void GamesTab_SaveInfoButton_Click(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to save");
                return;
            }
            SaveGameInfo();
        }

        /// <summary>
        /// Uplod game button
        /// Upload the currently selected game to UniCade cloud
        /// </summary>
        private void GamesTab_UploadButton_Click(object sender, EventArgs e)
        {

            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud login required");
                return;
            }

            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to upload");
                return;
            }

            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            SQLclient.UploadGame(CurrentGame);
            MessageBox.Show("Game Uploaded");
        }

        /// <summary>
        /// Sets the current game as a global favorite
        /// </summary>
        private void GamesTab_FavoriteCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Verify that a console/game is currently selected
            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            //Toggle favorite checkbox
            if (GamesTab_CheckBox__GlobalFavorite.IsChecked.Value == true)
            {
                CurrentGame.Favorite = 1;
            }
            else
            {
                CurrentGame.Favorite = 0;
            }
        }

        /// <summary>
        /// Rescrape console button
        /// Rescrape metadata for all games within teh current console
        /// </summary>
        private void GamesTab_RescrapeConsoleMetadataButton_Click(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }

            MessageBox.Show("This may take a while... Please wait for a completed nofication.");
            foreach (IGame game1 in CurrentConsole.GameList)
            {
                WebOps.ScrapeInfo(game1);
            }
            MessageBox.Show("Operation Successful");
        }

        #endregion

        #region Emulators Tab

        /// <summary>
        /// Update the console info fields when the selected console is changed
        /// </summary>
        private void EmulatorsTab_ConsoleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmulatorsTab_Listbox_ConsoleList.SelectedItem == null) { return; }
            string curItem = EmulatorsTab_Listbox_ConsoleList.SelectedItem.ToString();
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                if (console.ConsoleName.Equals(curItem))
                {
                    CurrentEmulator = console;
                    EmulatorsTab_Textbox_ConsoleName1.Text = console.ConsoleName;
                    GlobalTab_Textbox_EmulatorDirectory.Text = console.EmulatorPath;
                    EmulatorsTab_Textbox_ROMExtension.Text = console.RomExtension;
                    EmulatorsTab_Textbox_EmulatorArgs.Text = console.LaunchParams;
                    EmulatorsTab_Textbox_ConsoleInfo.Text = console.ConsoleInfo;
                    EmulatorsTab_Textbox_GameCount.Text = console.GameCount.ToString();
                    EmulatorsTab_Textbox_ReleaseDate.Text = console.ReleaseDate;
                }
            }
        }

        /// <summary>
        /// Save console button
        /// Save current console info to database file
        /// </summary>
        private void EmulatorsTab_SaveDatabaseFileButton_Click(object sender, EventArgs e)
        {
            CurrentEmulator.ConsoleName = EmulatorsTab_Textbox_ConsoleName1.Text;
            CurrentEmulator.EmulatorPath = GlobalTab_Textbox_EmulatorDirectory.Text;
            CurrentEmulator.RomExtension = EmulatorsTab_Textbox_ROMExtension.Text;
            CurrentEmulator.LaunchParams = EmulatorsTab_Textbox_EmulatorArgs.Text;
            CurrentEmulator.ReleaseDate = EmulatorsTab_Textbox_ReleaseDate.Text;
            CurrentEmulator.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
            FileOps.SaveDatabase(Program.DatabasePath);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void EmulatorsTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Delete console button
        /// Deletes a consle and all associated games from the database
        /// </summary>
        private void EmulatorsTab_DeleteConsoleButton_Click(object sender, EventArgs e)
        {
            //Ensure that at least one console exists
            if (Program.ActiveDatabase.ConsoleList.Count < 2)
            {
                MessageBox.Show("Cannot have an empty console list");
                return;
            }
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            Program.ActiveDatabase.ConsoleList.Remove(CurrentEmulator);
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                EmulatorsTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
                GamesTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
            }
            EmulatorsTab_Listbox_ConsoleList.SelectedIndex = 0;

            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// Add a new console
        /// </summary>
        private void EmulatorsTab_AddNewConsoleButton_Click(object sender, EventArgs e)
        {
            //Clear all text boxes initially 
            GlobalTab_Textbox_EmulatorDirectory.Text = null;
            EmulatorsTab_Textbox_ROMExtension.Text = null;
            EmulatorsTab_Textbox_EmulatorArgs.Text = null;
            EmulatorsTab_Textbox_ConsoleInfo.Text = null;
            EmulatorsTab_Textbox_GameCount.Text = null;
            EmulatorsTab_Textbox_ReleaseDate.Text = null;

            //Create a new console and add it to the datbase
            string newConsoleName = "New Console";
            IConsole console = new Console(newConsoleName);

            Program.ActiveDatabase.ConsoleList.Add(console);
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            foreach (IConsole con in Program.ActiveDatabase.ConsoleList)
            {
                EmulatorsTab_Listbox_ConsoleList.Items.Add(con.ConsoleName);
                GamesTab_Listbox_ConsoleList.Items.Add(con.ConsoleName);
            }
            GamesTab_Listbox_ConsoleList.SelectedIndex = (EmulatorsTab_Listbox_ConsoleList.Items.Count - 1);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// //Force metadata rescrape (All games within console)
        /// </summary>
        private void EmulatorsTab_ForceGlobalMetadataRescrapeButton_Click(object sender, EventArgs e)
        {
            foreach (IGame game in CurrentEmulator.GameList)
            {
                if (!WebOps.ScrapeInfo(game))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Save the custom info fields for the current emulator
        /// </summary>
        private void EmulatorsTab_SaveInfoButton_Click(object sender, EventArgs e)
        {
            //Invalid input check
            if (EmulatorsTab_Textbox_ConsoleName1.Text.Contains("|") || GlobalTab_Textbox_EmulatorDirectory.Text.Contains("|") || GamesTab_Textbox_TotalGames.Text.Contains("|") || EmulatorsTab_Textbox_ROMExtension.Text.Contains("|") || EmulatorsTab_Textbox_EmulatorArgs.Text.Contains("|") || EmulatorsTab_Textbox_ReleaseDate.Text.Contains("|") || EmulatorsTab_Textbox_ConsoleInfo.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
                {
                    if (GamesTab_Textbox_ReleaseDate.Text.Length < 5)
                    {
                        CurrentEmulator.ReleaseDate = EmulatorsTab_Textbox_ReleaseDate.Text;
                    }
                    else
                    {
                        MessageBox.Show("Release Date Invalid");
                    }
                }
                else
                {
                    MessageBox.Show("Release Date score must be only digits");
                }

                if ((EmulatorsTab_Textbox_ConsoleName1.Text.Length > 20) || (GlobalTab_Textbox_EmulatorDirectory.Text.Length > 100) || (GamesTab_Textbox_TotalGames.Text.Length > 100) || (EmulatorsTab_Textbox_ROMExtension.Text.Length > 30) || (EmulatorsTab_Textbox_ROMExtension.Text.Length > 300))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    //If all input checks are valid, set console into to the current text field values
                    CurrentEmulator.ConsoleName = EmulatorsTab_Textbox_ConsoleName1.Text;
                    CurrentEmulator.EmulatorPath = GlobalTab_Textbox_EmulatorDirectory.Text;
                    CurrentEmulator.RomExtension = EmulatorsTab_Textbox_ROMExtension.Text;
                    CurrentEmulator.LaunchParams = EmulatorsTab_Textbox_EmulatorArgs.Text;
                    CurrentEmulator.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
                }
                MainWindow.RefreshConsoleList();
            }

            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                EmulatorsTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
            }
        }

        /// <summary>
        /// Toggle enforceExt checkbox
        /// </summary>
        private void EmulatorsTab_EnforceROMExtensionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (EmulatorsTab_Checkbox_EnforceFileExtension.IsChecked.Value == true)
            {
                EnforceFileExtensions = 1;
            }
            else
            {
                EnforceFileExtensions = 0;
            }
        }

        /// <summary>
        /// Rescan all games across all emulators
        /// </summary>
        private void EmulatorsTab_GlobalRescanButton_Click(object sender, EventArgs e)
        {
            if (FileOps.Scan(Program.RomPath))
            {
                MessageBox.Show("Global Rescan Successful");
            }
        }

        /// <summary>
        /// Rescan console button
        /// Rescans all ROM files for the current console
        /// </summary>
        private void EmulatorsTab_RescanSingleConsoleButton_Click(object sender, EventArgs e)
        {
            //Ensure that a console is currently selected
            if (EmulatorsTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                if (console.ConsoleName.Equals(EmulatorsTab_Listbox_ConsoleList.SelectedItem.ToString()))
                {
                    if (FileOps.ScanDirectory(console.RomPath, Program.RomPath))
                    {
                        MessageBox.Show(console.ConsoleName + " Successfully Scanned");
                    }
                    else
                    {
                        MessageBox.Show(console.ConsoleName + " Library Rescan Failed");
                    }
                    break;
                }
            }
        }

        #endregion

        #region Users Tab

        /// <summary>
        /// Close and save button
        /// </summary>
        private void UsersTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            FileOps.SavePreferences(Program.PreferencesPath);
            this.Close();
        }

        /// <summary>
        /// Refresh user info under the User tab every time a new user is selected
        /// </summary>
        private void UsersTab_UsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update the current user text         
            if (CurrentUser != null)
            {
                UsersTab_Label_CurrentUser.Content = "Current User: " + CurrentUser.Username;
            }

            //Populate the favorites list for each user
            UsersTab_Listbox_UserFavorites.Items.Clear();
            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                if (user.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
                {
                    if (user.Favorites.Count > 0)
                    {
                        foreach (IGame game in user.Favorites)
                        {
                            UsersTab_Listbox_UserFavorites.Items.Add(game.Title + " - " + game.ConsoleName);
                        }
                    }

                    UsersTab_Textbox_Username.Text = user.Username;
                    UsersTab_Textbox_Email.Text = user.Email;
                    UsersTab_Textbox_UserInfo.Text = user.UserInfo;
                    UsersTab_Textbox_LoginCount.Text = user.LoginCount.ToString();
                    UsersTab_Textbox_LaunchCount.Text = user.TotalLaunchCount.ToString();
                    UsersTab_Dropdown_AllowedESRB.Text = user.AllowedEsrb;

                    //Only allow the current user to edit their own userdata
                    bool editEnabled = user.Username.Equals(CurrentUser.Username);
                    UsersTab_Textbox_Username.IsEnabled = true;
                    UsersTab_Textbox_Email.IsEnabled = true;
                    UsersTab_Textbox_UserInfo.IsEnabled = true;
                    UsersTab_Textbox_LoginCount.IsEnabled = true;
                    UsersTab_Textbox_LaunchCount.IsEnabled = true;
                    UsersTab_Dropdown_AllowedESRB.IsEnabled = true;
                    UsersTab_Listbox_UserFavorites.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Create new user button
        /// Create a new user and save the userdata to the preferences file
        /// </summary>
        private void UsersTab_NewUserButton_Click(object sender, EventArgs e)
        {
            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                if (CurrentUser.Username.Equals(user.Username))
                {
                    Program.ActiveDatabase.UserList.Remove(user);
                    Program.ActiveDatabase.UserList.Add(CurrentUser);
                    break;
                }
            }

            //Create a new unicade account and display the dialog
            AccountWindow uc = new AccountWindow(1);
            uc.ShowDialog();

            //Update the current labels and save the user info to the preferences file
            UsersTab_Label_CurrentUser.Content = "Current User: " + CurrentUser.Username;
            FileOps.SavePreferences(Program.PreferencesPath);

            //Refresh the listbox contents
            UsersTab_Listbox_CurrentUser.Items.Clear();
            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                UsersTab_Listbox_CurrentUser.Items.Add(user.Username);
            }
        }

        /// <summary>
        /// Save button
        /// </summary>
        private void UsersTab_SaveAllUsersButton_Click(object sender, EventArgs e)
        {
            FileOps.SavePreferences(Program.PreferencesPath);
        }

        /// <summary>
        /// Delete the currently selected user from the database
        /// </summary>
        private void UsersTab_DeleteUserButton_Click(object sender, EventArgs e)
        {
            //Ensure that there is always at least one user present in the database
            if (Program.ActiveDatabase.UserList.Count <= 1)
            {
                MessageBox.Show("Must at least have one user");
                return;
            }

            //Remove the user and refresh the database
            Program.ActiveDatabase.UserList.Remove(CurrentUser);
            UsersTab_Listbox_CurrentUser.Items.Clear();
            CurrentUser = null;
            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                UsersTab_Listbox_CurrentUser.Items.Add(user.Username);
            }
        }

        /// <summary>
        /// Save button (Global Settings tab)
        /// Save the current global settings to the preferences file
        /// </summary>
        private void UsersTab_SaveButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currently logged in
            if (!CurrentUser.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            if (UsersTab_Textbox_Username.Text.Contains("|") || UsersTab_Textbox_Email.Text.Contains("|") || UsersTab_Textbox_UserInfo.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if ((UsersTab_Textbox_Username.Text.Length > 20) || (UsersTab_Textbox_Email.Text.Length > 20) || (UsersTab_Textbox_UserInfo.Text.Length > 50))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    CurrentUser.Username = UsersTab_Textbox_Username.Text;
                    CurrentUser.Pass = UsersTab_Textbox_Email.Text;
                    CurrentUser.UserInfo = UsersTab_Textbox_UserInfo.Text;
                }

                if (GamesTab_Textbox_ESRB.Text.Contains("Everyone") || GamesTab_Textbox_ESRB.Text.Contains("Teen") || GamesTab_Textbox_ESRB.Text.Contains("Mature") || GamesTab_Textbox_ESRB.Text.Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                {
                    if (UsersTab_Dropdown_AllowedESRB.SelectedItem != null)
                    {
                        CurrentUser.AllowedEsrb = UsersTab_Dropdown_AllowedESRB.SelectedItem.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }
            }
            UsersTab_Listbox_CurrentUser.Items.Clear();

            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                UsersTab_Listbox_CurrentUser.Items.Add(user.Username);
            }
        }

        /// <summary>
        /// Delete user favorite
        /// </summary>
        private void UsersTab_DeleteFavoriteButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currenly logged in
            if (!CurrentUser.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            CurrentUser.Favorites.RemoveAt(UsersTab_Listbox_UserFavorites.SelectedIndex);
            UsersTab_Listbox_UserFavorites.Items.Clear();
            foreach (IGame g in CurrentUser.Favorites)
            {
                UsersTab_Listbox_UserFavorites.Items.Add(g.Title + " - " + g.ConsoleName);
            }
        }

        /// <summary>
        /// Login local user button
        /// </summary>
        private void UsersTab_LoginButton_Click(object sender, EventArgs e)
        {
            foreach (IUser user in Program.ActiveDatabase.UserList)
            {
                if (CurrentUser.Username.Equals(user.Username))
                {
                    Program.ActiveDatabase.UserList.Remove(user);
                    Program.ActiveDatabase.UserList.Add(CurrentUser);
                    break;
                }
            }

            //Display the login dialog
            LoginWindow login = new LoginWindow(1);
            login.ShowDialog();
            if (CurrentUser != null)
            {
                //If the user is logged in sucuesfully, save the current user and preferences file
                UsersTab_Label_CurrentUser.Content = "Current User: " + CurrentUser.Username;
                FileOps.SavePreferences(Program.PreferencesPath);
            }
        }

        /// <summary>
        /// Refresh the current users list and userdata
        /// </summary>
        private void UsersTab_RefreshButton_Click(object sender, EventArgs e)
        {
            UsersTab_Label_CurrentUser.Content = "Current User: " + CurrentUser.Username;
        }

        #endregion

        #region Global Settings Tab

        /// <summary>
        /// Modify the global ESRB rating when the dropdown is changed
        /// </summary>
        private void GlobalSettingsTab_AllowedEsrbRatingDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            RestrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
        }

        /// <summary>
        /// Save Global Settings button
        /// </summary>
        private void GlobalSettings_SavePreferenceFileButton_Click(object sender, EventArgs e)
        {
            if (GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("|") || GlobalTab_Textbox_EmulatorDirectory.Text.Contains("|") || GlobalTab_Textbox_MedaDirectory.Text.Contains("|") || GlobalTab_Textbox_ROMDirectory.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if (GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Everyone") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Teen") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Mature") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                {
                    RestrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }

                if ((GlobalTab_Textbox_EmulatorDirectory.Text.Length > 150) || (GlobalTab_Textbox_MedaDirectory.Text.Length > 150) || (GlobalTab_Textbox_ROMDirectory.Text.Length > 150))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    Program.EmulatorPath = GlobalTab_Textbox_EmulatorDirectory.Text;
                    Program.MediaPath = GlobalTab_Textbox_MedaDirectory.Text;
                    Program.RomPath = GlobalTab_Textbox_ROMDirectory.Text;
                }

                Int32.TryParse(GlobalTab_Textbox_Password.Password, out int n);
                if (n > 0)
                {
                    PasswordProtection = Int32.Parse(GlobalTab_Textbox_Password.Password);
                }

                Int32.TryParse(GlobalTab_Textbox_Coins.Text, out n);
                if (n > 0)
                {
                    CoinsRequired = Int32.Parse(GlobalTab_Textbox_Coins.Text);
                }

                if (GlobalTab_Dropdown_AllowedESRB.SelectedItem != null)
                {
                    RestrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
                }

                //Save all active preferences to the local preferences file
                FileOps.SavePreferences(Program.PreferencesPath);
            }
        }

        /// <summary>
        /// Toggle viewEsrb checkbox
        /// </summary>
        private void GlobalSettingsTab_AllowedToViewEsrbCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_ToView.IsChecked.Value == true)
            {
                DisplayEsrbWhileBrowsing = 1;
            }
            else
            {
                DisplayEsrbWhileBrowsing = 0;
            }
        }

        /// <summary>
        /// Toggle splash screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleSplashCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplaySplash.IsChecked.Value == true)
            {
                ShowSplashScreen = 1;
            }
            else
            {
                ShowSplashScreen = 0;
            }
        }

        /// <summary>
        /// Toggle show loading screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleLoadingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplayLoadingScreen.IsChecked.Value == true)
            {
                ShowLoadingScreen = 1;
            }
            else
            {
                ShowLoadingScreen = 0;
            }
        }

        /// <summary>
        /// Toggle require login checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleRequireLoginCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_RequireLogin.IsChecked.Value == true)
            {
                RequireLogin = 1;
            }
            else
            {
                RequireLogin = 0;
            }
        }

        /// <summary>
        /// Toggle scan on startup checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleScanOnStartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_RescanAllLibraries.IsChecked.Value == true)
            {
                ScanOnStartup = 1;
            }
            else
            {
                ScanOnStartup = 0;
            }
        }

        /// <summary>
        /// Toggle view ESRB checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleEsrbViewCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplayESRB.IsChecked.Value == true)
            {
                DisplayEsrbWhileBrowsing = 1;
            }
            else
            {
                DisplayEsrbWhileBrowsing = 0;
            }
        }

        /// <summary>
        /// Toggle payPerPlay checkbox
        /// </summary>
        private void GlobalSettingsTab_TogglePayPerPlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PayPerPlayEnabled = 0;
            GlobalTab_Textbox_Coins.IsEnabled = false;
            GlobalTab_Textbox_Playtime.IsEnabled = false;
            if (GlobalTab_Checkbox_EnablePayPerPlay.IsChecked.Value == true)
            {
                PayPerPlayEnabled = 1;
                GlobalTab_Textbox_Coins.IsEnabled = true;
                GlobalTab_Textbox_Playtime.IsEnabled = true;
            }
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void GlobalSettingsTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Refresh global favorites button
        /// </summary>
        private void GlobalSettingsTab_RefreshGlobalFavoritesButton_Click(object sender, EventArgs e)
        {
            RefreshGlobalFavs();
        }

        #endregion

        #region Web Options Tab

        /// <summary>
        /// Toggle Metacritic checkbox
        /// </summary>
        private void WebTab_Checkbox_Metacritic_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Metacritic.IsChecked.Value == true)
            {
                WebOps.metac = 1;
            }
            else
            {
                WebOps.metac = 0;
            }
        }

        /// <summary>
        /// Toggle Mobygames checkbox
        /// </summary>
        private void WebTab_Checkbox_Mobygames_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Mobygames1.IsChecked.Value == true)
            {
                WebOps.metac = 1;
            }
            else
            {
                WebOps.metac = 0;
            }
        }

        /// <summary>
        /// Toggle release date checkbox
        /// </summary>
        private void WebTab_Checkbox_ReleaseDate_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ReleaseDate.IsChecked.Value == true)
            {
                WebOps.releaseDate = 1;
            }
            else
            {
                WebOps.releaseDate = 0;
            }
        }

        /// <summary>
        /// Toggle critic score checkbox
        /// </summary>
        private void WebTab_Checkbox_CriticScore_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_CriticScore.IsChecked.Value == true)
            {
                WebOps.critic = 1;
            }
            else
            {
                WebOps.critic = 0;
            }
        }

        /// <summary>
        /// Toggle Publisher checkbox
        /// </summary>
        private void WebTab_Checkbox_Publisher_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Publisher.IsChecked.Value == true)
            {
                WebOps.publisher = 1;
            }
            else
            {
                WebOps.publisher = 0;
            }
        }

        /// <summary>
        /// Toggle developer checkbox
        /// </summary>
        private void WebTab_Checkbox_Developer_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Developer.IsChecked.Value == true)
            {
                WebOps.developer = 1;
            }
            else
            {
                WebOps.developer = 0;
            }
        }

        /// <summary>
        /// Toggle ESRB Rating checkbox
        /// </summary>
        private void WebTab_Checkbox_ESRBRating_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBRating.IsChecked.Value == true)
            {
                WebOps.esrb = 1;
            }
            else
            {
                WebOps.esrb = 0;
            }
        }

        /// <summary>
        /// Toggle ESRB Descriptor checkbox
        /// </summary>
        private void WebTab_Checkbox_ESRBDescriptor_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBDescriptor.IsChecked.Value == true)
            {
                WebOps.description = 1;
            }
            else
            {
                WebOps.description = 0;
            }
        }

        /// <summary>
        /// Toggle players checkbox
        /// </summary>
        private void WebTab_Checkbox_Players_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Players.IsChecked.Value == true)
            {
                WebOps.players = 1;
            }
            else
            {
                WebOps.players = 0;
            }
        }

        /// <summary>
        /// Toggle description checkbox
        /// </summary>
        private void WebTab_Checkbox_Description_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBDescriptor.IsChecked.Value == true)
            {
                WebOps.description = 1;
            }
            else
            {
                WebOps.description = 0;
            }
        }

        /// <summary>
        /// Toggle boxfront checkbox
        /// </summary>
        private void WebTab_Checkbox_BoxFront_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_BoxFront.IsChecked.Value == true)
            {
                WebOps.boxFront = 1;
            }
            else
            {
                WebOps.boxFront = 0;
            }
        }

        /// <summary>
        /// Toggle box back checkbox
        /// </summary>
        private void WebTab_Checkbox_BoxBack_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_BoxBack.IsChecked.Value == true)
            {
                WebOps.boxBack = 1;
            }
            else
            {
                WebOps.boxBack = 0;
            }
        }

        /// <summary>
        /// Toggle screenshot textbox
        /// </summary>
        private void WebTab_Checkbox_Screenshot_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Screenshot.IsChecked.Value == true)
            {
                WebOps.screenshot = 1;
            }
            else
            {
                WebOps.screenshot = 0;
            }
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void WebTab_Button_Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void WebTab_Button_SaveScraperSettings_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement
        }

        #endregion

        #region UniCade Cloud Tab

        /// <summary>
        /// Create new user button (UniCade Cloud tab)
        /// Create a new SQL UniCade Cloud user
        /// </summary>
        private void CloudTab_Button_CreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow ua = new AccountWindow(0);
            ua.ShowDialog();
        }

        /// <summary>
        /// Login button (UniCade Cloud tab)
        /// Login a UniCade Cloud SQL user
        /// </summary>
        private void CloudTab_Button_Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow l = new LoginWindow(0);
            l.ShowDialog();
            if (SQLclient.sqlUser != null)
            {
                WebTab_Label_CurrentWebUser.Content = "Current Web User: " + SQLclient.sqlUser;
            }
        }

        /// <summary>
        /// Logout button (UniCade Cloud tab)
        /// Logs out the current SQL user 
        /// </summary>
        private void CloudTab_Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            //Check if a user is actually logged in
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("User is already logged out");
                WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
                return;
            }

            //Log the current user out and update the interface
            SQLclient.sqlUser = null;
            WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
        }

        /// <summary>
        /// Delete user button
        /// Delete the SQL user and update the interface
        /// </summary>
        private void CloudTab_Button_DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            //Delete the current SQL user and update the label
            SQLclient.DeleteUser();
            WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
        }

        /// <summary>
        /// Upload all games button
        /// Upload all games across all consoles to UniCade Cloud
        /// </summary>
        private void CloudTab_Button_UploadAllGames_Click(object sender, RoutedEventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SQLclient.UploadAllGames();
            MessageBox.Show("Library successfully uploaded");
        }

        /// <summary>
        /// Delete all games from the current user's UniCade Cloud account
        /// </summary>
        private void CloudTab_Button_DeleteAllGamesInCloud_Click(object sender, RoutedEventArgs e)
        {
            //Check if a SQL user is currently logged in
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SQLclient.Deletegames();
            MessageBox.Show("Library successfully deleted");
        }

        /// <summary>
        /// Download all games button
        /// Download all game metadata across all consoles
        /// </summary>
        private void CloudTab_Button_DownloadAllGames_Click(object sender, RoutedEventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SQLclient.DownloadAllGames();
            MessageBox.Show("Library metadata sucuessfully updated");
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void CloudTab_Button_EndSession_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement
        }

        #endregion

        #region About Tab

        /// <summary>
        /// Enter license button
        /// </summary>
        private void AboutTab_Button_EnterLicenseKey_Click(object sender, RoutedEventArgs e)
        {
            //Create a new license entry info and validate the key
            LicenseEntry le = new LicenseEntry();
            le.ShowDialog();
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + Program.UserLicenseName;
            AboutTab_Label_LicenseKey.Content = "License Key: " + Program.UserLicenseKey;

            //Set the license text depending on if the key is valid
            if (Program.IsLicenseValid == true)
            {
                AboutTab_Label_Edition.Content = "License Status: Full Version";
            }
            else
            {
                AboutTab_Label_Edition.Content = "License Status: Invalid";
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Verify that a string contains only numeric chars
        /// </summary>
        bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Given the string value for an esrb rating, calculate and return the ESRB int value
        /// </summary>
        public static int CalcEsrb(String esrb)
        {
            int EsrbNum = 0;
            if (esrb.Equals("Everyone"))
            {
                EsrbNum = 1;
            }
            else if (esrb.Equals("Everyone 10+"))
            {
                EsrbNum = 2;
            }
            else if (esrb.Equals("Teen"))
            {
                EsrbNum = 3;
            }
            else if (esrb.Equals("Mature"))
            {
                EsrbNum = 4;
            }
            else if (esrb.Equals("Adults Only (AO)"))
            {
                EsrbNum = 5;
            }
            else if (esrb.Equals("None"))
            {
                EsrbNum = 0;
            }
            else
            {
                EsrbNum = 0;
            }

            return EsrbNum;
        }

        /// <summary>
        /// Refresh the current game info passed in as a Game object
        /// </summary>
        public void RefreshGameInfo(IGame game)
        {
            if (game == null)
            {
                //If no game is currently selected, set all info fields to null
                GamesTab_Textbox_Title.Text = null;
                GamesTab_Textbox_Console.Text = null;
                GamesTab_Textbox_ReleaseDate.Text = null;
                GamesTab_Textbox_CriticScore.Text = null;
                GamesTab_Textbox_Publisher.Text = null;
                GamesTab_Textbox_Developer.Text = null;
                GamesTab_Textbox_ESRB.Text = null;
                GamesTab_Textbox_Players.Text = null;
                GamesTab_Textbox_ESRBDescriptor.Text = null;
                GamesTab_Textbox_Description.Text = null;
                return;
            }

            //If a valid game is selected, update all info fields
            GamesTab_Textbox_Title.Text = game.Title;
            GamesTab_Textbox_Console.Text = game.ConsoleName;
            GamesTab_Textbox_ReleaseDate.Text = game.ReleaseDate;
            GamesTab_Textbox_CriticScore.Text = game.CriticScore;
            GamesTab_Textbox_Publisher.Text = game.Publisher;
            GamesTab_Textbox_Developer.Text = game.Developer;
            GamesTab_Textbox_ESRB.Text = game.Esrb;
            GamesTab_Textbox_Players.Text = game.Players;
            GamesTab_Textbox_ESRBDescriptor.Text = game.EsrbDescriptor;
            GamesTab_Textbox_Description.Text = game.Description;

            //Set favorite checkbox
            if (game.Favorite == 1)
            {
                GamesTab_CheckBox__GlobalFavorite.IsChecked = true;
            }
            else
            {
                GamesTab_CheckBox__GlobalFavorite.IsChecked = false;
            }

            GamesTab_Image_Boxfront.Source = null;
            GamesTab_Image_Boxback.Source = null;
            GamesTab_Image_Screeshot.Source = null;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"))
            {
                GamesTab_Image_Boxfront.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"));
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"))
            {
                GamesTab_Image_Boxback.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"));
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"))
            {
                GamesTab_Image_Screeshot.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"));
            }
        }

        /// <summary>
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        private void SaveGameInfo()
        {

            //Invalid input checks
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                return;
            }

            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
            {
                if (GamesTab_Textbox_ReleaseDate.Text.Length < 5)
                {
                    CurrentGame.ReleaseDate = GamesTab_Textbox_ReleaseDate.Text;
                }
                else
                {
                    MessageBox.Show("Release Date Invalid");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Release Date score must be only digits");
                return;
            }
            if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
            {
                if (GamesTab_Textbox_ReleaseDate.Text.Length < 5)
                {
                    CurrentGame.CriticScore = GamesTab_Textbox_CriticScore.Text;
                }
                else
                {
                    MessageBox.Show("Critic Score Invalid");
                }
            }
            else
            {
                MessageBox.Show("Critic Score must be only digits");
            }

            if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
            {
                if (GamesTab_Textbox_ReleaseDate.Text.Length > 2)
                {
                    CurrentGame.Players = GamesTab_Textbox_Players.Text;
                }
                else
                {
                    MessageBox.Show("Players Invalid");
                }
            }
            else
            {
                MessageBox.Show("Players must be only digits");
            }

            if (GamesTab_Textbox_Developer.Text.Contains("|") || GamesTab_Textbox_Publisher.Text.Contains("|") || GamesTab_Textbox_ESRB.Text.Contains("|") || GamesTab_Textbox_Description.Text.Contains("|") || GamesTab_Textbox_ESRBDescriptor.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if (GamesTab_Textbox_ESRB.Text.Contains("Everyone") || GamesTab_Textbox_ESRB.Text.Contains("Teen") || GamesTab_Textbox_ESRB.Text.Contains("Mature") || GamesTab_Textbox_ESRB.Text.Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                {
                    CurrentGame.Esrb = GamesTab_Textbox_ESRB.Text;
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }

                if ((GamesTab_Textbox_Developer.Text.Length > 20) || (GamesTab_Textbox_Publisher.Text.Length > 20) || (GamesTab_Textbox_Description.Text.Length > 20) || (GamesTab_Textbox_ESRBDescriptor.Text.Length > 20))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    CurrentGame.Publisher = GamesTab_Textbox_Publisher.Text;
                    CurrentGame.Developer = GamesTab_Textbox_Developer.Text;
                    CurrentGame.Description = GamesTab_Textbox_Description.Text;
                    CurrentGame.EsrbDescriptor = GamesTab_Textbox_ESRBDescriptor.Text;
                }
            }

            //If all input fields are valid, save the database
            FileOps.SaveDatabase(Program.DatabasePath);
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void RefreshEsrbIcon(IGame g)
        {
            if (g == null) { return; }
            GamesTab_Image_ESRB.Source = null;
            if (g.Esrb.Equals("Everyone"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png"));
            }
            else if (g.Esrb.Equals("Everyone (KA)"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png"));
            }
            else if (g.Esrb.Equals("Everyone 10+"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png"));
            }
            else if (g.Esrb.Equals("Teen"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png"));
            }
            else if (g.Esrb.Equals("Mature"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png"));
            }

            if (g.Esrb.Equals("Adults Only (AO)"))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png"));
            }
        }

        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
            GlobalTab_Listbox_GlobalFavorites.Items.Clear();
            foreach (IConsole console in Program.ActiveDatabase.ConsoleList)
            {
                if (console.GameCount > 0)
                {
                    foreach (IGame game in console.GameList)
                    {
                        if (game.Favorite > 0)
                        {
                            GlobalTab_Listbox_GlobalFavorites.Items.Add(game.Title + " (" + game.ConsoleName + ")");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Placeholder method
        /// </summary>
        public void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }
}
