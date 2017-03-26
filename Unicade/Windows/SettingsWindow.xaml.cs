using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        public SettingsWindow()
        {
            InitializeComponent();
        }

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
                    textBox8.Text = c.GameCount.ToString();
                    textBox3.Text = Database.TotalGameCount.ToString();
                    if (c.GameCount > 0)
                    {
                        foreach (Game g in c.GameList)
                        {
                            listBox3.Items.Add(g.Title);
                        }
                    }
                }
            }
            if (listBox3.Items.Count > 0)
            {
                listBox3.SelectedIndex = 0;
                foreach (Game g in _curConsole2.GameList)
                {
                    if (g.Title.Equals(listBox3.SelectedItem.ToString()))
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
            string curItem = listBox3.SelectedItem.ToString();
            foreach (Game g in _curConsole2.GameList)
            {
                if (g.Title.Equals(curItem))
                {
                    _curGame = g;
                }
            }
            RefreshGameInfo(_curGame);
            RefreshEsrbIcon(_curGame);
        }

        private void GamesTab_RescrapeGameButton_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// Rescrape game info button.
            /// Rescrapes info the the specified game from the web
            /// </summary>
            private void GamesTab_RescrapeGameButton_Click(object sender, EventArgs e)
            {
                //Require that a user select a valid game to rescrape
                if (listBox2.SelectedItem == null)
                {
                    MessageBox.Show("Must select a console/game");
                    return;
                }

                //Scrape info and populate local fields
                WebOps.ScrapeInfo(_curGame);
                textBox2.Text = _curGame.Title;
                textBox13.Text = _curGame.Console;
                textBox12.Text = _curGame.ReleaseDate;
                textBox15.Text = _curGame.CriticScore;
                textBox11.Text = _curGame.Publisher;
                textBox10.Text = _curGame.Developer;
                textBox6.Text = _curGame.Esrb;
                textBox17.Text = _curGame.Players;
                textBox19.Text = _curGame.EsrbDescriptor;
                textBox18.Text = _curGame.Description;
                RefreshEsrbIcon(_curGame);
            }
        }

        /// <summary>
        /// Save database button
        /// Save all active info to the text databse
        /// </summary>
        private void GamesTab_SaveToDatabaseButton_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
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
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            if (listBox3.Items.Count < 1)
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
            PictureBox pb = (PictureBox)sender;
            if (pb.Dock == DockStyle.None)
            {
                pb.Dock = DockStyle.Fill;
                pb.BringToFront();
            }
            else
                pb.Dock = DockStyle.None;
        }

        /// <summary>
        /// Expand game media image #2
        /// </summary>
        private void GamesTab_BoxbackImage_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.Dock == DockStyle.None)
            {
                pb.Dock = DockStyle.Fill;
                pb.BringToFront();
            }
            else
                pb.Dock = DockStyle.None;
        }

        /// <summary>
        /// Expand game media image #3
        /// </summary>
        private void GamesTab_ScreenshotImage_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.Dock == DockStyle.None)
            {
                pb.Dock = DockStyle.Fill;
                pb.BringToFront();
            }
            else
                pb.Dock = DockStyle.None;
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

            if (listBox3.Items.Count < 1)
            {
                MessageBox.Show("No games to upload");
                return;
            }

            if (listBox2.SelectedItem == null)
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
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            //Toggle favorite checkbox
            if (checkBox3.Checked)
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
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (listBox2.SelectedItem == null)
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
            string curItem = listBox1.SelectedItem.ToString();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(curItem))
                {
                    _curConsole = c;
                    textBox9.Text = c.Name;
                    textBox1.Text = c.EmuPath;
                    textBox4.Text = c.RomExt;
                    textBox5.Text = c.LaunchParam;
                    textBox20.Text = c.ConsoleInfo;
                    textBox21.Text = c.GameCount.ToString();
                    textBox22.Text = c.ReleaseDate;
                }
            }
        }

        /// <summary>
        /// Save console button
        /// Save current console info to database file
        /// </summary>
        private void EmulatorsTab_SaveDatabaseFileButton_Click(object sender, EventArgs e)
        {
            _curConsole.Name = textBox9.Text;
            _curConsole.EmuPath = textBox1.Text;
            _curConsole.RomExt = textBox4.Text;
            _curConsole.LaunchParam = textBox5.Text;
            _curConsole.ReleaseDate = textBox22.Text;
            _curConsole.ConsoleInfo = textBox20.Text;
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
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            Database.ConsoleList.Remove(_curConsole);
            foreach (Console c in Database.ConsoleList)
            {
                listBox1.Items.Add(c.Name);
                listBox2.Items.Add(c.Name);
            }
            listBox1.SelectedIndex = 0;

            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// Add a new console
        /// </summary>
        private void EmulatorsTab_AddNewConsoleButton_Click(object sender, EventArgs e)
        {
            //Clear all text boxes initially 
            textBox1.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox20.Text = null;
            textBox21.Text = null;
            textBox22.Text = null;

            //Create a new console and add it to the datbase
            Console c = new Console()
            {
                Name = "New Console"
            };
            Database.ConsoleList.Add(c);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach (Console con in Database.ConsoleList)
            {
                listBox1.Items.Add(con.Name);
                listBox2.Items.Add(con.Name);
            }
            listBox2.SelectedIndex = (listBox2.Items.Count - 1);
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
            if (textBox9.Text.Contains("|") || textBox1.Text.Contains("|") || textBox3.Text.Contains("|") || textBox4.Text.Contains("|") || textBox5.Text.Contains("|") || textBox22.Text.Contains("|") || textBox20.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (IsAllDigits(textBox12.Text))
                {
                    if (textBox12.TextLength < 5)
                        _curConsole.ReleaseDate = textBox22.Text;
                    else
                        MessageBox.Show("Release Date Invalid");
                }
                else
                    MessageBox.Show("Release Date score must be only digits");
                if ((textBox9.Text.Length > 20) || (textBox1.Text.Length > 100) || (textBox3.Text.Length > 100) || (textBox4.Text.Length > 30) || (textBox3.Text.Length > 40) || (textBox4.Text.Length > 300))
                    MessageBox.Show("Invalid Length");
                else
                {
                    //If all input checks are valid, set console into to the current text field values
                    _curConsole.Name = textBox9.Text;
                    _curConsole.EmuPath = textBox1.Text;
                    _curConsole.RomExt = textBox4.Text;
                    _curConsole.LaunchParam = textBox5.Text;
                    _curConsole.ConsoleInfo = textBox20.Text;
                }
                MainWindow.RefreshConsoleList();
            }

            listBox1.Items.Clear();
            foreach (Console c in Database.ConsoleList)
                listBox1.Items.Add(c.Name);
        }

        /// <summary>
        /// Toggle enforceExt checkbox
        /// </summary>
        private void EmulatorsTab_EnforceROMExtensionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
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
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(listBox1.SelectedItem.ToString()))
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
                label38.Text = "Current User: " + _curUser.Username;

            //Populate the favorites list for each user
            listBox5.Items.Clear();
            foreach (User u in Database.UserList)
            {
                if (u.Username.Equals(listBox4.SelectedItem.ToString()))
                {
                    if (u.Favorites.Count > 0)
                    {
                        foreach (Game g in u.Favorites)
                            listBox5.Items.Add(g.Title + " - " + g.Console);
                    }

                    textBox23.Text = u.Username;
                    textBox24.Text = u.Email;
                    textBox26.Text = u.UserInfo;
                    textBox27.Text = u.LoginCount.ToString();
                    textBox28.Text = u.TotalLaunchCount.ToString();
                    comboBox2.Text = u.AllowedEsrb;

                    //Only allow the current user to edit their own userdata
                    bool editEnabled = u.Username.Equals(_curUser.Username);
                    textBox23.Enabled = editEnabled;
                    textBox24.Enabled = editEnabled;
                    textBox26.Enabled = editEnabled;
                    textBox27.Enabled = editEnabled;
                    textBox28.Enabled = editEnabled;
                    comboBox2.Enabled = editEnabled;
                    listBox5.Enabled = editEnabled;
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
            label38.Text = "Current User: " + _curUser.Username;
            FileOps.savePreferences(Program._prefPath);

            //Refresh the listbox contents
            listBox4.Items.Clear();
            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
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
            listBox4.Items.Clear();
            _curUser = null;
            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
        }

        /// <summary>
        /// Save button (Global Settings tab)
        /// Save the current global settings to the preferences file
        /// </summary>
        private void UsersTab_SaveButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currently logged in
            if (!_curUser.Username.Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            if (textBox23.Text.Contains("|") || textBox24.Text.Contains("|") || textBox26.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if ((textBox23.Text.Length > 20) || (textBox24.Text.Length > 20) || (textBox26.Text.Length > 50))
                    MessageBox.Show("Invalid Length");
                else
                {
                    _curUser.Username = textBox23.Text;
                    _curUser.Pass = textBox24.Text;
                    _curUser.UserInfo = textBox26.Text;
                }

                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults") || textBox6.TextLength < 1)
                {
                    if (comboBox2.SelectedItem != null)
                        _curUser.AllowedEsrb = comboBox2.SelectedItem.ToString();
                }
                else
                    MessageBox.Show("Invalid ESRB Rating");
            }
            listBox4.Items.Clear();

            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
        }

        /// <summary>
        /// Delete user favorite
        /// </summary>
        private void UsersTab_DeleteFavoriteButton_Click(object sender, EventArgs e)
        {
            //Verify that a user is currenly logged in
            if (!_curUser.Username.Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            _curUser.Favorites.RemoveAt(listBox5.SelectedIndex);
            listBox5.Items.Clear();
            foreach (Game g in _curUser.Favorites)
            {
                listBox5.Items.Add(g.Title + " - " + g.Console);
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
                label38.Text = "Current User: " + _curUser.Username;
                FileOps.savePreferences(Program._prefPath);
            }
        }

        /// <summary>
        /// Refresh the current users list and userdata
        /// </summary>
        private void UsersTab_RefreshButton_Click(object sender, EventArgs e)
        {
            label38.Text = "Current User: " + _curUser.Username;
        }

        #endregion

        #region Global Settings Tab

        /// <summary>
        /// Modify the global ESRB rating when the dropdown is changed
        /// </summary>
        private void GlobalSettingsTab_AllowedEsrbRatingDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            _restrictESRB = CalcEsrb(comboBox1.SelectedItem.ToString());
        }

        /// <summary>
        /// Save Global Settings button
        /// </summary>
        private void GlobalSettings_SavePreferenceFileButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Contains("|") || textBox25.Text.Contains("|") || textBox32.Text.Contains("|") || textBox33.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (comboBox1.SelectedItem.ToString().Contains("Everyone") || comboBox1.SelectedItem.ToString().Contains("Teen") || comboBox1.SelectedItem.ToString().Contains("Mature") || comboBox1.SelectedItem.ToString().Contains("Adults") || textBox6.TextLength < 1)
                    _restrictESRB = CalcEsrb(comboBox1.SelectedItem.ToString());
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((textBox25.Text.Length > 150) || (textBox32.Text.Length > 150) || (textBox33.Text.Length > 150))
                    MessageBox.Show("Invalid Length");
                else
                {
                    Program._emuPath = textBox25.Text;
                    Program._mediaPath = textBox32.Text;
                    Program._romPath = textBox33.Text;
                }

                Int32.TryParse(textBox7.Text, out int n);
                if (n > 0)
                    _passProtect = Int32.Parse(textBox7.Text);
                Int32.TryParse(textBox29.Text, out n);
                if (n > 0)
                    _coins = Int32.Parse(textBox29.Text);
                if (comboBox1.SelectedItem != null)
                    _restrictESRB = CalcEsrb(comboBox1.SelectedItem.ToString());

                //Save all active preferences to the local preferences file
                FileOps.savePreferences(Program._prefPath);
            }
        }

        /// <summary>
        /// Toggle viewEsrb checkbox
        /// </summary>
        private void GlobalSettingsTab_AllowedToViewEsrbCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                _viewEsrb = 1;
            else
                _viewEsrb = 0;
        }

        /// <summary>
        /// Toggle splash screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleSplashCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
                _showSplash = 1;
            else
                _showSplash = 0;
        }

        /// <summary>
        /// Toggle show loading screen checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleLoadingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                _showLoading = 1;
            else
                _showLoading = 0;
        }

        /// <summary>
        /// Toggle require login checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleRequireLoginCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
                _requireLogin = 1;
            else
                _requireLogin = 0;
        }

        /// <summary>
        /// Toggle scan on startup checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleScanOnStartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
                _scanOnStartup = 1;
            else
                _scanOnStartup = 0;
        }

        /// <summary>
        /// Toggle view ESRB checkbox
        /// </summary>
        private void GlobalSettingsTab_ToggleEsrbViewCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
                _viewEsrb = 1;
            else
                _viewEsrb = 0;
        }

        /// <summary>
        /// Toggle payPerPlay checkbox
        /// </summary>
        private void GlobalSettingsTab_TogglePayPerPlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
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
                label56.Text = "Current Web User: " + SQLclient.sqlUser;
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
                label56.Text = "Current Web User: ";
                return;
            }

            //Log the current user out and update the interface
            SQLclient.sqlUser = null;
            label56.Text = "Current Web User: ";
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
            label56.Text = "Current Web User: ";
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

        #endregion

        #region Helper Methods

        /// <summary>
        /// Placeholder method
        /// </summary>
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

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
            /*if (game == null)
            {
                //If no game is currently selected, set all info fields to null
                textBox2.Text = null;
                textBox13.Text = null;
                textBox12.Text = null;
                textBox15.Text = null;
                textBox11.Text = null;
                textBox10.Text = null;
                textBox6.Text = null;
                textBox17.Text = null;
                textBox19.Text = null;
                textBox18.Text = null;
                return;
            }

            //If a valid game is selected, update all info fields
            textBox2.Text = game.Title;
            textBox13.Text = game.Console;
            textBox12.Text = game.ReleaseDate;
            textBox15.Text = game.CriticScore;
            textBox11.Text = game.Publisher;
            textBox10.Text = game.Developer;
            textBox6.Text = game.Esrb;
            textBox17.Text = game.Players;
            textBox19.Text = game.EsrbDescriptor;
            textBox18.Text = game.Description;

            //Set favorite checkbox
            if (game.Favorite == 1)
                checkBox3.Checked = true;
            else
                checkBox3.Checked = false;

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png"))
                pictureBox1.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png"))
                pictureBox2.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png"))
                pictureBox3.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png");
    */   
    }

        /// <summary>
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        private void SaveGameInfo()
        {
            /*
            //Invalid input checks
            if (listBox3.Items.Count < 1)
                return;
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    _curGame.ReleaseDate = textBox12.Text;
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
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    _curGame.CriticScore = textBox15.Text;
                else
                    MessageBox.Show("Critic Score Invalid");
            }
            else
                MessageBox.Show("Critic Score must be only digits");
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength > 2)
                    _curGame.Players = textBox17.Text;
                else
                    MessageBox.Show("Players Invalid");
            }
            else
                MessageBox.Show("Players must be only digits");
            if (textBox10.Text.Contains("|") || textBox11.Text.Contains("|") || textBox6.Text.Contains("|") || textBox18.Text.Contains("|") || textBox19.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults") || textBox6.TextLength < 1)
                    _curGame.Esrb = textBox6.Text;
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((textBox10.Text.Length > 20) || (textBox11.Text.Length > 20) || (textBox18.Text.Length > 20) || (textBox19.Text.Length > 20))
                    MessageBox.Show("Invalid Length");
                else
                {
                    _curGame.Publisher = textBox11.Text;
                    _curGame.Developer = textBox10.Text;
                    _curGame.Description = textBox18.Text;
                    _curGame.EsrbDescriptor = textBox19.Text;
                }
            }

            //If all input fields are valid, save the database
            FileOps.saveDatabase(Program._databasePath);
            */
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void RefreshEsrbIcon(Game g)
        {
            /*
            pictureBox4.Image = null;
            if (g.Esrb.Equals("Everyone"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png");
            else if (g.Esrb.Equals("Everyone (KA)"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png");
            else if (g.Esrb.Equals("Everyone 10+"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png");
            else if (g.Esrb.Equals("Teen"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png");
            else if (g.Esrb.Equals("Mature"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png");
            if (g.Esrb.Equals("Adults Only (AO)"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png");
            */
        }

        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
            /*
            listBox6.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.GameCount > 0)
                {
                    foreach (Game g in c.GameList)
                    {
                        if (g.Favorite > 0)
                        {
                            listBox6.Items.Add(g.Title + " (" + g.Console + ")");
                        }
                    }
                }
            }
            */
        }









        #endregion

    }
}
