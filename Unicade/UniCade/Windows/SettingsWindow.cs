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
            //Populate console lists
            foreach (Console c in Database.ConsoleList)
            {
                listBox1.Items.Add(c.Name);
                listBox2.Items.Add(c.Name);
            }
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

            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Load(Directory.GetCurrentDirectory()+ @"\Media\Backgrounds\UniCade Logo.png");
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Load(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png");
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Load(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png");
            if (restrictESRB == 0)
            {
                comboBox1.Text = "None";
            }
            else if(restrictESRB == 1)
                {
                comboBox1.Text = "Everyone";
            }
            else if (restrictESRB == 2)
            {
                comboBox1.Text = "Everyone 10+";
            }
            else if (restrictESRB == 3)
            {
                comboBox1.Text = "Teen";
            }
            else if (restrictESRB == 4)
            {
                comboBox1.Text = "Mature";
            }
            else if (restrictESRB == 5)
            {
                comboBox1.Text = "Adults Only (AO)";
            }


            textBox23.Enabled = false;  //Disable editing user info unless logged in
            textBox24.Enabled = false;
            textBox26.Enabled = false;
            textBox27.Enabled = false;
            textBox28.Enabled = false;
            textBox25.Enabled = false;
            textBox31.Enabled = false;
            comboBox2.Enabled = false;
            listBox5.Enabled = false;


            richTextBox1.Text = TextFiles.features + "\n\n\n\n\n\n" + TextFiles.instructions;

            if (viewEsrb > 0)
            {
                checkBox6.Checked = true;
            }
            textBox7.Text = passProtect.ToString();
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;



            textBox31.Text = Program._databasePath;
            textBox25.Text = Program._emuPath;
            textBox32.Text = Program._mediaPath;
            textBox33.Text = Program._romPath;
            if (WebOps.releaseDate > 0)
            {
                checkBox8.Checked = true;
            }
            if (WebOps.critic > 0)
            {
                checkBox9.Checked = true;
            }
            if (WebOps.publisher > 0)
            {
                checkBox15.Checked = true;
            }
            if (WebOps.developer > 0)
            {
                checkBox17.Checked = true;
            }
            if (WebOps.esrb > 0)
            {
                checkBox18.Checked = true;
            }
            if (WebOps.esrbDescriptor > 0)
            {
                checkBox19.Checked = true;
            }
            if (WebOps.players > 0)
            {
                checkBox20.Checked = true;
            }
            if (WebOps.description > 0)
            {
                checkBox21.Checked = true;
            }
            if (WebOps.boxFront > 0)
            {
                checkBox22.Checked = true;
            }
            if (WebOps.boxBack > 0)
            {
                checkBox23.Checked = true;
            }
            if (WebOps.screenshot > 0)
            {
                checkBox24.Checked = true;
            }
            if (WebOps.metac > 0)
            {
                checkBox4.Checked = true;
            }
            if (WebOps.mobyg > 0)
            {
                checkBox5.Checked = true;
            }


            if (showSplash > 0)
            {
                checkBox10.Checked = true;
            }
            if (showLoading > 0)
            {
                checkBox2.Checked = true;
            }
            if (requireLogin > 0)
            {
                checkBox11.Checked = true;
            }
            if (scanOnStartup > 0)
            {
                checkBox12.Checked = true;
            }
            if (enforceExt > 0)
            {
                checkBox1.Checked = true;
            }
            if (viewEsrb == 1)
            {
                checkBox13.Checked = true;
            }
            if (payPerPlay > 0)
            {
                checkBox14.Checked = true;
            }

            textBox29.Text = coins.ToString();
            textBox30.Text = playtime.ToString();

            foreach (User u in Database.UserList)
            {
                listBox4.Items.Add(u.Username);
            }

            refreshGlobalFavs();


            //this.Activate();
            //this.Focus();
            //this.TopMost = true;

            //Pupulate License info
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


        private void button4_Click(object sender, EventArgs e)   //Save current console info to database
        {
            curConsole.Name =textBox9.Text;
            curConsole.EmuPath = textBox1.Text;
            curConsole.RomExt = textBox4.Text;
            curConsole.LaunchParam = textBox5.Text;
            curConsole.ReleaseDate = textBox22.Text;
            curConsole.ConsoleInfo=  textBox20.Text;
            FileOps.saveDatabase(Program._databasePath);
            MainWindow.RefreshConsoleList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
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
            //Cursor.Hide();
            //this.Close();
            MainWindow._settingsWindowActive = false;
            Close();

        }

        private void button1_Click(object sender, EventArgs e)   //Rescrape Game Info Button
        {

            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            Game g = curGame;
            WebOps.scrapeInfo(g);
            textBox2.Text = g.Title;
            textBox13.Text = g.Console;
            textBox12.Text = g.ReleaseDate;
            textBox15.Text = g.CriticScore;
            textBox11.Text = g.Publisher;
            textBox10.Text = g.Developer;
            textBox6.Text = g.Esrb;
            textBox17.Text = g.Players;
            textBox19.Text = g.EsrbDescriptor;
            textBox18.Text = g.Description;
            refreshEsrbIcon(g);
        }

        private void button6_Click(object sender, EventArgs e)  //Save to Database Button
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            saveGameInfo();

        }

        private void saveGameInfo()
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }

            if (listBox3.Items.Count<1)
            {
                
                return;
            }

            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                {
                    curGame.ReleaseDate = textBox12.Text;
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

            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                {
                    curGame.CriticScore = textBox15.Text;
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

            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 2)
                {
                    curGame.Players = textBox17.Text;
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



            if (textBox10.Text.Contains("|") || textBox11.Text.Contains("|") || textBox6.Text.Contains("|") || textBox18.Text.Contains("|") || textBox19.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults")||textBox6.TextLength<1)
                {
                    curGame.Esrb = textBox6.Text;
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }

                if ((textBox10.Text.Length > 20) || (textBox11.Text.Length > 20) || (textBox18.Text.Length > 20) || (textBox19.Text.Length > 20))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    curGame.Publisher = textBox11.Text;
                    curGame.Developer = textBox10.Text;

                    curGame.Description = textBox18.Text;
                    curGame.EsrbDescriptor = textBox19.Text;
                }
            }

            FileOps.saveDatabase(Program._databasePath);
        }





        private void button8_Click(object sender, EventArgs e)  //Delete console
        {
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

        private void button7_Click(object sender, EventArgs e)  //Add new emulator/Console
        {
            textBox1.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox20.Text = null;
            textBox21.Text = null;
            textBox22.Text = null;
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

        private void button9_Click(object sender, EventArgs e)  //Force metadata rescrape (All games from console)
        {
            foreach (Game g in curConsole.GameList)
            {
                if (!WebOps.scrapeInfo(g))
                {
                    return;
                }
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow._settingsWindowActive = false;
            FileOps.savePreferences(Program._prefPath);

            this.Close();
        }

        //Extra Methods

        public static int calcEsrb(String e)
        {
            int EsrbNum = 0;
            if (e.Equals("Everyone"))
            {
                EsrbNum = 1;
            }
            else if (e.Equals("Everyone 10+"))
            {
                EsrbNum = 2;
            }
            else if (e.Equals("Teen"))
            {
                EsrbNum = 3;
            }
            else if (e.Equals("Mature"))
            {
                EsrbNum = 4;
            }
            else if (e.Equals("Adults Only (AO)"))
            {
                EsrbNum = 5;
            }
            else if (e.Equals("None"))
            {
                EsrbNum = 0;
            }
            else
            {
                EsrbNum = 0;
            }
            return EsrbNum;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow._settingsWindowActive = false;
            this.Close();

        }

        private void button15_Click(object sender, EventArgs e)  //Save Global Settings 
        {
            if (comboBox1.SelectedItem.ToString().Contains("|") || textBox25.Text.Contains("|") || textBox32.Text.Contains("|") || textBox33.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if (comboBox1.SelectedItem.ToString().Contains("Everyone") || comboBox1.SelectedItem.ToString().Contains("Teen") || comboBox1.SelectedItem.ToString().Contains("Mature") || comboBox1.SelectedItem.ToString().Contains("Adults") || textBox6.TextLength < 1)
                {
                    restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }
                if ((textBox25.Text.Length > 150) || (textBox32.Text.Length > 150) || (textBox33.Text.Length > 150))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    Program._emuPath = textBox25.Text;
                    Program._mediaPath = textBox32.Text;
                    Program._romPath = textBox33.Text;
                }




                int n = 0;
                Int32.TryParse(textBox7.Text, out n);
                if (n > 0)
                {
                    passProtect = Int32.Parse(textBox7.Text);
                }
                Int32.TryParse(textBox29.Text, out n);
                if (n > 0)
                {
                    coins = Int32.Parse(textBox29.Text);
                }
                Int32.TryParse(textBox30.Text, out n);
                if (n > 0)
                {
                    playtime = Int32.Parse(textBox30.Text);
                }
                if (comboBox1.SelectedItem != null)
                {
                    restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
                }

                if (SettingsWindow.payPerPlay > 0)
                {

                    if (SettingsWindow.playtime > 0)
                    {
                        // Program.gui.displayPayNotification("PayPerPlay . Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + coins);
                    }
                    else if (SettingsWindow.coins > 0)
                    {
                        //Program.gui.displayPayNotification("PayPerPlay. Coins Per Launch: " + SettingsWindow.coins + "Current: " + Program.coins);
                    }

                }
                FileOps.savePreferences(Program._prefPath);
            }
        }





        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                viewEsrb = 1;
            }
            else
            {
                viewEsrb = 0;
            }

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                showSplash = 1;
            }
            else
            {
                showSplash = 0;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                showLoading = 1;
            }
            else
            {
                showLoading = 0;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                requireLogin = 1;
            }
            else
            {
                requireLogin = 0;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
            {
                scanOnStartup = 1;
            }
            else
            {
                scanOnStartup = 0;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
            {
                viewEsrb = 1;
            }
            else
            {
                viewEsrb = 0;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
            {
                payPerPlay = 1;

            }
            else
            {
                payPerPlay = 0;
               // Program.gui.closePayNotification();
            }

            if (SettingsWindow.payPerPlay > 0)
            {
                //Program.gui.closePayNotification();
                if (SettingsWindow.playtime > 0)
                {
                    //Program.gui.displayPayNotification("PayPerPlay . Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + coins);
                }
                else if (SettingsWindow.coins > 0)
                {
                   // Program.gui.displayPayNotification("PayPerPlay. Coins Per Launch: " + SettingsWindow.coins + "Current: " + Program.coins);
                }

            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            int n = 0;
            Int32.TryParse(textBox30.Text, out n);
            if (n > 0)
            {
                playtime = Int32.Parse(textBox30.Text);
            }

            //Program.gui.closePayNotification();
            if (SettingsWindow.payPerPlay > 0)
            {

                if (SettingsWindow.playtime > 0)
                {
                    //Program.gui.displayPayNotification("PayPerPlay . Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + coins);
                }
                else if (SettingsWindow.coins > 0)
                {
                    //Program.gui.displayPayNotification("PayPerPlay. Coins Per Launch: " + SettingsWindow.coins + "Current: " + Program.coins);
                }

            }
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            int n = 0;
            Int32.TryParse(textBox29.Text, out n);
            if (n > 0)
            {
                coins = Int32.Parse(textBox29.Text);
            }
            //Program.gui.closePayNotification();
            if (SettingsWindow.payPerPlay > 0)
            {

                if (SettingsWindow.playtime > 0)
                {
                    //Program.gui.displayPayNotification("PayPerPlay . Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + coins);
                }
                else if (SettingsWindow.coins > 0)
                {
                   // Program.gui.displayPayNotification("PayPerPlay. Coins Per Launch: " + SettingsWindow.coins + "Current: " + Program.coins);
                }

            }

        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            Program._databasePath = textBox31.Text;
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            Program._emuPath = textBox25.Text;
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            Program._mediaPath = textBox32.Text;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)  //User listbox changed
        {

            listBox5.Items.Clear();

            if (curUser != null)
            {
                label38.Text = "Current User: " + curUser.Username;

                

            }
            foreach (User u in Database.UserList)
            {
                if (u.Username.Equals(listBox4.SelectedItem.ToString()))
                {

                    if (u.Favorites.Count > 0)
                    {
                        foreach (Game g in u.Favorites)
                        {
                            listBox5.Items.Add(g.Title + " - " + g.Console);
                        }
                    }

                    textBox23.Text = u.Username;
                    textBox24.Text = u.Email;
                    textBox26.Text = u.UserInfo;
                    textBox27.Text = u.LoginCount.ToString();
                    textBox28.Text = u.TotalLaunchCount.ToString();
                    comboBox2.Text = u.AllowedEsrb;
                    if (u.Username.Equals(curUser.Username))
                    {
                        textBox23.Enabled = true;
                        textBox24.Enabled = true;
                        textBox26.Enabled = true;
                        textBox27.Enabled = true;
                        textBox28.Enabled = true;
                        comboBox2.Enabled = true;
                        listBox5.Enabled = true;

                    }
                    else
                    {
                        textBox23.Enabled = false;
                        textBox24.Enabled = false;
                        textBox26.Enabled = false;
                        textBox27.Enabled = false;
                        textBox28.Enabled = false;
                        comboBox2.Enabled = false;
                        listBox5.Enabled = false;
                    }
                }
                
            }
        }



        private void button10_Click(object sender, EventArgs e)  //Create new user
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

                label38.Text = "Current User: " + curUser.Username;
                FileOps.savePreferences(Program._prefPath);

            listBox4.Items.Clear();
            foreach (User us in Database.UserList)
            {
                listBox4.Items.Add(us.Username);
            }

        }

        private void button11_Click(object sender, EventArgs e)  //Save
        {
            /*curUser.setName(textBox23.Text);
            curUser.setEmail(textBox24.Text);
            curUser.setUserInfo(textBox26.Text);
            if (comboBox2 != null)
            {
                curUser.setAllowedEsrb(comboBox2.SelectedItem.ToString());
            }*/
            FileOps.savePreferences(Program._prefPath);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Database.UserList.Count <= 1)
            {
                MessageBox.Show("Must at least have one user");
                return;
            }
            Database.UserList.Remove(curUser);
            listBox4.Items.Clear();
            curUser = null;
            foreach (User us in Database.UserList)
            {
                listBox4.Items.Add(us.Username);
            }


        }

        private void button17_Click(object sender, EventArgs e)               //Save local user info
        {
            if (!curUser.Username.Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            
           

            if ( textBox23.Text.Contains("|") || textBox24.Text.Contains("|") || textBox26.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {
                if ((textBox23.Text.Length > 20) || (textBox24.Text.Length > 20) || (textBox26.Text.Length > 50))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
                    curUser.Username = textBox23.Text;
                    curUser.Pass = textBox24.Text;
                    curUser.UserInfo = textBox26.Text;
                }

                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults") || textBox6.TextLength < 1)
                {
                    if (comboBox2.SelectedItem != null)
                    {
                        curUser.AllowedEsrb = comboBox2.SelectedItem.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }
                
            }
            listBox4.Items.Clear();


            foreach (User us in Database.UserList)
            {
                listBox4.Items.Add(us.Username);
            }
        }

        bool isAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private void button18_Click(object sender, EventArgs e)  //Save game info
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

        private void button19_Click(object sender, EventArgs e)  //Consoles - Save Info Button
        {
            if (textBox9.Text.Contains("|") || textBox1.Text.Contains("|") || textBox3.Text.Contains("|") || textBox4.Text.Contains("|") || textBox5.Text.Contains("|") || textBox22.Text.Contains("|") || textBox20.Text.Contains("|"))
            {
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            }
            else
            {

                if (isAllDigits(textBox12.Text))
                {
                    if (textBox12.TextLength < 5)
                    {
                        curConsole.ReleaseDate = textBox22.Text;
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


                if ((textBox9.Text.Length > 20) || (textBox1.Text.Length > 100) || (textBox3.Text.Length > 100) || (textBox4.Text.Length > 30) || (textBox3.Text.Length > 40) || (textBox4.Text.Length > 300))
                {
                    MessageBox.Show("Invalid Length");
                }
                else
                {
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
            {
                listBox1.Items.Add(c.Name);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                enforceExt = 1;
            }
            else
            {
                enforceExt = 0;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)  //Refresh Global Favorites List
        {
            refreshGlobalFavs();
        }

        public void refreshGlobalFavs()
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

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console/game");
                return;
            }
            if (checkBox3.Checked)
            {
                curGame.Favorite = 1;
            }
            else
            {
                curGame.Favorite = 0;
            }

        }

        private void button21_Click(object sender, EventArgs e)  //Delete User Favorite
        {

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

        public void refreshEsrbIcon(Game g)
        {
            pictureBox4.Image = null;
            if (g.Esrb.Equals("Everyone"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() +  @"\Media\Esrb\Everyone.png");
            }
            else if (g.Esrb.Equals("Everyone (KA)"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() +  @"\Media\Esrb\Everyone.png");
            }
            else if (g.Esrb.Equals("Everyone 10+"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png");
            }
            else if (g.Esrb.Equals("Teen"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png");
            }
            else if (g.Esrb.Equals("Mature"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png");
            }
            if (g.Esrb.Equals("Adults Only (AO)"))
            {
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png");
            }
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)  //Metacritic CheckBox
        {
            if (checkBox4.Checked)
            {
                WebOps.metac = 1;
            }
            else
            {
                WebOps.metac = 0;
            }

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)  //MobyGames Checkbox
        {
            if (checkBox5.Checked)
            {
                WebOps.mobyg = 1;
            }
            else
            {
                WebOps.mobyg = 0;
            }

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)  //Release Date Checkbox
        {
            if (checkBox8.Checked)
            {
                WebOps.releaseDate = 1;
            }
            else
            {
                WebOps.releaseDate = 0;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)  // Critic Score Checkbox
        {
            if (checkBox9.Checked)
            {
                WebOps.critic = 1;
            }
            else
            {
                WebOps.critic = 0;
            }

        }

        private void checkBox15_CheckedChanged_1(object sender, EventArgs e) //Publisher Checkbox
        {
            if (checkBox15.Checked)
            {
                WebOps.publisher = 1;
            }
            else
            {
                WebOps.publisher = 0;
            }

        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)  //Developer Checkbox
        {
            if (checkBox17.Checked)
            {
                WebOps.developer = 1;
            }
            else
            {
                WebOps.developer = 0;
            }

        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)  //ESRB Checkbox
        {
            if (checkBox18.Checked)
            {
                WebOps.esrb = 1;
            }
            else
            {
                WebOps.esrb = 0;
            }

        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)  //ESRB Descriptor Checkbox
        {
            if (checkBox19.Checked)
            {
                WebOps.esrbDescriptor = 1;
            }
            else
            {
                WebOps.esrbDescriptor = 0;
            }

        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e) //Players Checkbox
        {
            if (checkBox20.Checked)
            {
                WebOps.players = 1;
            }
            else
            {
                WebOps.players = 0;
            }

        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)  //Description Checkbox
        {
            if (checkBox21.Checked)
            {
                WebOps.description = 1;
            }
            else
            {
                WebOps.description = 0;
            }

        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)  //BoxFront CheckBox
        {
            if (checkBox23.Checked)
            {
                WebOps.boxFront = 1;
            }
            else
            {
                WebOps.boxFront = 0;
            }

        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)  //BoxBack CheckBox
        {
            if (checkBox22.Checked)
            {
                WebOps.boxBack = 1;
            }
            else
            {
                WebOps.boxBack = 0;
            }

        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)  //Screenshot Checkbox
        {
            if (checkBox24.Checked)
            {
                WebOps.screenshot = 1;
            }
            else
            {
                WebOps.screenshot = 0;
            }

        }

        private void button23_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {

        }

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

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)  //Global Rescan button
        {
            if (FileOps.scan(Program._romPath))
            {
                MessageBox.Show("Global Rescan Successful");
            }
        }

        private void button24_Click(object sender, EventArgs e)  //Single console rescan button
        {
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

        private void button26_Click(object sender, EventArgs e)
        {
            foreach (Console c in Database.ConsoleList)
            {
                c.GameList.Clear();
            }
            MessageBox.Show("Game library successfully cleared");
        }

        private void button27_Click(object sender, EventArgs e)
        {
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

        private void button29_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = SQLclient.connectSQL();
        }





        

        private void button33_Click(object sender, EventArgs e)  //Upload game button
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

        private void button34_Click(object sender, EventArgs e)//Create new web user
        {
            UnicadeAccount ua = new UnicadeAccount(0);
            ua.ShowDialog();
        }

        private void button29_Click_1(object sender, EventArgs e)  //Login button
        {
            Login l = new Login(0);
            l.ShowDialog();
            if (SQLclient.sqlUser != null)
            {
                label56.Text = "Current Web User: " + SQLclient.sqlUser;
            }

        }

        private void button39_Click(object sender, EventArgs e)  //Logout button (SQL)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("User is already logged out");
                label56.Text = "Current Web User: ";
                return;
            }
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

        private void button35_Click(object sender, EventArgs e)  //Delete user button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            if (SQLclient.DeleteUser())
            {
                label56.Text = "Current Web User: ";
            }
        }

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

        private void button37_Click(object sender, EventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            this.Close();
        }

        private void button41_Click(object sender, EventArgs e)//Delete all games in sql cloud button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }
            SQLclient.Deletegames();
            MessageBox.Show("Library successfully deleted");

        }

        private void button40_Click(object sender, EventArgs e)  //Download all games function
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

            SQLclient.DownloadAllGames();
            MessageBox.Show("Library metadata sucuessfully updated");
        }

        private void button44_Click(object sender, EventArgs e)  //Download game button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("UniCade Cloud Login Required");
                return;
            }

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



            Game gam = null;
            gam = SQLclient.getSingleGame(curGame.Console, curGame.Title);
            if (gam != null)
            {

                for (int i = 0; i < curConsole2.GameList.Count; i++)
                {
                    Game g = (Game)curConsole2.GameList[i];
                    if (gam.FileName.Equals(g.FileName))
                    {
                        curConsole2.GameList[i] = gam;
                        refreshGameInfo(gam);
                        MessageBox.Show("Game Metadata Downloaded");
                        return;
                    }
                }
            }


            MessageBox.Show("Download successful");



        }

        private void button42_Click(object sender, EventArgs e)  //Upload console button
        {


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
            foreach (Game g in curConsole2.GameList)
            {
                SQLclient.uploadGame(g);
                MessageBox.Show("Console Uploaded");
            }
            System.Console.WriteLine("All " + curConsole2.Name + " Uploaded");


        }

        private void button43_Click(object sender, EventArgs e)  //Download console button
        {

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
                Game g = (Game)curConsole2.GameList[i];
                Game gam = null;
                gam = SQLclient.getSingleGame(g.Console, g.Title);
                if (gam != null)
                {
                    if (gam.FileName.Length > 3)
                    {
                        curConsole2.GameList[i] = gam;
                    }
                }

            }

            MessageBox.Show("Download successful");
            refreshGameInfo(curGame);
        }

        public void refreshGameInfo(Game g)
        {
            if (g == null)
                    {
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
                    textBox2.Text = g.Title;
                    textBox13.Text = g.Console;
                    textBox12.Text = g.ReleaseDate;
                    textBox15.Text = g.CriticScore;
                    textBox11.Text = g.Publisher;
                    textBox10.Text = g.Developer;
                    textBox6.Text = g.Esrb;
                    textBox17.Text = g.Players;
                    textBox19.Text = g.EsrbDescriptor;
                    textBox18.Text = g.Description;
                    if (g.Favorite == 1)
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_BoxFront.png"))
                    {
                        pictureBox1.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_BoxFront.png");

                    }

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_BoxBack.png"))
                    {
                        pictureBox2.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_BoxBack.png");

                    }

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_Screenshot.png"))
                    {
                        pictureBox3.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + curConsole2.Name + "\\" + g.Title + "_Screenshot.png");

                    }
         

        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainWindow._settingsWindowActive = false;
            MainWindow.ReHookKeys();
        }



        private void button30_Click_1(object sender, EventArgs e)
        {
            LicenseEntry le = new LicenseEntry();
            le.Owner = this;
            le.ShowDialog();
            label35.Text = "Licensed to: " + Program._userLicenseName;
            label37.Text = "License Key: " + Program._userLicenseKey;
            if (Program._validLicense)
            {
                label34.Text = "License Status: Full Version";
            }
            else
            {
                label34.Text = "License Status: INVALID";
            }

        }

        private void button31_Click_1(object sender, EventArgs e)
        {
            SQLclient.connectSQL();
        }

        private void button31_Click_2(object sender, EventArgs e)  //Login local user
        {

            foreach (User us in Database.UserList)
            {
                if (curUser.Username.Equals(us.Username))   //Save curuser to datalist
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(curUser);
                    break;

                }
            }


            Login l = new Login(1);
            l.ShowDialog();
            if (curUser != null)
            {
                label38.Text = "Current User: " + curUser.Username;
                FileOps.savePreferences(Program._prefPath);
            }
        }



        private void button45_Click(object sender, EventArgs e)
        {
            label38.Text = "Current User: " + curUser.Username;
        }
    }
}

