using System;
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

        public static Game _curGame;
        public static User _curUser;
        static Console _curConsole2;
        static Console _curConsole;
        public static string _defaultUser;
        public static int _showSplash;
        public static int _scanOnStartup;
        public static int _restrictESRB;
        public static int _requireLogin;
        public static int _cmdOrGui;
        public static int _showLoading;
        public static int _payPerPlay;
        public static int _coins;
        public static int _playtime;
        public static int _perLaunch;
        public static int _viewEsrb;
        public static int _passProtect;
        public static int _enforceExt;

        #endregion

        #region Class Methods

        public SettingsWindow()
        {
            InitializeComponent();
            //FormClosing += SettingsWindow_FormClosing;
            Populate();
        }

        /// <summary>
        /// Populate settings window fields under all tabs
        /// </summary>
        private void Populate()
        {
            //Populate console list with the currently active games
            foreach (Console c in Database.ConsoleList)
            {
                GamesTab_Listbox_ConsoleList.Items.Add(c.Name);
                EmulatorsTab_Listbox_ConsoleList.Items.Add(c.Name);
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
            //GamesTab_Image_ESRB.siz = PictureBoxSizeMode.StretchImage;
            //pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            AboutTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            //pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            CloudTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            //pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            EmulatorsTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));
            //pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            WebTab_Image_UniCadeLogo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png"));

            //Populate the 'Allowed ESRB' combo box with the specified rating
            if (_restrictESRB == 0)
                GlobalTab_Dropdown_AllowedESRB.Text = "None";
            else if (_restrictESRB == 1)
                GlobalTab_Dropdown_AllowedESRB.Text = "Everyone";
            else if (_restrictESRB == 2)
                GlobalTab_Dropdown_AllowedESRB.Text = "Everyone 10+";
            else if (_restrictESRB == 3)
                GlobalTab_Dropdown_AllowedESRB.Text = "Teen";
            else if (_restrictESRB == 4)
                GlobalTab_Dropdown_AllowedESRB.Text = "Mature";
            else if (_restrictESRB == 5)
                GlobalTab_Dropdown_AllowedESRB.Text = "Adults Only (AO)";
            if (_viewEsrb > 0)
                GamesTab_CheckBox__GlobalFavorite.IsChecked = true;

            //Disable editing userinfo unless logged in
            UsersTab_Textbox_Username.IsEnabled = false;
            UsersTab_Textbox_Email.IsEnabled = false;
            UsersTab_Textbox_UserInfo.IsEnabled = false;

            //Set specific textboxes as readonly
            UsersTab_Textbox_LoginCount.IsEnabled = false;
            UsersTab_Textbox_LaunchCount.IsEnabled = false;
            EmulatorsTab_Textbox_GameCount.IsEnabled = false;
            GamesTab_Textbox_LaunchCount.IsEnabled = false;
            GamesTab_Textbox_TotalGames.IsEnabled = false;
            GamesTab_Textbox_GamesForConsole.IsEnabled = false;

            //Set additional textboxes to readonly
            GlobalTab_Textbox_EmulatorDirectory.IsEnabled = false;
            GlobalTab_Textbox_DatabasePath.IsEnabled = false;
            UsersTab_Listbox_UserFavorites.IsEnabled = false;

            //Populate features textbox under the About tab
            //AboutTab_Textbox_Info. = TextFiles.features + "\n\n\n\n\n\n" + TextFiles.instructions;

            //Populate textbox fields
            GlobalTab_Textbox_Password.Text = _passProtect.ToString();
            GlobalTab_Textbox_DatabasePath.Text = Program._databasePath;
            GlobalTab_Textbox_EmulatorDirectory.Text = Program._emuPath;
            GlobalTab_Textbox_MedaDirectory.Text = Program._mediaPath;
            GlobalTab_Textbox_ROMDirectory.Text = Program._romPath;

            //Check specified boxes under the Web tab
            if (WebOps.releaseDate > 0)
                WebTab_Checkbox_ReleaseDate.IsChecked = true;
            if (WebOps.critic > 0)
                WebTab_Checkbox_CriticScore.IsChecked = true;
            if (WebOps.publisher > 0)
                WebTab_Checkbox_Publisher.IsChecked = true;
            if (WebOps.developer > 0)
                WebTab_Checkbox_Developer.IsChecked = true;
            if (WebOps.esrb > 0)
                WebTab_Checkbox_ESRBRating.IsChecked = true;
            if (WebOps.esrbDescriptor > 0)
                WebTab_Checkbox_ESRBDescriptor.IsChecked = true;
            if (WebOps.players > 0)
                WebTab_Checkbox_Players.IsChecked = true;
            if (WebOps.description > 0)
                WebTab_Checkbox_Description.IsChecked = true;
            if (WebOps.boxFront > 0)
                WebTab_Checkbox_BoxFront.IsChecked = true;
            if (WebOps.boxBack > 0)
                WebTab_Checkbox_BoxBack.IsChecked = true;
            if (WebOps.screenshot > 0)
                WebTab_Checkbox_Screenshot.IsChecked = true;
            if (WebOps.metac > 0)
                WebTab_Checkbox_Metacritic.IsChecked = true;
            if (WebOps.mobyg > 0)
                WebTab_Checkbox_Mobygames1.IsChecked = true;

            //Populate Global Settings checkboxes
            if (_showSplash > 0)
                GlobalTab_Checkbox_DisplaySplash.IsChecked = true;
            if (_showLoading > 0)
                GlobalTab_Checkbox_DisplayLoadingScreen.IsChecked = true;
            if (_requireLogin > 0)
                GlobalTab_Checkbox_RequireLogin.IsChecked = true;
            if (_scanOnStartup > 0)
                GlobalTab_Checkbox_RescanAllLibraries.IsChecked = true;
            if (_enforceExt > 0)
                EmulatorsTab_Checkbox_EnforceFileExtension.IsChecked = true;
            if (_viewEsrb == 1)
                GlobalTab_Checkbox_DisplayESRB.IsChecked = true;
            if (_payPerPlay > 0)
                GlobalTab_Checkbox_EnablePayPerPlay.IsChecked = true;

            //Populate payPerPlay fields
            GlobalTab_Textbox_Coins.Text = _coins.ToString();
            GlobalTab_Textbox_Playtime.Text = _playtime.ToString();

            foreach (User u in Database.UserList)
                UsersTab_Listbox_CurrentUser.Items.Add(u.Username);

            //Refresh the global favorites list
            RefreshGlobalFavs();

            //Populate user license info
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + Program._userLicenseName;
            AboutTab_Label_Edition.Content = "License Status: Full Version";
            AboutTab_Label_LicenseKey.Content = "License Key: " + Program._userLicenseKey;
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

            if (_curGame == null)
            {
                MessageBox.Show("Must select a game");
                return;
            }
            Game game = null;
            game = SQLclient.GetSingleGame(_curGame.Console, _curGame.Title);
            if (game != null)
            {
                for (int i = 0; i < _curConsole2.GameList.Count; i++)
                {
                    Game g = (Game)_curConsole2.GameList[i];
                    if (game.FileName.Equals(g.FileName))
                    {
                        _curConsole2.GameList[i] = game;
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
                MessageBox.Show("Login Required");
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
            foreach (Game g in _curConsole2.GameList)
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
                MessageBox.Show("Login Required");
                return;
            }
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
            {
                MessageBox.Show("No games to delete");
                return;
            }
            if (_curConsole2 == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            for (int i = 0; i < _curConsole2.GameList.Count; i++)
            {
                Game game1 = (Game)_curConsole2.GameList[i];
                Game game2 = null;
                game2 = SQLclient.GetSingleGame(game1.Console, game1.Title);
                if (game2 != null)
                {
                    if (game2.FileName.Length > 3)
                        _curConsole2.GameList[i] = game2;
                }
            }

            //Refresh the current game info
            MessageBox.Show("Download successful");
            RefreshGameInfo(_curGame);
        }

        /// <summary>
        /// Called when the select index is changed. Update the proper game info in the details fields. 
        /// </summary>
        private void GamesTab_GamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = GamesTab_Listbox_ConsoleList.SelectedItem.ToString();
            GamesTab_Listbox_GamesList.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(curItem))
                {
                    _curConsole2 = c;
                    GamesTab_Textbox_GamesForConsole.Text = c.GameCount.ToString();
                    GamesTab_Textbox_TotalGames.Text = Database.TotalGameCount.ToString();
                    if (c.GameCount > 0)
                    {
                        foreach (Game g in c.GameList)
                        {
                            GamesTab_Listbox_GamesList.Items.Add(g.Title);
                        }
                    }
                }
            }
            if (GamesTab_Listbox_GamesList.Items.Count > 0)
            {
                GamesTab_Listbox_GamesList.SelectedIndex = 0;
                foreach (Game g in _curConsole2.GameList)
                {
                    if (g.Title.Equals(GamesTab_Listbox_GamesList.SelectedItem.ToString()))
                    {
                        _curGame = g;
                    }
                }
            }
            else
            {
                RefreshGameInfo(null);
            }
        }

        /// <summary>
        /// Called when the select index is changed for the console listbox. Update the games list for the selected console. 
        /// </summary>
        private void GamesTab_ConsoleListBox__SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentConsole = GamesTab_Listbox_ConsoleList.SelectedItem.ToString();
            foreach (Game g in _curConsole2.GameList)
            {
                if (g.Title.Equals(currentConsole))
                {
                    _curGame = g;
                }
            }
            RefreshGameInfo(_curGame);
            RefreshEsrbIcon(_curGame);
        }

        private void GamesTab_RescrapeGameButton_Click(object sender, RoutedEventArgs e)
        {
        }


            /// <summary>
            /// Rescrape game info button.
            /// Rescrapes info the the specified game from the web
            /// </summary>
            private void GamesTab_RescrapeGameButton_Click(object sender, EventArgs e)
            {
                //Require that a user select a valid game to rescrape
                if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
                {
                    MessageBox.Show("Must select a console/game");
                    return;
                }

                //Scrape info and populate local fields
                WebOps.ScrapeInfo(_curGame);
                GamesTab_Textbox_Title.Text = _curGame.Title;
                GamesTab_Textbox_Console.Text = _curGame.Console;
                GamesTab_Textbox_ReleaseDate.Text = _curGame.ReleaseDate;
                GamesTab_Textbox_CriticScore.Text = _curGame.CriticScore;
                GamesTab_Textbox_Publisher.Text = _curGame.Publisher;
                GamesTab_Textbox_Developer.Text = _curGame.Developer;
                GamesTab_Textbox_ESRB.Text = _curGame.Esrb;
                GamesTab_Textbox_Players.Text = _curGame.Players;
                GamesTab_Textbox_ESRBDescriptor.Text = _curGame.EsrbDescriptor;
                GamesTab_Textbox_Description.Text = _curGame.Description;
                RefreshEsrbIcon(_curGame);
            }

        /// <summary>
        /// Save database button
        /// Save all active info to the text databse
        /// </summary>
        private void GamesTab_SaveToDatabaseButton_Click(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            SaveGameInfo();
        }

        /// <summary>
        /// Save game info button
        /// </summary>
        private void GamesTab_SaveInfoButton_Click(object sender, EventArgs e)
        {
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
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
        /// Expand game media image #1
        /// NOTE: Image boxes do not detect clicks. This is a placeholder
        /// </summary>
        private void GamesTab_BoxfrontImage_Click(object sender, EventArgs e)
        {
            /*
            PictureBox pb = (PictureBox)sender;
            if (pb.Dock == DockStyle.None)
            {
                pb.Dock = DockStyle.Fill;
                pb.BringToFront();
            }
            else
                pb.Dock = DockStyle.None;
                */
        }

        /// <summary>
        /// Expand game media image #2
        /// </summary>
        private void GamesTab_BoxbackImage_Click(object sender, EventArgs e)
        {
            /*
            PictureBox pb = (PictureBox)sender;
            if (pb.Dock == DockStyle.None)
            {
                pb.Dock = DockStyle.Fill;
                pb.BringToFront();
            }
            else
                pb.Dock = DockStyle.None;
            */
        }

        /// <summary>
        /// Expand game media image #3
        /// </summary>
        private void GamesTab_ScreenshotImage_Click(object sender, EventArgs e)
        {
            /*Image image = (Image)sender;
            if (image.Dock == DockStyle.None)
            {
                image.Dock = DockStyle.Fill;
                image.BringToFront();
            }
            else
                image.Dock = DockStyle.None;*/
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

            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            SQLclient.UploadGame(_curGame);
            MessageBox.Show("Game Uploaded");
        }

        /// <summary>
        /// Sets the current game as a global favorite
        /// </summary>
        private void GamesTab_FavoriteCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Verify that a console/game is currently selected
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            //Toggle favorite checkbox
            if (GamesTab_CheckBox__GlobalFavorite.IsChecked == true)
                _curGame.Favorite = 1;
            else
                _curGame.Favorite = 0;
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
            foreach (Game g1 in _curConsole2.GameList)
            {
                WebOps.ScrapeInfo(g1);
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
            string curItem = EmulatorsTab_Listbox_ConsoleList.SelectedItem.ToString();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(curItem))
                {
                    _curConsole = c;
                    EmulatorsTab_Textbox_ConsoleName1.Text = c.Name;
                    GlobalTab_Textbox_EmulatorDirectory.Text = c.EmuPath;
                    EmulatorsTab_Textbox_ROMExtension.Text = c.RomExt;
                    EmulatorsTab_Textbox_EmulatorArgs.Text = c.LaunchParam;
                    EmulatorsTab_Textbox_ConsoleInfo.Text = c.ConsoleInfo;
                    EmulatorsTab_Textbox_GameCount.Text = c.GameCount.ToString();
                    EmulatorsTab_Textbox_ReleaseDate.Text = c.ReleaseDate;
                }
            }
        }

        /// <summary>
        /// Save console button
        /// Save current console info to database file
        /// </summary>
        private void EmulatorsTab_SaveDatabaseFileButton_Click(object sender, EventArgs e)
        {
            _curConsole.Name = EmulatorsTab_Textbox_ConsoleName1.Text;
            _curConsole.EmuPath = GlobalTab_Textbox_EmulatorDirectory.Text;
            _curConsole.RomExt = EmulatorsTab_Textbox_ROMExtension.Text;
            _curConsole.LaunchParam = EmulatorsTab_Textbox_EmulatorArgs.Text;
            _curConsole.ReleaseDate = EmulatorsTab_Textbox_ReleaseDate.Text;
            _curConsole.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
            FileOps.saveDatabase(Program._databasePath);
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
            if (Database.ConsoleList.Count < 2)
            {
                MessageBox.Show("Cannot have an empty console list");
                return;
            }
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            Database.ConsoleList.Remove(_curConsole);
            foreach (Console c in Database.ConsoleList)
            {
                EmulatorsTab_Listbox_ConsoleList.Items.Add(c.Name);
                GamesTab_Listbox_ConsoleList.Items.Add(c.Name);
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
            Console c = new Console()
            {
                Name = "New Console"
            };
            Database.ConsoleList.Add(c);
            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            GamesTab_Listbox_ConsoleList.Items.Clear();
            foreach (Console con in Database.ConsoleList)
            {
                EmulatorsTab_Listbox_ConsoleList.Items.Add(con.Name);
                GamesTab_Listbox_ConsoleList.Items.Add(con.Name);
            }
            GamesTab_Listbox_ConsoleList.SelectedIndex = (GamesTab_Listbox_ConsoleList.Items.Count - 1);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// //Force metadata rescrape (All games within console)
        /// </summary>
        private void EmulatorsTab_ForceGlobalMetadataRescrapeButton_Click(object sender, EventArgs e)
        {
            foreach (Game g in _curConsole.GameList)
            {
                if (!WebOps.ScrapeInfo(g))
                    return;
            }
        }

        /// <summary>
        /// Save the custom info fields for the current emulator
        /// </summary>
        private void EmulatorsTab_SaveInfoButton_Click(object sender, EventArgs e)
        {
            //Invalid input check
            if (EmulatorsTab_Textbox_ConsoleName1.Text.Contains("|") || GlobalTab_Textbox_EmulatorDirectory.Text.Contains("|") || GamesTab_Textbox_TotalGames.Text.Contains("|") || EmulatorsTab_Textbox_ROMExtension.Text.Contains("|") || EmulatorsTab_Textbox_EmulatorArgs.Text.Contains("|") || EmulatorsTab_Textbox_ReleaseDate.Text.Contains("|") || EmulatorsTab_Textbox_ConsoleInfo.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
                {
                    if (GamesTab_Textbox_ReleaseDate.Text.Length < 5)
                        _curConsole.ReleaseDate = EmulatorsTab_Textbox_ReleaseDate.Text;
                    else
                        MessageBox.Show("Release Date Invalid");
                }
                else
                    MessageBox.Show("Release Date score must be only digits");
                if ((EmulatorsTab_Textbox_ConsoleName1.Text.Length > 20) || (GlobalTab_Textbox_EmulatorDirectory.Text.Length > 100) || (GamesTab_Textbox_TotalGames.Text.Length > 100) || (EmulatorsTab_Textbox_ROMExtension.Text.Length > 30) || (EmulatorsTab_Textbox_ROMExtension.Text.Length > 300))
                    MessageBox.Show("Invalid Length");
                else
                {
                    //If all input checks are valid, set console into to the current text field values
                    _curConsole.Name = EmulatorsTab_Textbox_ConsoleName1.Text;
                    _curConsole.EmuPath = GlobalTab_Textbox_EmulatorDirectory.Text;
                    _curConsole.RomExt = EmulatorsTab_Textbox_ROMExtension.Text;
                    _curConsole.LaunchParam = EmulatorsTab_Textbox_EmulatorArgs.Text;
                    _curConsole.ConsoleInfo = EmulatorsTab_Textbox_ConsoleInfo.Text;
                }
                MainWindow.RefreshConsoleList();
            }

            EmulatorsTab_Listbox_ConsoleList.Items.Clear();
            foreach (Console c in Database.ConsoleList)
                EmulatorsTab_Listbox_ConsoleList.Items.Add(c.Name);
        }

        /// <summary>
        /// Toggle enforceExt checkbox
        /// </summary>
        private void EmulatorsTab_EnforceROMExtensionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (EmulatorsTab_Checkbox_EnforceFileExtension.IsChecked == true)
                _enforceExt = 1;
            else
                _enforceExt = 0;
        }

        /// <summary>
        /// Rescan all games across all emulators
        /// </summary>
        private void EmulatorsTab_GlobalRescanButton_Click(object sender, EventArgs e)
        {
            if (FileOps.scan(Program._romPath))
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
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(EmulatorsTab_Listbox_ConsoleList.SelectedItem.ToString()))
                {
                    if (FileOps.scanDirectory(c.RomPath, Program._romPath))
                    {
                        MessageBox.Show(c.Name + " Successfully Scanned");
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
            FileOps.savePreferences(Program._prefPath);
            this.Close();
        }

        /// <summary>
        /// Refresh user info under the User tab every time a new user is selected
        /// </summary>
        private void UsersTab_UsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update the current user text         
            if (_curUser != null)
                UsersTab_Label_CurrentUser.Content = "Current User: " + _curUser.Username;

            //Populate the favorites list for each user
            UsersTab_Listbox_UserFavorites.Items.Clear();
            foreach (User u in Database.UserList)
            {
                if (u.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
                {
                    if (u.Favorites.Count > 0)
                    {
                        foreach (Game g in u.Favorites)
                            UsersTab_Listbox_UserFavorites.Items.Add(g.Title + " - " + g.Console);
                    }

                    UsersTab_Textbox_Username.Text = u.Username;
                    UsersTab_Textbox_Email.Text = u.Email;
                    UsersTab_Textbox_UserInfo.Text = u.UserInfo;
                    UsersTab_Textbox_LoginCount.Text = u.LoginCount.ToString();
                    UsersTab_Textbox_LaunchCount.Text = u.TotalLaunchCount.ToString();
                    UsersTab_Dropdown_AllowedESRB.Text = u.AllowedEsrb;

                    //Only allow the current user to edit their own userdata
                    bool editEnabled = u.Username.Equals(_curUser.Username);
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
            foreach (User us in Database.UserList)
            {
                if (_curUser.Username.Equals(us.Username))
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(_curUser);
                    break;
                }
            }

            //Create a new unicade account and display the dialog
            AccountWindow uc = new AccountWindow(1);
            uc.ShowDialog();

            //Update the current labels and save the user info to the preferences file
            UsersTab_Label_CurrentUser.Content = "Current User: " + _curUser.Username;
            FileOps.savePreferences(Program._prefPath);

            //Refresh the listbox contents
            UsersTab_Listbox_CurrentUser.Items.Clear();
            foreach (User us in Database.UserList)
                UsersTab_Listbox_CurrentUser.Items.Add(us.Username);
        }

        /// <summary>
        /// Save button
        /// </summary>
        private void UsersTab_SaveAllUsersButton_Click(object sender, EventArgs e)
        {
            FileOps.savePreferences(Program._prefPath);
        }

        /// <summary>
        /// Delete the currently selected user from the database
        /// </summary>
        private void UsersTab_DeleteUserButton_Click(object sender, EventArgs e)
        {
            //Ensure that there is always at least one user present in the database
            if (Database.UserList.Count <= 1)
            {
                MessageBox.Show("Must at least have one user");
                return;
            }

            //Remove the user and refresh the database
            Database.UserList.Remove(_curUser);
            UsersTab_Listbox_CurrentUser.Items.Clear();
            _curUser = null;
            foreach (User us in Database.UserList)
                UsersTab_Listbox_CurrentUser.Items.Add(us.Username);
        }

        /// <summary>
        /// Save button (Global Settings tab)
        /// Save the current global settings to the preferences file
        /// </summary>
        private void UsersTab_SaveButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currently logged in
            if (!_curUser.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            if (UsersTab_Textbox_Username.Text.Contains("|") || UsersTab_Textbox_Email.Text.Contains("|") || UsersTab_Textbox_UserInfo.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if ((UsersTab_Textbox_Username.Text.Length > 20) || (UsersTab_Textbox_Email.Text.Length > 20) || (UsersTab_Textbox_UserInfo.Text.Length > 50))
                    MessageBox.Show("Invalid Length");
                else
                {
                    _curUser.Username = UsersTab_Textbox_Username.Text;
                    _curUser.Pass = UsersTab_Textbox_Email.Text;
                    _curUser.UserInfo = UsersTab_Textbox_UserInfo.Text;
                }

                if (GamesTab_Textbox_ESRB.Text.Contains("Everyone") || GamesTab_Textbox_ESRB.Text.Contains("Teen") || GamesTab_Textbox_ESRB.Text.Contains("Mature") || GamesTab_Textbox_ESRB.Text.Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                {
                    if (UsersTab_Dropdown_AllowedESRB.SelectedItem != null)
                        _curUser.AllowedEsrb = UsersTab_Dropdown_AllowedESRB.SelectedItem.ToString();
                }
                else
                    MessageBox.Show("Invalid ESRB Rating");
            }
            UsersTab_Listbox_CurrentUser.Items.Clear();

            foreach (User us in Database.UserList)
                UsersTab_Listbox_CurrentUser.Items.Add(us.Username);
        }

        /// <summary>
        /// Delete user favorite
        /// </summary>
        private void UsersTab_DeleteFavoriteButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currenly logged in
            if (!_curUser.Username.Equals(UsersTab_Listbox_CurrentUser.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            _curUser.Favorites.RemoveAt(UsersTab_Listbox_UserFavorites.SelectedIndex);
            UsersTab_Listbox_UserFavorites.Items.Clear();
            foreach (Game g in _curUser.Favorites)
            {
                UsersTab_Listbox_UserFavorites.Items.Add(g.Title + " - " + g.Console);
            }
        }

        /// <summary>
        /// Login local user button
        /// </summary>
        private void UsersTab_LoginButton_Click(object sender, EventArgs e)
        {
            foreach (User us in Database.UserList)
            {
                if (_curUser.Username.Equals(us.Username))
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(_curUser);
                    break;
                }
            }

            //Display the login dialog
            LoginWindow login = new LoginWindow(1);
            login.ShowDialog();
            if (_curUser != null)
            {
                //If the user is logged in sucuesfully, save the current user and preferences file
                UsersTab_Label_CurrentUser.Content = "Current User: " + _curUser.Username;
                FileOps.savePreferences(Program._prefPath);
            }
        }

        /// <summary>
        /// Refresh the current users list and userdata
        /// </summary>
        private void UsersTab_RefreshButton_Click(object sender, EventArgs e)
        {
            UsersTab_Label_CurrentUser.Content = "Current User: " + _curUser.Username;
        }

        #endregion

        #region Global Settings Tab

        /// <summary>
        /// Modify the global ESRB rating when the dropdown is changed
        /// </summary>
        private void GlobalSettingsTab_AllowedEsrbRatingDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            _restrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
        }

        /// <summary>
        /// Save Global Settings button
        /// </summary>
        private void GlobalSettings_SavePreferenceFileButton_Click(object sender, EventArgs e)
        {
            if (GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("|") || GlobalTab_Textbox_EmulatorDirectory.Text.Contains("|") || GlobalTab_Textbox_MedaDirectory.Text.Contains("|") || GlobalTab_Textbox_ROMDirectory.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Everyone") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Teen") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Mature") || GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString().Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                    _restrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((GlobalTab_Textbox_EmulatorDirectory.Text.Length > 150) || (GlobalTab_Textbox_MedaDirectory.Text.Length > 150) || (GlobalTab_Textbox_ROMDirectory.Text.Length > 150))
                    MessageBox.Show("Invalid Length");
                else
                {
                    Program._emuPath = GlobalTab_Textbox_EmulatorDirectory.Text;
                    Program._mediaPath = GlobalTab_Textbox_MedaDirectory.Text;
                    Program._romPath = GlobalTab_Textbox_ROMDirectory.Text;
                }

                Int32.TryParse(GlobalTab_Textbox_Password.Text, out int n);
                if (n > 0)
                    _passProtect = Int32.Parse(GlobalTab_Textbox_Password.Text);
                Int32.TryParse(GlobalTab_Textbox_Coins.Text, out n);
                if (n > 0)
                    _coins = Int32.Parse(GlobalTab_Textbox_Coins.Text);
                if (GlobalTab_Dropdown_AllowedESRB.SelectedItem != null)
                    _restrictESRB = CalcEsrb(GlobalTab_Dropdown_AllowedESRB.SelectedItem.ToString());

                //Save all active preferences to the local preferences file
                FileOps.savePreferences(Program._prefPath);
            }
        }

        /// <summary>
        /// Toggle viewEsrb checkbox
        /// </summary>
        private void GlobalSettingsTab_AllowedToViewEsrbCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_ToView.IsChecked == true)
                _viewEsrb = 1;
            else
                _viewEsrb = 0;
        }

        /// <summary>
        /// Toggle splash screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleSplashCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplaySplash.IsChecked == true)
                _showSplash = 1;
            else
                _showSplash = 0;
        }

        /// <summary>
        /// Toggle show loading screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleLoadingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplayLoadingScreen.IsChecked == true)
                _showLoading = 1;
            else
                _showLoading = 0;
        }

        /// <summary>
        /// Toggle require login checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleRequireLoginCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_RequireLogin.IsChecked == true)
                _requireLogin = 1;
            else
                _requireLogin = 0;
        }

        /// <summary>
        /// Toggle scan on startup checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleScanOnStartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_RescanAllLibraries.IsChecked == true)
                _scanOnStartup = 1;
            else
                _scanOnStartup = 0;
        }

        /// <summary>
        /// Toggle view ESRB checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleEsrbViewCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_DisplayESRB.IsChecked == true)
                _viewEsrb = 1;
            else
                _viewEsrb = 0;
        }

        /// <summary>
        /// Toggle payPerPlay checkbox
        /// </summary>
        private void GlobalSettingsTab_TogglePayPerPlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalTab_Checkbox_EnablePayPerPlay.IsChecked == true)
                _payPerPlay = 1;
            else
                _payPerPlay = 0;
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
            if (WebTab_Checkbox_Metacritic.IsChecked == true)
                WebOps.metac = 1;
            else
                WebOps.metac = 0;
        }

        /// <summary>
        /// Toggle Mobygames checkbox
        /// </summary>
        private void WebTab_Checkbox_Mobygames_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Mobygames1.IsChecked == true)
                WebOps.metac = 1;
            else
                WebOps.metac = 0;
        }

        /// <summary>
        /// Toggle release date checkbox
        /// </summary>
        private void WebTab_Checkbox_ReleaseDate_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ReleaseDate.IsChecked == true)
                WebOps.releaseDate = 1;
            else
                WebOps.releaseDate = 0;
        }

        /// <summary>
        /// Toggle critic score checkbox
        /// </summary>
        private void WebTab_Checkbox_CriticScore_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_CriticScore.IsChecked == true)
                WebOps.critic = 1;
            else
                WebOps.critic = 0;
        }

        /// <summary>
        /// Toggle Publisher checkbox
        /// </summary>
        private void WebTab_Checkbox_Publisher_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Publisher.IsChecked == true)
                WebOps.publisher = 1;
            else
                WebOps.publisher = 0;
        }

        /// <summary>
        /// Toggle developer checkbox
        /// </summary>
        private void WebTab_Checkbox_Developer_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Developer.IsChecked == true)
                WebOps.developer = 1;
            else
                WebOps.developer = 0;
        }

        /// <summary>
        /// Toggle ESRB Rating checkbox
        /// </summary>
        private void WebTab_Checkbox_ESRBRating_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBRating.IsChecked == true)
                WebOps.esrb = 1;
            else
                WebOps.esrb = 0;
        }

        /// <summary>
        /// Toggle ESRB Descriptor checkbox
        /// </summary>
        private void WebTab_Checkbox_ESRBDescriptor_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBDescriptor.IsChecked == true)
                WebOps.description = 1;
            else
                WebOps.description = 0;
        }

        /// <summary>
        /// Toggle players checkbox
        /// </summary>
        private void WebTab_Checkbox_Players_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Players.IsChecked == true)
                WebOps.players = 1;
            else
                WebOps.players = 0;
        }

        /// <summary>
        /// Toggle description checkbox
        /// </summary>
        private void WebTab_Checkbox_Description_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_ESRBDescriptor.IsChecked == true)
                WebOps.description = 1;
            else
                WebOps.description = 0;
        }

        /// <summary>
        /// Toggle boxfront checkbox
        /// </summary>
        private void WebTab_Checkbox_BoxFront_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_BoxFront.IsChecked == true)
                WebOps.boxFront = 1;
            else
                WebOps.boxFront = 0;
        }

        /// <summary>
        /// Toggle box back checkbox
        /// </summary>
        private void WebTab_Checkbox_BoxBack_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_BoxBack.IsChecked == true)
                WebOps.boxBack = 1;
            else
                WebOps.boxBack = 0;
        }

        /// <summary>
        /// Toggle screenshot textbox
        /// </summary>
        private void WebTab_Checkbox_Screenshot_Checked(object sender, RoutedEventArgs e)
        {
            if (WebTab_Checkbox_Screenshot.IsChecked == true)
                WebOps.screenshot = 1;
            else
                WebOps.screenshot = 0;
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
            AboutTab_Label_LicensedTo.Content = "Licensed to: " + Program._userLicenseName;
            AboutTab_Label_LicenseKey.Content = "License Key: " + Program._userLicenseKey;

            //Set the license text depending on if the key is valid
            if (Program._validLicense)
                AboutTab_Label_Edition.Content = "License Status: Full Version";
            else
                AboutTab_Label_Edition.Content = "License Status: INVALID";

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
                    return false;
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
                EsrbNum = 1;
            else if (esrb.Equals("Everyone 10+"))
                EsrbNum = 2;
            else if (esrb.Equals("Teen"))
                EsrbNum = 3;
            else if (esrb.Equals("Mature"))
                EsrbNum = 4;
            else if (esrb.Equals("Adults Only (AO)"))
                EsrbNum = 5;
            else if (esrb.Equals("None"))
                EsrbNum = 0;
            else
                EsrbNum = 0;
            return EsrbNum;
        }

        /// <summary>
        /// Refresh the current game info passed in as a Game object
        /// </summary>
        public void RefreshGameInfo(Game game)
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
            GamesTab_Textbox_Console.Text = game.Console;
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
                GamesTab_CheckBox__GlobalFavorite.IsChecked = true;
            else
                GamesTab_CheckBox__GlobalFavorite.IsChecked = false;
       


            GamesTab_Image_Boxfront.Source = null;
            GamesTab_Image_Boxback.Source = null;
            GamesTab_Image_Screeshot.Source = null;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png"))
                GamesTab_Image_Boxfront.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png"));
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png"))
                GamesTab_Image_Boxback.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png"));
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png"))
                GamesTab_Image_Screeshot.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png"));
       
    }

        /// <summary>
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        private void SaveGameInfo()
        {
            
            //Invalid input checks
            if (GamesTab_Listbox_GamesList.Items.Count < 1)
                return;
            if (GamesTab_Listbox_ConsoleList.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
            {
                if (GamesTab_Textbox_ReleaseDate.Text.Length < 5)
                    _curGame.ReleaseDate = GamesTab_Textbox_ReleaseDate.Text;
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
                    _curGame.CriticScore = GamesTab_Textbox_CriticScore.Text;
                else
                    MessageBox.Show("Critic Score Invalid");
            }
            else
                MessageBox.Show("Critic Score must be only digits");
            if (IsAllDigits(GamesTab_Textbox_ReleaseDate.Text))
            {
                if (GamesTab_Textbox_ReleaseDate.Text.Length > 2)
                    _curGame.Players = GamesTab_Textbox_Players.Text;
                else
                    MessageBox.Show("Players Invalid");
            }
            else
                MessageBox.Show("Players must be only digits");
            if (GamesTab_Textbox_Developer.Text.Contains("|") || GamesTab_Textbox_Publisher.Text.Contains("|") || GamesTab_Textbox_ESRB.Text.Contains("|") || GamesTab_Textbox_Description.Text.Contains("|") || GamesTab_Textbox_ESRBDescriptor.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (GamesTab_Textbox_ESRB.Text.Contains("Everyone") || GamesTab_Textbox_ESRB.Text.Contains("Teen") || GamesTab_Textbox_ESRB.Text.Contains("Mature") || GamesTab_Textbox_ESRB.Text.Contains("Adults") || GamesTab_Textbox_ESRB.Text.Length < 1)
                    _curGame.Esrb = GamesTab_Textbox_ESRB.Text;
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((GamesTab_Textbox_Developer.Text.Length > 20) || (GamesTab_Textbox_Publisher.Text.Length > 20) || (GamesTab_Textbox_Description.Text.Length > 20) || (GamesTab_Textbox_ESRBDescriptor.Text.Length > 20))
                    MessageBox.Show("Invalid Length");
                else
                {
                    _curGame.Publisher = GamesTab_Textbox_Publisher.Text;
                    _curGame.Developer = GamesTab_Textbox_Developer.Text;
                    _curGame.Description = GamesTab_Textbox_Description.Text;
                    _curGame.EsrbDescriptor = GamesTab_Textbox_ESRBDescriptor.Text;
                }
            }

            //If all input fields are valid, save the database
            FileOps.saveDatabase(Program._databasePath);
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void RefreshEsrbIcon(Game g)
        {
            GamesTab_Image_ESRB.Source = null;
            if (g.Esrb.Equals("Everyone"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png"));
            else if (g.Esrb.Equals("Everyone (KA)"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png"));
            else if (g.Esrb.Equals("Everyone 10+"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png"));
            else if (g.Esrb.Equals("Teen"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png"));
            else if (g.Esrb.Equals("Mature"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png"));
            if (g.Esrb.Equals("Adults Only (AO)"))
                GamesTab_Image_ESRB.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png"));
        }

        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
            GlobalTab_Listbox_GlobalFavorites.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.GameCount > 0)
                {
                    foreach (Game g in c.GameList)
                    {
                        if (g.Favorite > 0)
                        {
                            GlobalTab_Listbox_GlobalFavorites.Items.Add(g.Title + " (" + g.Console + ")");
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
