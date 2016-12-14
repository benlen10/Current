using System;
using System.IO;
using System.Windows.Forms;

namespace UniCade
{
    public partial class SettingsWindow : Form
    {
        #region Properties

        public static Game curGame;
        public static User curUser;
        static Console curConsole2;
        static Console curConsole;
        public static string defaultUser;
        public static int showSplash;
        public static int scanOnStartup;
        public static int restrictESRB;
        public static int requireLogin;
        public static int cmdOrGui;
        public static int showLoading;
        public static int payPerPlay;
        public static int coins;
        public static int playtime;
        public static int perLaunch;
        public static int viewEsrb;
        public static int passProtect;
        public static int enforceExt;

        #endregion

        public SettingsWindow()
        {
            InitializeComponent();
            FormClosing += SettingsWindow_FormClosing;
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
                listBox1.Items.Add(c.Name);
                listBox2.Items.Add(c.Name);
            }

            //Poplate ESRB dropdown combo boxes
            listBox1.SelectedIndex = 0;
            comboBox1.Items.Add("Everyone");
            comboBox1.Items.Add("Everyone 10+");
            comboBox1.Items.Add("Teen");
            comboBox1.Items.Add("Mature");
            comboBox1.Items.Add("Adults Only (AO)");
            comboBox1.Items.Add("None");
            comboBox2.Items.Add("Everyone");
            comboBox2.Items.Add("Everyone 10+");
            comboBox2.Items.Add("Teen");
            comboBox2.Items.Add("Mature");
            comboBox2.Items.Add("Adults Only (AO)");
            comboBox2.Items.Add("None");

