using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UniCade.Backend;
using UniCade.ConsoleInterface;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Network;
using Console = UniCade.Objects.Console;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {

        #region Private Instance Fields

        /// <summary>
        /// The game that is currently selected within the Games tab
        /// </summary>
        private static IGame _currentGame;

        /// <summary>
        /// The current console that is selected within the games tab
        /// </summary>
        private static IConsole _currentConsole;

        /// <summary>
        /// The current emulator that is selected within the Emulators tab
        /// </summary>
        private static IConsole _currentEmulator;

        /// <summary>
        /// True if the box front image within the games tab is currently expanded
        /// </summary>
        private bool _isBoxfrontExpanded;

        /// <summary>
        /// True if the box back image within the games tab is currently expanded
        /// </summary>
        private bool _isBoxBackExpanded;

        /// <summary>
        /// True if the screenshot image within the games tab is currently expanded
        /// </summary>
        private bool _isScreenshotExpanded;

        #endregion

        #region Constructors

        /// <summary>
        /// Public constructor for the SettingsWindowClass
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            Populate();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called on window close event
        /// </summary>
        void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow.IsSettingsWindowActive = false;
            MainWindow.KeyboardHook.HookKeys();
        }

        /// <summary>
        /// Populate settings window fields under all tabs
        /// </summary>
        private void Populate()
        {
            //Populate console list with the currently active games
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
                GamesTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
                EmulatorsTab_Listbox_ConsoleList.Items.Add(console.ConsoleName);
            }

            //Set initial selected indexes
            EmulatorsTab_Listbox_ConsoleList.SelectedIndex = 0;
            GamesTab_Listbox_ConsoleList.SelectedIndex = 0;

            //Poplate ESRB dropdown combo boxes
            foreach (Enums.Esrb esrb in Enum.GetValues(typeof(Enums.Esrb)))
            {
                GlobalTab_Dropdown_AllowedESRB.Items.Add(esrb.GetStringValue());
                UsersTab_Dropdown_AllowedESRB.Items.Add(esrb.GetStringValue());
            }

            //Load UniCade Logo images within the settings window
            try
            {
                AboutTab_Image_UniCadeLogo.Source =
                    new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
                CloudTab_Image_UniCadeLogo.Source =
                    new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
                EmulatorsTab_Image_UniCadeLogo.Source =
                    new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
                WebTab_Image_UniCadeLogo.Source =
                    new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            }
            catch (DirectoryNotFoundException)
            {
                //Do nothing
            }

            //Disable editing userinfo unless logged in
            UsersTab_Textbox_Username.IsEnabled = false;
            UsersTab_Textbox_Email.IsEnabled = false;
            UsersTab_Textbox_UserInfo.IsEnabled = false;

            //Set specific textboxes as readonly
            GlobalTab_Textbox_Coins.IsEnabled = false;
            GlobalTab_Textbox_Playtime.IsEnabled = false;

            //Populate features textbox under the About tab
            AboutTab_Textbox_SoftwareInfo.Text = TextFiles.features + "\n\n\n\n\n\n" + TextFiles.instructions;
            AboutTab_Textbox_SoftwareInfo.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            AboutTab_Textbox_SoftwareInfo.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;

            //Populate textbox fields
            GlobalTab_Textbox_Password.Password = Program.PasswordProtection.ToString();

            //Populate checkboxes
            WebTab_Checkbox_ReleaseDate.IsChecked = WebOps.ParseReleaseDate;
            WebTab_Checkbox_CriticScore.IsChecked = WebOps.ParseCriticScore;
            WebTab_Checkbox_Publisher.IsChecked = WebOps.ParsePublisher;
            WebTab_Checkbox_Developer.IsChecked = WebOps.ParseDeveloper;
            WebTab_Checkbox_ESRBRating.IsChecked = WebOps.ParseEsrbRating;
            WebTab_Checkbox_ESRBDescriptor.IsChecked = WebOps.ParseEsrbDescriptors;
            WebTab_Checkbox_Players.IsChecked = WebOps.ParsePlayerCount;
            WebTab_Checkbox_Description.IsChecked = WebOps.ParseDescription;
            WebTab_Checkbox_BoxFront.IsChecked = WebOps.ParseBoxFrontImage;
            WebTab_Checkbox_BoxBack.IsChecked = WebOps.ParseBoxBackImage;
            WebTab_Checkbox_Screenshot.IsChecked = WebOps.ParseScreenshot;
            WebTab_Checkbox_Metacritic.IsChecked = WebOps.ScanMetacritic;
            WebTab_Checkbox_Mobygames1.IsChecked = WebOps.ScanMobygames;
            GlobalTab_Checkbox_DisplaySplash.IsChecked = Program.ShowSplashScreen;
            GlobalTab_Checkbox_DisplayLoadingScreen.IsChecked = Program.ShowLoadingScreen;
            GlobalTab_Checkbox_RequireLogin.IsChecked = Program.RequireLogin;
            GlobalTab_Checkbox_RescanAllLibraries.IsChecked = Program.RescanOnStartup;
            GlobalTab_Checkbox_EnforceFileExtension.IsChecked = Program.EnforceFileExtensions;
            GlobalTab_Checkbox_DisplayESRB.IsChecked = MainWindow.DisplayEsrbWhileBrowsing;
            GlobalTab_Checkbox_EnablePayPerPlay.IsChecked = PayPerPlay.PayPerPlayEnabled;
            GlobalTab_Textbox_Coins.IsEnabled = PayPerPlay.PayPerPlayEnabled;
            GlobalTab_Textbox_Playtime.IsEnabled = PayPerPlay.PayPerPlayEnabled;
            GlobalTab_Dropdown_AllowedESRB.Text = Program.RestrictGlobalEsrb.GetStringValue();
            GamesTab_CheckBox__GlobalFavorite.IsChecked = MainWindow.DisplayEsrbWhileBrowsing;

            //Populate payPerPlay fields
            GlobalTab_Textbox_Coins.Text = PayPerPlay.CoinsRequired.ToString();
            GlobalTab_Textbox_Playtime.Text = PayPerPlay.Playtime.ToString();

            var userList = Database.GetUserList();
            foreach (string username in userList)
            {
                UsersTab_Listbox_CurrentUser.Items.Add(username);
            }

            //Refresh the global favorites list
            RefreshGlobalFavs();

            //Populate user license info
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + LicenseEngine.UserLicenseName;
            if (LicenseEngine.IsLicenseValid)
            {
                AboutTab_Label_Edition.Content = "License Status: Full Version";
            }
            else
            {
                AboutTab_Label_Edition.Content = "License Status: Invalid";
            }
            AboutTab_Label_LicenseKey.Content = "License Key: " + LicenseEngine.UserLicenseKey;
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
            if (SqlClient.SqlUsername == null)
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

            if (_currentGame == null)
            {
                MessageBox.Show("Must select a game");
                return;
            }
            IGame game = SqlClient.GetSingleGame(_currentGame.ConsoleName, _currentGame.Title);
            if (game != null)
            {
                //replace the game
                _currentConsole.RemoveGame(game.Title);
                _currentConsole.AddGame(game);
                RefreshGameInfo(game);
                MessageBox.Show("Download successful");
                return;
            }
            MessageBox.Show("Download failed");
        }

        /// <summary>
        /// Upload console button
        /// Upload all games from the selected console to UniCade Cloud
        /// </summary>
        private void GamesTab_UploadConsoleButton_Click(object sender, EventArgs e)
        {
            //Invalid input checks
            if (SqlClient.SqlUsername == null)
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
            var gameList = _currentConsole.GetGameList();
            foreach (string gameTitle in gameList)
            {
                IGame game = _currentConsole.GetGame(gameTitle);
                SqlClient.UploadGame(game);
            }
            MessageBox.Show("Console Uploaded");
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
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to delete");
                return;
            }
            if (_currentConsole == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            var gameList = _currentConsole.GetGameList();
            foreach (string gameTitle in gameList)
            {
                IGame g = _currentConsole.GetGame(gameTitle);
                IGame game = SqlClient.GetSingleGame(g.ConsoleName, g.Title);
                if (game != null)
                {
                    _currentConsole.RemoveGame(game.Title);
                    _currentConsole.AddGame(game);
                }
            }

            //Refresh the current game info
            MessageBox.Show("Download successful");
            RefreshGameInfo(_currentGame);
        }

        /// <summary>
        /// Called when the select index is changed. Update the proper game info in the details fields. 
        /// </summary>
        private void GamesTab_GamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_GamesList.SelectedItem == null) { return; }
            string currentGame = GamesTab_Listbox_GamesList.SelectedItem.ToString();
            var gameList = _currentConsole.GetGameList();
            foreach (string gameTitle in gameList)
            {
                IGame game = _currentConsole.GetGame(gameTitle);
                if (game.Title.Equals(currentGame))
                {
                    _currentGame = game;
                }
            }
            RefreshGameInfo(_currentGame);
            RefreshEsrbIcon(_currentGame);
        }

        /// <summary>
        /// Called when the select index is changed for the console listbox. Update the games list for the selected console. 
        /// </summary>
        private void GamesTab_ConsoleListBox__SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null) { return; }
            string curItem = GamesTab_Listbox_ConsoleList.SelectedItem.ToString();
            GamesTab_Listbox_GamesList.Items.Clear();
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
                if (console.ConsoleName.Equals(curItem))
                {
                    _currentConsole = console;
                    GamesTab_Textbox_GamesForConsole.Text = console.GetGameCount().ToString();
                    GamesTab_Textbox_TotalGames.Text = Database.GetTotalGameCount().ToString();

                    //Populate the games list
                    console.GetGameList().ForEach(g => GamesTab_Listbox_GamesList.Items.Add(g));
                }
            }
            if (GamesTab_Listbox_GamesList.Items.Count > 0)
            {
                GamesTab_Listbox_GamesList.SelectedIndex = 0;
                var gameList = _currentConsole.GetGameList();
                foreach (string gameTitle in gameList)
                {
                    IGame g = _currentConsole.GetGame(gameTitle);
                    if (g.Title.Equals(GamesTab_Listbox_GamesList.SelectedItem.ToString()))
                    {
                        _currentGame = g;
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
        internal void GamesTab_RescrapeGameButton_Click(object sender, EventArgs e)
        {
            //Require that a user select a valid game to rescrape
            if (GamesTab_Listbox_GamesList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }

            //Scrape info and populate local fields
            WebOps.ScrapeInfo(_currentGame);
            GamesTab_Textbox_Title.Text = _currentGame.Title;
            GamesTab_Textbox_Console.Text = _currentGame.ConsoleName;
            GamesTab_Textbox_ReleaseDate.Text = _currentGame.ReleaseDate;
            GamesTab_Textbox_CriticScore.Text = _currentGame.CriticReviewScore;
            GamesTab_Textbox_Publisher.Text = _currentGame.PublisherName;
            GamesTab_Textbox_Developer.Text = _currentGame.DeveloperName;
            GamesTab_Textbox_ESRB.Text = _currentGame.EsrbRating.GetStringValue();
            GamesTab_Textbox_Players.Text = _currentGame.SupportedPlayerCount;
            GamesTab_Textbox_ESRBDescriptor.Text = _currentGame.EsrbDescriptors;
            GamesTab_Textbox_Description.Text = _currentGame.Description;
            RefreshEsrbIcon(_currentGame);
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

            if (SqlClient.SqlUsername == null)
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
            SqlClient.UploadGame(_currentGame);
            MessageBox.Show("Game Uploaded");
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
            var gameList = _currentConsole.GetGameList();
            foreach (string gameTitle in gameList)
            {
                IGame game1 = _currentConsole.GetGame(gameTitle);
                WebOps.ScrapeInfo(game1);
            }
            MessageBox.Show("Operation Successful");
        }

        /// <summary>
        /// Toggle expansion of the boxfront image
        /// </summary>
        private void Image_Boxfront_Expand(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isBoxBackExpanded && !_isScreenshotExpanded)
            {
                if (!_isBoxfrontExpanded)
                {
                    GamesTab_Image_Boxfront.Margin = new Thickness(0, 0, 0, 0);
                    GamesTab_Image_Boxfront.Height = 500;
                    GamesTab_Image_Boxfront.Width = 500;
                    _isBoxfrontExpanded = true;
                }
                else
                {
                    GamesTab_Image_Boxfront.Margin = new Thickness(550, 57, 0, 0);
                    GamesTab_Image_Boxfront.Height = 109;
                    GamesTab_Image_Boxfront.Width = 92;
                    _isBoxfrontExpanded = false;
                }
            }
        }

        /// <summary>
        /// Toggle expansion of the boxfront image
        /// </summary>
        private void Image_Boxback_Expand(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isBoxfrontExpanded && !_isScreenshotExpanded)
            {
                if (!_isBoxBackExpanded)
                {
                    GamesTab_Image_Boxback.Margin = new Thickness(0, 0, 0, 0);
                    GamesTab_Image_Boxback.Height = 500;
                    GamesTab_Image_Boxback.Width = 500;
                    _isBoxBackExpanded = true;
                }
                else
                {
                    GamesTab_Image_Boxback.Margin = new Thickness(647, 57, 0, 0);
                    GamesTab_Image_Boxback.Height = 107;
                    GamesTab_Image_Boxback.Width = 97;
                    _isBoxBackExpanded = false;
                }
            }
        }

        /// <summary>
        /// Toggle expansion of the screenshot image
        /// </summary>
        private void Image_Screenshot_Expand(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isBoxfrontExpanded && !_isBoxBackExpanded)
            {
                if (!_isScreenshotExpanded)
                {
                    GamesTab_Image_Screeshot.Margin = new Thickness(0, 0, 0, 0);
                    GamesTab_Image_Screeshot.Height = 500;
                    GamesTab_Image_Screeshot.Width = 500;
                    _isScreenshotExpanded = true;
                }
                else
                {
                    GamesTab_Image_Screeshot.Margin = new Thickness(572, 196, 0, 0);
                    GamesTab_Image_Screeshot.Height = 103;
                    GamesTab_Image_Screeshot.Width = 172;
                    _isScreenshotExpanded = false;
                }
            }
        }

        /// <summary>
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        internal void SaveGameInfo()
        {
            try
            {
                _currentGame.ReleaseDate = GamesTab_Textbox_ReleaseDate.Text;
                _currentGame.CriticReviewScore = GamesTab_Textbox_CriticScore.Text;
                _currentGame.SupportedPlayerCount = GamesTab_Textbox_Players.Text;
                _currentGame.EsrbRating = Enums.ConvertStringToEsrbEnum(GamesTab_Textbox_ESRB.Text);
                _currentGame.PublisherName = GamesTab_Textbox_Publisher.Text;
                _currentGame.DeveloperName = GamesTab_Textbox_Developer.Text;
                _currentGame.Description = GamesTab_Textbox_Description.Text;
                _currentGame.EsrbDescriptors = GamesTab_Textbox_ESRBDescriptor.Text;
                _currentGame.Favorite = GamesTab_CheckBox__GlobalFavorite.IsChecked.Value;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Error: " + e.Message);

            }

            //If all input fields are valid, save the database
            FileOps.SaveDatabase(Program.DatabasePath);
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void RefreshEsrbIcon(IGame game)
        {
            if (game == null) { return; }
            GamesTab_Image_ESRB.Source = null;
            if (game.EsrbRating.Equals(Enums.Esrb.Everyone))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png"));
            }
            else if (game.EsrbRating.Equals(Enums.Esrb.Everyone10))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png"));
            }
            else if (game.EsrbRating.Equals(Enums.Esrb.Teen))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png"));
            }
            else if (game.EsrbRating.Equals(Enums.Esrb.Mature))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png"));
            }

            if (game.EsrbRating.Equals(Enums.Esrb.Ao))
            {
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (Ao).png"));
            }
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
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
                if (console.ConsoleName.Equals(curItem))
                {
                    _currentEmulator = console;
                    EmulatorsTab_Textbox_ConsoleName1.Text = console.ConsoleName;
                    EmulatorsTab_Textbox_ROMExtension.Text = console.RomExtension;
                    EmulatorsTab_Textbox_EmulatorArgs.Text = console.LaunchParams;
                    EmulatorsTab_Textbox_EmulatorExe.Text = console.EmulatorPath;
                    EmulatorsTab_Textbox_ConsoleInfo.Text = console.ConsoleInfo;
                    EmulatorsTab_Textbox_GameCount.Text = console.GetGameCount().ToString();
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
            try
            {
                _currentEmulator.ConsoleName = EmulatorsTab_Textbox_ConsoleName1.Text;
                _currentEmulator.RomExtension = EmulatorsTab_Textbox_ROMExtension.Text;
                _currentEmulator.EmulatorPath = EmulatorsTab_Textbox_EmulatorExe.Text;
                _currentEmulator.LaunchParams = EmulatorsTab_Textbox_EmulatorArgs.Text;
                _currentEmulator.ReleaseDate = EmulatorsTab_Textbox_ReleaseDate.Text;
                _currentEmulator.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            FileOps.SaveDatabase(Program.DatabasePath);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void EmulatorsTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow.IsSettingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Delete console button
        /// Deletes a consle and all associated games from the database
        /// </summary>
        private void EmulatorsTab_DeleteConsoleButton_Click(object sender, EventArgs e)
        {
            //Ensure that at least one console exists
            if (Database.GetConsoleCount() <= 1)
            {
                MessageBox.Show("Cannot have an empty console list");
                return;
            }
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            Database.RemoveConsole(_currentEmulator.ConsoleName);
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
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
            EmulatorsTab_Textbox_ROMExtension.Text = null;
            EmulatorsTab_Textbox_EmulatorArgs.Text = null;
            EmulatorsTab_Textbox_ConsoleInfo.Text = null;
            EmulatorsTab_Textbox_GameCount.Text = null;
            EmulatorsTab_Textbox_ReleaseDate.Text = null;

            //Create a new console and add it to the datbase
            string newConsoleName = "New Console";
            IConsole newConsole = new Console(newConsoleName);

            Database.AddConsole(newConsole);
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole con = Database.GetConsole(consoleName);
                EmulatorsTab_Listbox_ConsoleList.Items.Add(con.ConsoleName);
                GamesTab_Listbox_ConsoleList.Items.Add(con.ConsoleName);
            }
            EmulatorsTab_Listbox_ConsoleList.SelectedIndex = (EmulatorsTab_Listbox_ConsoleList.Items.Count - 1);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// //Force metadata rescrape (All games within console)
        /// </summary>
        private void EmulatorsTab_ForceGlobalMetadataRescrapeButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Save the custom info fields for the current emulator
        /// </summary>
        private void EmulatorsTab_SaveInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                _currentEmulator.ConsoleName = EmulatorsTab_Textbox_ConsoleName1.Text;
                _currentEmulator.RomExtension = EmulatorsTab_Textbox_ROMExtension.Text;
                _currentEmulator.LaunchParams = EmulatorsTab_Textbox_EmulatorArgs.Text;
                _currentEmulator.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
            MainWindow.RefreshConsoleList();
            MessageBox.Show("Saved");
        }

        /// <summary>
        /// Rescan all games across all emulators
        /// </summary>
        private void EmulatorsTab_GlobalRescanButton_Click(object sender, EventArgs e)
        {
            FileOps.ScanAllConsoles();
            MessageBox.Show("Global Rescan Successful");
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

            //Fetch the currently selected console
            IConsole console = Database.GetConsole(EmulatorsTab_Listbox_ConsoleList.SelectedItem.ToString());

            if (FileOps.ScanSingleConsole(_currentEmulator))
            {
                MessageBox.Show(console.ConsoleName + " Successfully Scanned");
                return;
            }
            MessageBox.Show(console.ConsoleName + " Library Rescan Failed");
        }

        #endregion

        #region Users Tab

        /// <summary>
        /// Close and save button
        /// </summary>
        private void UsersTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow.IsSettingsWindowActive = false;
            FileOps.SavePreferences(Program.PreferencesPath);
            this.Close();
        }

        /// <summary>
        /// Refresh user info under the User tab every time a new user is selected
        /// </summary>
        private void UsersTab_UsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fetch the current user
            IUser user = Database.GetUser(UsersTab_Listbox_CurrentUser.SelectedItem.ToString());

            //Populate the favorites list for each user
            UsersTab_Listbox_UserFavorites.Items.Clear();
            user.FavoritesList.ForEach(g => UsersTab_Listbox_UserFavorites.Items.Add(g.Title + " - " + g.ConsoleName));

            //Populate the user fields
            UsersTab_Textbox_Username.Text = user.Username;
            UsersTab_Textbox_Email.Text = user.Email;
            UsersTab_Textbox_UserInfo.Text = user.UserInfo;
            UsersTab_Textbox_LoginCount.Text = user.GetUserLoginCount().ToString();
            UsersTab_Textbox_LaunchCount.Text = user.GetUserLaunchCount().ToString();
            UsersTab_Dropdown_AllowedESRB.Text = user.AllowedEsrb.GetStringValue();

            //Only allow the current user to edit their own userdata
            if (user.Username.Equals(Database.GetCurrentUser().Username))
            {
                UsersTab_Textbox_Username.IsEnabled = true;
                UsersTab_Textbox_Email.IsEnabled = true;
                UsersTab_Textbox_UserInfo.IsEnabled = true;
                UsersTab_Dropdown_AllowedESRB.IsEnabled = true;
                UsersTab_Listbox_UserFavorites.IsEnabled = true;
            }
        }

        /// <summary>
        /// Create new user button
        /// Create a new user and save the userdata to the preferences file
        /// </summary>
        private void UsersTab_NewUserButton_Click(object sender, EventArgs e)
        {
            //Create a new unicade account and display the dialog
            AccountWindow uc = new AccountWindow(1);
            uc.ShowDialog();

            //Save the user info to the preferences file
            FileOps.SavePreferences(Program.PreferencesPath);

            //Refresh the listbox contents
            UsersTab_Listbox_CurrentUser.Items.Clear();
            Database.GetUserList().ForEach(u => UsersTab_Listbox_CurrentUser.Items.Add(u));
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
            IUser user = Database.GetUser(UsersTab_Listbox_CurrentUser.SelectedItem.ToString());

            try
            {
                //Remove the user and refresh the database
                Database.RemoveUser(user.Username);
            }
            catch(ArgumentException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }

            //Refresh the user list
            UsersTab_Listbox_CurrentUser.Items.Clear();
            Database.GetUserList().ForEach(u => UsersTab_Listbox_CurrentUser.Items.Add(u));
        }

        /// <summary>
        /// Save button (Global Settings tab)
        /// Save the current global settings to the preferences file
        /// </summary>
        private void UsersTab_SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Database.GetCurrentUser().Username = UsersTab_Textbox_Username.Text;
                Database.GetCurrentUser().SetUserPassword(UsersTab_Textbox_Email.Text);
                Database.GetCurrentUser().UserInfo = UsersTab_Textbox_UserInfo.Text;
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }


            if (UsersTab_Dropdown_AllowedESRB.SelectedItem == null)
            {
                Database.GetCurrentUser().AllowedEsrb = Enums.Esrb.Null;
            }
            else
            {
                Database.GetCurrentUser().AllowedEsrb =
                    Enums.ConvertStringToEsrbEnum(UsersTab_Dropdown_AllowedESRB.SelectedItem.ToString());
            }

            

            //Refresh the user list
            UsersTab_Listbox_CurrentUser.Items.Clear();
            Database.GetUserList().ForEach(u => UsersTab_Listbox_CurrentUser.Items.Add(u));
        }

        /// <summary>
        /// Delete user favorite
        /// </summary>
        private void UsersTab_DeleteFavoriteButton_Click(object sender, EventArgs e)
        {
            //Remove the favorite game at the current index
            Database.GetCurrentUser().FavoritesList.RemoveAt(UsersTab_Listbox_UserFavorites.SelectedIndex);

            //Refresh the user favorites list
            UsersTab_Listbox_UserFavorites.Items.Clear();
             Database.GetCurrentUser().FavoritesList.ForEach(g => UsersTab_Listbox_UserFavorites.Items.Add(g.Title + " - " + g.ConsoleName));
        }

        /// <summary>
        /// Login local user button
        /// </summary>
        private void UsersTab_LoginButton_Click(object sender, EventArgs e)
        {
            //Display the login dialog
            LoginWindow login = new LoginWindow(1);
            login.ShowDialog();

            //If the user is logged in sucuesfully, save the current user and preferences file
            UsersTab_Label_CurrentUser.Content = "Current User: " + Database.GetCurrentUser().Username;
        }

        /// <summary>
        /// Refresh the current users list and userdata
        /// </summary>
        private void UsersTab_RefreshButton_Click(object sender, EventArgs e)
        {
            UsersTab_Label_CurrentUser.Content = "Current User: " + Database.GetCurrentUser().Username;
        }

        #endregion

        #region Global Settings Tab

        /// <summary>
        /// Modify the global ESRB rating when the dropdown is changed
        /// </summary>
        private void GlobalSettingsTab_AllowedEsrbRatingDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.RestrictGlobalEsrb = Enums.ConvertStringToEsrbEnum(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
        }

        /// <summary>
        /// Save Global Settings button
        /// </summary>
        private void GlobalSettings_SavePreferenceFileButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (GlobalTab_Dropdown_AllowedESRB.SelectedItem == null)
                {
                    Program.RestrictGlobalEsrb = Enums.Esrb.Null;
                }
                else
                {
                    Program.RestrictGlobalEsrb =
                        Enums.ConvertStringToEsrbEnum(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
                }
                Program.EnforceFileExtensions = GlobalTab_Checkbox_EnforceFileExtension.IsChecked.Value;
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }

            //Save checkboxes
            MainWindow.DisplayEsrbWhileBrowsing = GlobalTab_Checkbox_ToView.IsChecked.Value;
            Program.ShowSplashScreen = GlobalTab_Checkbox_DisplaySplash.IsChecked.Value;
            Program.RequireLogin = GlobalTab_Checkbox_RequireLogin.IsChecked.Value;
            Program.RescanOnStartup = GlobalTab_Checkbox_RescanAllLibraries.IsChecked.Value;
            MainWindow.DisplayEsrbWhileBrowsing = GlobalTab_Checkbox_DisplayESRB.IsChecked.Value;

            int.TryParse(GlobalTab_Textbox_Password.Password, out int n);
            Program.PasswordProtection = int.Parse(GlobalTab_Textbox_Password.Password);

            int.TryParse(GlobalTab_Textbox_Coins.Text, out n);
            PayPerPlay.CoinsRequired = int.Parse(GlobalTab_Textbox_Coins.Text);

            //Save all active preferences to the local preferences file
            FileOps.SavePreferences(Program.PreferencesPath);
        }

        /// <summary>
        /// Toggle payPerPlay checkbox
        /// </summary>
        private void GlobalSettingsTab_TogglePayPerPlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PayPerPlay.PayPerPlayEnabled = false;
            GlobalTab_Textbox_Coins.IsEnabled = false;
            GlobalTab_Textbox_Playtime.IsEnabled = false;
            PayPerPlay.PayPerPlayEnabled = GlobalTab_Checkbox_EnablePayPerPlay.IsChecked.Value;
            GlobalTab_Textbox_Coins.IsEnabled = GlobalTab_Checkbox_EnablePayPerPlay.IsChecked.Value;
            GlobalTab_Textbox_Playtime.IsEnabled = GlobalTab_Checkbox_EnablePayPerPlay.IsChecked.Value;

        }

        /// <summary>
        /// Close button
        /// </summary>
        private void GlobalSettingsTab_CloseButton_Click(object sender, EventArgs e)
        {
            MainWindow.IsSettingsWindowActive = false;
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
        /// Close button
        /// </summary>
        private void WebTab_Button_Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.IsSettingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Save all current webscraper settings to the database
        /// </summary>
        private void WebTab_Button_SaveScraperSettings_Click(object sender, RoutedEventArgs e)
        {
            WebOps.ScanMobygames = WebTab_Checkbox_Mobygames1.IsChecked.Value;
            WebOps.ScanMetacritic = WebTab_Checkbox_Metacritic.IsChecked.Value;
            WebOps.ParseReleaseDate = WebTab_Checkbox_ReleaseDate.IsChecked.Value;
            WebOps.ParseCriticScore = WebTab_Checkbox_CriticScore.IsChecked.Value;
            WebOps.ParsePublisher = WebTab_Checkbox_Publisher.IsChecked.Value;
            WebOps.ParseDeveloper = WebTab_Checkbox_Developer.IsChecked.Value;
            WebOps.ParseEsrbRating = WebTab_Checkbox_ESRBRating.IsChecked.Value;
            WebOps.ParseDescription = WebTab_Checkbox_ESRBDescriptor.IsChecked.Value;
            WebOps.ParsePlayerCount = WebTab_Checkbox_Players.IsChecked.Value;
            WebOps.ParseDescription = WebTab_Checkbox_ESRBDescriptor.IsChecked.Value;
            WebOps.ParseBoxFrontImage = WebTab_Checkbox_BoxFront.IsChecked.Value;
            WebOps.ParseBoxBackImage = WebTab_Checkbox_BoxBack.IsChecked.Value;
            WebOps.ParseScreenshot = WebTab_Checkbox_Screenshot.IsChecked.Value;
            WebOps.ParseReleaseDate = WebTab_Checkbox_ReleaseDate.IsChecked.Value;
            WebOps.ParseCriticScore = WebTab_Checkbox_CriticScore.IsChecked.Value;
            WebOps.ParsePublisher = WebTab_Checkbox_Publisher.IsChecked.Value;
            WebOps.ParseDeveloper = WebTab_Checkbox_Developer.IsChecked.Value;
            WebOps.ParseEsrbRating = WebTab_Checkbox_ESRBRating.IsChecked.Value;
            WebOps.ParseDescription = WebTab_Checkbox_ESRBDescriptor.IsChecked.Value;
            WebOps.ParsePlayerCount = WebTab_Checkbox_Players.IsChecked.Value;
            WebOps.ParseDescription = WebTab_Checkbox_ESRBDescriptor.IsChecked.Value;
            WebOps.ParseBoxFrontImage = WebTab_Checkbox_BoxFront.IsChecked.Value;
            WebOps.ParseBoxBackImage = WebTab_Checkbox_BoxBack.IsChecked.Value;
            WebOps.ParseScreenshot = WebTab_Checkbox_Screenshot.IsChecked.Value;
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
            if (SqlClient.SqlUsername != null)
            {
                WebTab_Label_CurrentWebUser.Content = "Current Web User: " + SqlClient.SqlUsername;
            }
        }

        /// <summary>
        /// Logout button (UniCade Cloud tab)
        /// Logs out the current SQL user 
        /// </summary>
        private void CloudTab_Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            //Check if a user is actually logged in
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("User is already logged out");
                WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
                return;
            }

            //Log the current user out and update the interface
            SqlClient.SqlUsername = null;
            WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
        }

        /// <summary>
        /// Delete user button
        /// Delete the SQL user and update the interface
        /// </summary>
        private void CloudTab_Button_DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            //Delete the current SQL user and update the label
            SqlClient.DeleteUser();
            WebTab_Label_CurrentWebUser.Content = "Current Web User: ";
        }

        /// <summary>
        /// Upload all games button
        /// Upload all games across all consoles to UniCade Cloud
        /// </summary>
        private void CloudTab_Button_UploadAllGames_Click(object sender, RoutedEventArgs e)
        {
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SqlClient.UploadAllGames();
            MessageBox.Show("Library successfully uploaded");
        }

        /// <summary>
        /// Delete all games from the current user's UniCade Cloud account
        /// </summary>
        private void CloudTab_Button_DeleteAllGamesInCloud_Click(object sender, RoutedEventArgs e)
        {
            //Check if a SQL user is currently logged in
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SqlClient.Deletegames();
            MessageBox.Show("Library successfully deleted");
        }

        /// <summary>
        /// Download all games button
        /// Download all game metadata across all consoles
        /// </summary>
        private void CloudTab_Button_DownloadAllGames_Click(object sender, RoutedEventArgs e)
        {
            if (SqlClient.SqlUsername == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SqlClient.DownloadAllGames();
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
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + LicenseEngine.UserLicenseName;
            AboutTab_Label_LicenseKey.Content = "License Key: " + LicenseEngine.UserLicenseKey;

            //Set the license text depending on if the key is valid
            if (LicenseEngine.IsLicenseValid)
            {
                AboutTab_Label_Edition.Content = "License Status: Full Version";
            }
            else
            {
                AboutTab_Label_Edition.Content = "License Status: Invalid";
            }
        }

        private void LaunchCmdInterface_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            UniCadeCmd.PrepAndRun();
        }

        #endregion

        #region Helper Methods

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
            GamesTab_Textbox_CriticScore.Text = game.CriticReviewScore;
            GamesTab_Textbox_Publisher.Text = game.PublisherName;
            GamesTab_Textbox_Developer.Text = game.DeveloperName;
            GamesTab_Textbox_ESRB.Text = game.EsrbRating.GetStringValue();
            GamesTab_Textbox_Players.Text = game.SupportedPlayerCount;
            GamesTab_Textbox_ESRBDescriptor.Text = game.EsrbDescriptors;
            GamesTab_Textbox_Description.Text = game.Description;
            GamesTab_Textbox_LaunchCount.Text = game.GetLaunchCount().ToString();
            GamesTab_CheckBox__GlobalFavorite.IsChecked = game.Favorite;

            GamesTab_Image_Boxfront.Source = null;
            GamesTab_Image_Boxback.Source = null;
            GamesTab_Image_Screeshot.Source = null;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"))
            {
                GamesTab_Image_Boxfront.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"));
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"))
            {
                GamesTab_Image_Boxback.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"));
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"))
            {
                GamesTab_Image_Screeshot.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _currentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"));
            }
        }
        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
            GlobalTab_Listbox_GlobalFavorites.Items.Clear();
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
                if (console.GetGameCount() > 0)
                {
                    var gameList = console.GetGameList();
                    foreach (string gameTitle in gameList)
                    {
                        IGame game = console.GetGame(gameTitle);
                        if (game != null)
                        {
                            if (game.Favorite)
                            {
                                GlobalTab_Listbox_GlobalFavorites.Items.Add(game.Title + " (" + game.ConsoleName + ")");
                            }
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