            //Load UniCade Logo images within the settings window
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Load(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png");
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Load(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png");
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Load(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png");

            //Populate the 'Allowed ESRB' combo box with the specified rating
            if (restrictESRB == 0)
                comboBox1.Text = "None";
            else if (restrictESRB == 1)
                comboBox1.Text = "Everyone";
            else if (restrictESRB == 2)
                comboBox1.Text = "Everyone 10+";
            else if (restrictESRB == 3)
                comboBox1.Text = "Teen";
            else if (restrictESRB == 4)
                comboBox1.Text = "Mature";
            else if (restrictESRB == 5)
                comboBox1.Text = "Adults Only (AO)";
            if (viewEsrb > 0)
                checkBox6.Checked = true;

            //Disable editing userinfo unless logged in
            textBox23.Enabled = false;
            textBox24.Enabled = false;
            textBox26.Enabled = false;
            textBox27.Enabled = false;
            textBox28.Enabled = false;
            textBox25.Enabled = false;
            textBox31.Enabled = false;
            comboBox2.Enabled = false;
            listBox5.Enabled = false;

            //Populate features textbox under the About tab
            richTextBox1.Text = TextFiles.features + "\n\n\n\n\n\n" + TextFiles.instructions;

            //Populate textbox fields
            textBox7.Text = passProtect.ToString();
            textBox31.Text = Program._databasePath;
            textBox25.Text = Program._emuPath;
            textBox32.Text = Program._mediaPath;
            textBox33.Text = Program._romPath;

            //Check specified boxes under the Web tab
            if (WebOps.releaseDate > 0)
                checkBox8.Checked = true;
            if (WebOps.critic > 0)
                checkBox9.Checked = true;
            if (WebOps.publisher > 0)
                checkBox15.Checked = true;
            if (WebOps.developer > 0)
                checkBox17.Checked = true;
            if (WebOps.esrb > 0)
                checkBox18.Checked = true;
            if (WebOps.esrbDescriptor > 0)
                checkBox19.Checked = true;
            if (WebOps.players > 0)
                checkBox20.Checked = true;
            if (WebOps.description > 0)
                checkBox21.Checked = true;
            if (WebOps.boxFront > 0)
                checkBox22.Checked = true;
            if (WebOps.boxBack > 0)
                checkBox23.Checked = true;
            if (WebOps.screenshot > 0)
                checkBox24.Checked = true;
            if (WebOps.metac > 0)
                checkBox4.Checked = true;
            if (WebOps.mobyg > 0)
                checkBox5.Checked = true;

            //Populate Global Settings checkboxes
            if (showSplash > 0)
                checkBox10.Checked = true;
            if (showLoading > 0)
                checkBox2.Checked = true;
            if (requireLogin > 0)
                checkBox11.Checked = true;
            if (scanOnStartup > 0)
                checkBox12.Checked = true;
            if (enforceExt > 0)
                checkBox1.Checked = true;
            if (viewEsrb == 1)
                checkBox13.Checked = true;
            if (payPerPlay > 0)
                checkBox14.Checked = true;

            //Populate payPerPlay fields
            textBox29.Text = coins.ToString();
            textBox30.Text = playtime.ToString();

            foreach (User u in Database.UserList)
                listBox4.Items.Add(u.Username);

            //Refresh the global favorites list
            RefreshGlobalFavs();

            //Populate user license info
            label35.Text = "Licensed to: " + Program._userLicenseName;
            label34.Text = "License Status: Full Version";
            label37.Text = "License Key: " + Program._userLicenseKey;
        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox1.SelectedItem.ToString();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(curItem))
                {
                    curConsole = c;
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
        private void button4_Click(object sender, EventArgs e)
        {
            curConsole.Name = textBox9.Text;
            curConsole.EmuPath = textBox1.Text;
            curConsole.RomExt = textBox4.Text;
            curConsole.LaunchParam = textBox5.Text;
            curConsole.ReleaseDate = textBox22.Text;
            curConsole.ConsoleInfo = textBox20.Text;
            FileOps.saveDatabase(Program._databasePath);
            MainWindow.RefreshConsoleList();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox2.SelectedItem.ToString();
            listBox3.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(curItem))
                {
                    curConsole2 = c;
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
                foreach (Game g in curConsole2.GameList)
                {
                    if (g.Title.Equals(listBox3.SelectedItem.ToString()))
                    {
                        curGame = g;
                    }
                }
            }
            else
            {
                refreshGameInfo(null);
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox3.SelectedItem.ToString();
            foreach (Game g in curConsole2.GameList)
            {
                if (g.Title.Equals(curItem))
                {
                    curGame = g;
                }
            }
            refreshGameInfo(curGame);
            refreshEsrbIcon(curGame);
        }

        private void button5_Click(object sender, EventArgs e)  //Close Button
        {
            MainWindow._settingsWindowActive = false;
            Close();
        }

        /// <summary>
        /// Rescrape game info button.
        /// Rescrapes info the the specified game from the web
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            //Require that a user select a valid game to rescrape
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }

            //Scrape info and populate local fields
            WebOps.scrapeInfo(curGame);
            textBox2.Text = curGame.Title;
            textBox13.Text = curGame.Console;
            textBox12.Text = curGame.ReleaseDate;
            textBox15.Text = curGame.CriticScore;
            textBox11.Text = curGame.Publisher;
            textBox10.Text = curGame.Developer;
            textBox6.Text = curGame.Esrb;
            textBox17.Text = curGame.Players;
            textBox19.Text = curGame.EsrbDescriptor;
            textBox18.Text = curGame.Description;
            refreshEsrbIcon(curGame);
        }

        /// <summary>
        /// Save database button
        /// Save all active info to the text databse
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            saveGameInfo();
        }

        /// <summary>
        /// Delete console button
        /// Deletes a consle and all associated games from the database
        /// </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            //Ensure that at least one console exists
            if (Database.ConsoleList.Count < 2)
            {
                MessageBox.Show("Cannot have an empty console list");
                return;
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            Database.ConsoleList.Remove(curConsole);
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
        private void button7_Click(object sender, EventArgs e)
        {
            //Clear all text boxes initially 
            textBox1.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox20.Text = null;
            textBox21.Text = null;
            textBox22.Text = null;

            //Create a new console and add it to the datbase
            Console c = new Console();
            c.Name = "New Console";
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
        private void button9_Click(object sender, EventArgs e)
        {
            foreach (Game g in curConsole.GameList)
            {
                if (!WebOps.scrapeInfo(g))
                    return;
            }
        }

        /// <summary>
        /// Close and save button
        /// </summary>
        private void button13_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            FileOps.savePreferences(Program._prefPath);
            this.Close();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button16_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button14_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }


        /// <summary>
        /// Refresh user info under the User tab every time a new user is selected
        /// </summary>
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update the current user text         
            if (curUser != null)
                label38.Text = "Current User: " + curUser.Username;

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
                    bool editEnabled = u.Username.Equals(curUser.Username);
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
        private void button10_Click(object sender, EventArgs e)
        {

            foreach (User us in Database.UserList)
            {
                if (curUser.Username.Equals(us.Username))
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(curUser);
                    break;
                }
            }

            UnicadeAccount uc = new UnicadeAccount(1);
            uc.ShowDialog();

            //Update the current labels and save the user info to the preferences file
            label38.Text = "Current User: " + curUser.Username;
            FileOps.savePreferences(Program._prefPath);

            //Refresh the listbox contents
            listBox4.Items.Clear();
            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
        }

        /// <summary>
        /// Save button
        /// </summary>
        private void button11_Click(object sender, EventArgs e)
        {
            FileOps.savePreferences(Program._prefPath);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Ensure that there is always at least one user present in the database
            if (Database.UserList.Count <= 1)
            {
                MessageBox.Show("Must at least have one user");
                return;
            }

            //Remove the user and refresh the database
            Database.UserList.Remove(curUser);
            listBox4.Items.Clear();
            curUser = null;
            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
        }

        /// <summary>
        /// Save button (Global Settings tab)
        /// Save the current global settings to the preferences file
        /// </summary>
        private void button17_Click(object sender, EventArgs e)
        {
            //Verify that a user is currently logged in
            if (!curUser.Username.Equals(listBox4.SelectedItem.ToString()))
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
                    curUser.Username = textBox23.Text;
                    curUser.Pass = textBox24.Text;
                    curUser.UserInfo = textBox26.Text;
                }

                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults") || textBox6.TextLength < 1)
                {
                    if (comboBox2.SelectedItem != null)
                        curUser.AllowedEsrb = comboBox2.SelectedItem.ToString();
                }
                else
                    MessageBox.Show("Invalid ESRB Rating");
            }
            listBox4.Items.Clear();

            foreach (User us in Database.UserList)
                listBox4.Items.Add(us.Username);
        }

        /// <summary>
        /// Save game info button
        /// </summary>
        private void button18_Click(object sender, EventArgs e)
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
            saveGameInfo();
        }

        /// <summary>
        /// Save info button (Consoles tab)
        /// Save all console data to the preferences text file
        /// </summary>
        private void button19_Click(object sender, EventArgs e)
        {
            //Invalid input check
            if (textBox9.Text.Contains("|") || textBox1.Text.Contains("|") || textBox3.Text.Contains("|") || textBox4.Text.Contains("|") || textBox5.Text.Contains("|") || textBox22.Text.Contains("|") || textBox20.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (isAllDigits(textBox12.Text))
                {
                    if (textBox12.TextLength < 5)
                        curConsole.ReleaseDate = textBox22.Text;
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
                    curConsole.Name = textBox9.Text;
                    curConsole.EmuPath = textBox1.Text;
                    curConsole.RomExt = textBox4.Text;
                    curConsole.LaunchParam = textBox5.Text;
                    curConsole.ConsoleInfo = textBox20.Text;
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
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                enforceExt = 1;
            else
                enforceExt = 0;
        }

        /// <summary>
        /// Refresh global favorites button
        /// </summary>
        private void button20_Click(object sender, EventArgs e)
        {
            RefreshGlobalFavs();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //Verify that a console/game is currently selected
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            //Toggle favorite checkbox
            if (checkBox3.Checked)
                curGame.Favorite = 1;
            else
                curGame.Favorite = 0;
        }

        /// <summary>
        /// Delete user favorite
        /// </summary>
        private void button21_Click(object sender, EventArgs e)
        {
            //Verify that a user is currenly logged in
            if (!curUser.Username.Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            curUser.Favorites.RemoveAt(listBox5.SelectedIndex);
            listBox5.Items.Clear();
            foreach (Game g in curUser.Favorites)
            {
                listBox5.Items.Add(g.Title + " - " + g.Console);
            }
        }

        /// <summary>
        /// Expand game media image #1
        /// </summary>
        private void pictureBox1_Click(object sender, EventArgs e)
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
        private void pictureBox2_Click(object sender, EventArgs e)
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
        private void pictureBox3_Click(object sender, EventArgs e)
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
        /// Global rescan button
        /// </summary>
        private void button25_Click(object sender, EventArgs e)
        {
            if (FileOps.scan(Program._romPath))
                MessageBox.Show("Global Rescan Successful");
        }

        /// <summary>
        /// Rescan console button
        /// Rescans all ROM files for the current console
        /// </summary>
        private void button24_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Clear console button
        /// </summary>
        private void button26_Click(object sender, EventArgs e)
        {
            foreach (Console c in Database.ConsoleList)
            {
                c.GameList.Clear();
            }
            MessageBox.Show("Game library successfully cleared");
        }

        /// <summary>
        /// Clear console button
        /// </summary>
        private void button27_Click(object sender, EventArgs e)
        {
            //Verify that a console is selected
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }

            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(listBox1.SelectedItem.ToString()))
                {
                    c.GameList.Clear();
                    MessageBox.Show(c.Name + " Library cleared");
                    break;
                }
            }
        }

        /// <summary>
        /// Rescrape console button
        /// Rescrape metadata for all games within teh current console
        /// </summary>
        private void button28_Click(object sender, EventArgs e)
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
            foreach (Game g1 in curConsole2.GameList)
            {
                WebOps.scrapeInfo(g1);
            }
            MessageBox.Show("Operation Successful");
        }

        /// <summary>
        /// Connect button (UniCade cloud tab)
        /// Connect to UniCade Cloud with the current user
        /// </summary>
        private void button29_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = SQLclient.connectSQL();
        }

        /// <summary>
        /// Uplod game button
        /// Upload the currently selected game to UniCade cloud
        /// </summary>
        private void button33_Click(object sender, EventArgs e)
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
            SQLclient.uploadGame(curGame);
            MessageBox.Show("Game Uploaded");
        }

        /// <summary>
        /// Create new user button (UniCade Cloud tab)
        /// Create a new SQL UniCade Cloud user
        /// </summary>
        private void button34_Click(object sender, EventArgs e)
        {
            UnicadeAccount ua = new UnicadeAccount(0);
            ua.ShowDialog();
        }

        /// <summary>
        /// Login button (UniCade Cloud tab)
        /// Login a UniCade Cloud SQL user
        /// </summary>
        private void button29_Click_1(object sender, EventArgs e)
        {
            Login l = new Login(0);
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
        private void button39_Click(object sender, EventArgs e)
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

        private void button36_Click(object sender, EventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
        }

        /// <summary>
        /// Delete user button
        /// Delete the SQL user and update the interface
        /// </summary>
        private void button35_Click(object sender, EventArgs e)
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
        private void button38_Click(object sender, EventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SQLclient.uploadAllGames();
            MessageBox.Show("Library successfully uploaded");
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button37_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        /// <summary>
        /// Delete all games from the current user's UniCade Cloud account
        /// </summary>
        private void button41_Click(object sender, EventArgs e)
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
        private void button40_Click(object sender, EventArgs e)
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
        /// Download game button
        /// Download metadata for the selected game from UniCade Cloud
        /// </summary>
        private void button44_Click(object sender, EventArgs e)
        {
            //Check if a UniCade Cloud user is currently active
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            //Invalid input checks
            if (listBox3.Items.Count < 1)
            {
                MessageBox.Show("No games to download");
                return;
            }

            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }

            if (curGame == null)
            {
                MessageBox.Show("Must select a game");
                return;
            }
            Game game = null;
            game = SQLclient.getSingleGame(curGame.Console, curGame.Title);
            if (game != null)
            {
                for (int i = 0; i < curConsole2.GameList.Count; i++)
                {
                    Game g = (Game)curConsole2.GameList[i];
                    if (game.FileName.Equals(g.FileName))
                    {
                        curConsole2.GameList[i] = game;
                        refreshGameInfo(game);
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
        private void button42_Click(object sender, EventArgs e)
        {
            //Invalid input checks
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (listBox3.Items.Count < 1)
            {
                MessageBox.Show("No games to upload");
                return;
            }

            //Upload all games if all initial checks are passed
            foreach (Game g in curConsole2.GameList)
            {
                SQLclient.uploadGame(g);
                MessageBox.Show("Console Uploaded");
            }
        }

        /// <summary>
        /// Download console info button
        /// Downloads all game metadata for the current console from the current user's Unicade Cloud account
        /// </summary>
        private void button43_Click(object sender, EventArgs e)
        {
            //Invalid input checks
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            if (listBox3.Items.Count < 1)
            {
                MessageBox.Show("No games to delete");
                return;
            }
            if (curConsole2 == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            for (int i = 0; i < curConsole2.GameList.Count; i++)
            {
                Game game1 = (Game)curConsole2.GameList[i];
                Game game2 = null;
                game2 = SQLclient.getSingleGame(game1.Console, game1.Title);
                if (game2 != null)
                {
                    if (game2.FileName.Length > 3)
                        curConsole2.GameList[i] = game2;
                }
            }

            //Refresh the current game info
            MessageBox.Show("Download successful");
            refreshGameInfo(curGame);
        }

        public void refreshGameInfo(Game game)
        {
            if (game == null)
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
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_BoxFront.png"))
                pictureBox1.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_BoxFront.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_BoxBack.png"))
                pictureBox2.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_BoxBack.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_Screenshot.png"))
                pictureBox3.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + game.Title + "_Screenshot.png");
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            MainWindow.ReHookKeys();
        }

        /// <summary>
        /// Enter license button
        /// </summary>
        private void button30_Click_1(object sender, EventArgs e)
        {
            //Create a new license entry info and validate the key
            LicenseEntry le = new LicenseEntry();
            le.Owner = this;
            le.ShowDialog();
            label35.Text = "Licensed to: " + Program._userLicenseName;
            label37.Text = "License Key: " + Program._userLicenseKey;

            //Set the license text depending on if the key is valid
            if (Program._validLicense)
                label34.Text = "License Status: Full Version";
            else
                label34.Text = "License Status: INVALID";
        }

        /// <summary>
        /// Connect SQL button
        /// </summary>
        private void button31_Click_1(object sender, EventArgs e)
        {
            SQLclient.connectSQL();
        }

        /// <summary>
        /// Login local user button
        /// </summary>
        private void button31_Click_2(object sender, EventArgs e)
        {
            foreach (User us in Database.UserList)
            {
                if (curUser.Username.Equals(us.Username))
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(curUser);
                    break;
                }
            }
            //Display the login dialog
            Login login = new Login(1);
            login.ShowDialog();
            if (curUser != null)
            {
                //If the user is logged in sucuesfully, save the current user and preferences file
                label38.Text = "Current User: " + curUser.Username;
                FileOps.savePreferences(Program._prefPath);
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            label38.Text = "Current User: " + curUser.Username;
        }

        #region WebOps Tab

        /// <summary>
        /// Toggle Metacritic checkbox
        /// </summary>
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                WebOps.metac = 1;
            else
                WebOps.metac = 0;
        }

        /// <summary>
        /// Toggle Mobygames checkbox
        /// </summary>
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                WebOps.mobyg = 1;
            else
                WebOps.mobyg = 0;
        }

        /// <summary>
        /// Toggle release date checkbox
        /// </summary>
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
                WebOps.releaseDate = 1;
            else
                WebOps.releaseDate = 0;
        }

        /// <summary>
        /// Toggle critic score checkbox
        /// </summary>
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
                WebOps.critic = 1;
            else
                WebOps.critic = 0;
        }

        /// <summary>
        /// Toggle Publisher checkbox
        /// </summary>
        private void checkBox15_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox15.Checked)
                WebOps.publisher = 1;
            else
                WebOps.publisher = 0;
        }

        /// <summary>
        /// Toggle developer checkbox
        /// </summary>
        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked)
                WebOps.developer = 1;
            else
                WebOps.developer = 0;
        }

        /// <summary>
        /// Toggle ESRB Rating checkbox
        /// </summary>
        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
                WebOps.esrb = 1;
            else
                WebOps.esrb = 0;
        }

        /// <summary>
        /// Toggle ESRB Descriptor checkbox
        /// </summary>
        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
                WebOps.esrbDescriptor = 1;
            else
                WebOps.esrbDescriptor = 0;
        }

        /// <summary>
        /// Toggle players checkbox
        /// </summary>
        private void checkBox20_CheckedChanged(object sender, EventArgs e) //Players Checkbox
        {
            if (checkBox20.Checked)
                WebOps.players = 1;
            else
                WebOps.players = 0;
        }

        /// <summary>
        /// Toggle description checkbox
        /// </summary>
        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked)
                WebOps.description = 1;
            else
                WebOps.description = 0;
        }

        /// <summary>
        /// Toggle boxfront checkbox
        /// </summary>
        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked)
                WebOps.boxFront = 1;
            else
                WebOps.boxFront = 0;
        }

        /// <summary>
        /// Toggle box back checkbox
        /// </summary>
        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked)
                WebOps.boxBack = 1;
            else
                WebOps.boxBack = 0;
        }

        /// <summary>
        /// Toggle screenshot textbox
        /// </summary>
        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked)
                WebOps.screenshot = 1;
            else
                WebOps.screenshot = 0;
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button23_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        #endregion

        #region Global Settings Tab

        /// <summary>
        /// Save Global Settings button
        /// </summary>
        private void button15_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Contains("|") || textBox25.Text.Contains("|") || textBox32.Text.Contains("|") || textBox33.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (comboBox1.SelectedItem.ToString().Contains("Everyone") || comboBox1.SelectedItem.ToString().Contains("Teen") || comboBox1.SelectedItem.ToString().Contains("Mature") || comboBox1.SelectedItem.ToString().Contains("Adults") || textBox6.TextLength < 1)
                    restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
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

                int n = 0;
                Int32.TryParse(textBox7.Text, out n);
                if (n > 0)
                    passProtect = Int32.Parse(textBox7.Text);
                Int32.TryParse(textBox29.Text, out n);
                if (n > 0)
                    coins = Int32.Parse(textBox29.Text);
                if (comboBox1.SelectedItem != null)
                    restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());

                //Save all active preferences to the local preferences file
                FileOps.savePreferences(Program._prefPath);
            }
        }

        /// <summary>
        /// Toggle viewEsrb checkbox
        /// </summary>
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                viewEsrb = 1;
            else
                viewEsrb = 0;
        }

        /// <summary>
        /// Toggle splash screen checkbox
        /// </summary>
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
                showSplash = 1;
            else
                showSplash = 0;
        }

        /// <summary>
        /// Toggle show loading screen checkbox
        /// </summary>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                showLoading = 1;
            else
                showLoading = 0;
        }

        /// <summary>
        /// Toggle require login checkbox
        /// </summary>
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
                requireLogin = 1;
            else
                requireLogin = 0;
        }

        /// <summary>
        /// Toggle scan on startup checkbox
        /// </summary>
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
                scanOnStartup = 1;
            else
                scanOnStartup = 0;
        }

        /// <summary>
        /// Toggle view ESRB checkbox
        /// </summary>
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
                viewEsrb = 1;
            else
                viewEsrb = 0;
        }

        /// <summary>
        /// Toggle payPerPlay checkbox
        /// </summary>
        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
                payPerPlay = 1;
            else
                payPerPlay = 0;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Verify that a string contains only numeric chars
        /// </summary>
        bool isAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public static int calcEsrb(String esrb)
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
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        private void saveGameInfo()
        {
            //Invalid input checks
            if (listBox3.Items.Count < 1)
                return;
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    curGame.ReleaseDate = textBox12.Text;
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
            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    curGame.CriticScore = textBox15.Text;
                else
                    MessageBox.Show("Critic Score Invalid");
            }
            else
                MessageBox.Show("Critic Score must be only digits");
            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 2)
                    curGame.Players = textBox17.Text;
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
                    curGame.Esrb = textBox6.Text;
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((textBox10.Text.Length > 20) || (textBox11.Text.Length > 20) || (textBox18.Text.Length > 20) || (textBox19.Text.Length > 20))
                    MessageBox.Show("Invalid Length");
                else
                {
                    curGame.Publisher = textBox11.Text;
                    curGame.Developer = textBox10.Text;
                    curGame.Description = textBox18.Text;
                    curGame.EsrbDescriptor = textBox19.Text;
                }
            }

            //If all input fields are valid, save the database
            FileOps.saveDatabase(Program._databasePath);
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void refreshEsrbIcon(Game g)
        {
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
        }

        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
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
        }

        #endregion

        #region Extra Methods

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }


        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}

