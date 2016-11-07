using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace UniCade
{
    public partial class SettingsWindow : Form
    {
        static Console curConsole2;
        static Console curConsole;
        public static Game curGame;
        public static User curUser;

        //Preference Data

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



        public SettingsWindow()
        {

            InitializeComponent();
            this.FormClosing += SettingsWindow_FormClosing;
            populate();

        }

        private void populate()
        {
            foreach (Console c in Program.dat.consoleList)
            {
                listBox1.Items.Add(c.getName());
                listBox2.Items.Add(c.getName());
            }
            comboBox1.Items.Add("Everyone");
            comboBox1.Items.Add("Everyone 10+");
            comboBox1.Items.Add("Teen");
            comboBox1.Items.Add("Mature");
            comboBox1.Items.Add("Adults Only (AO)");
            comboBox2.Items.Add("Everyone");
            comboBox2.Items.Add("Everyone 10+");
            comboBox2.Items.Add("Teen");
            comboBox2.Items.Add("Mature");
            comboBox2.Items.Add("Adults Only (AO)");
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.Load(@"C:\UniCade\Media\Backgrounds\UniCade Logo.png");
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Load(@"C:\UniCade\Media\Backgrounds\UniCade Logo.png");

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



            textBox31.Text = Program.databasePath;
            textBox25.Text = Program.emuPath;
            textBox32.Text = Program.mediaPath;
            textBox33.Text = Program.romPath;
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

            foreach (User u in Program.dat.userList)
            {
                listBox4.Items.Add(u.getUsername());
            }

            refreshGlobalFavs();


            //this.Activate();
            //this.Focus();
            //this.TopMost = true;

            //Pupulate License info
            label35.Text = "Licensed to: " + Program.userLicenseName;
            label34.Text = "License Status: Full Version";
            label37.Text = "License Key: " + Program.userLicenseKey;
            


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
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(curItem))
                {
                    curConsole = c;
                    textBox9.Text = c.getName();
                    textBox1.Text = c.getEmuPath();
                    //textBox3.Text = c.getRomPath();
                    textBox4.Text = c.getRomExt();
                    textBox5.Text = c.getLaunchParam();
                    textBox20.Text = c.getConsoleInfo();
                    textBox21.Text = c.gameCount.ToString();
                    textBox22.Text = c.getReleaseDate();

                }
            }
        }


        private void button4_Click(object sender, EventArgs e)   //Save current console info to databse
        {
            curConsole.setName(textBox9.Text);
            curConsole.setEmuPath(textBox1.Text);
            //curConsole.setRomPath(textBox3.Text);
            curConsole.setRomExt(textBox4.Text);
            curConsole.setLaunchParam(textBox5.Text);
            curConsole.setReleaseDate(textBox22.Text);
            curConsole.setConsoleInfo(textBox20.Text);
            FileOps.saveDatabase(Program.databasePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow.settingsWindowActive = false;
            this.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string curItem = listBox2.SelectedItem.ToString();
            listBox3.Items.Clear();
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(curItem))
                {
                    curConsole2 = c;
                    textBox8.Text = c.gameCount.ToString();
                    textBox3.Text = Database.totalGameCount.ToString();
                    foreach (Game g in c.getGameList())
                    {
                        listBox3.Items.Add(g.getTitle());
                    }
                }
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox3.SelectedItem.ToString();
            foreach (Game g in curConsole2.getGameList())
            {
                if (g.getTitle().Equals(curItem))
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
            saveGameInfo();
            MainWindow.settingsWindowActive = false;
            Close();

        }

        private void button1_Click(object sender, EventArgs e)   //Rescrape Game Info Button
        {
            Game g = curGame;
            WebOps.scrapeInfo(g);
            textBox2.Text = g.getTitle();
            textBox13.Text = g.getConsole();
            textBox12.Text = g.getReleaseDate();
            textBox15.Text = g.getCriticScore();
            textBox11.Text = g.getPublisher();
            textBox10.Text = g.getDeveloper();
            textBox6.Text = g.getEsrb();
            textBox17.Text = g.getPlayers();
            textBox19.Text = g.getEsrbDescriptor();
            textBox18.Text = g.getDescription();
            refreshEsrbIcon(g);
        }

        private void button6_Click(object sender, EventArgs e)  //Save to Database Button
        {
            saveGameInfo();

        }

        private void saveGameInfo()
        {

            if (isAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                {
                    curGame.setReleaseDate(textBox12.Text);
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
                    curGame.setCriticScore(textBox15.Text);
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
                    curGame.setPlayers(textBox17.Text);
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
                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults"))
                {
                    curGame.setEsrb(textBox6.Text);
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
                    curGame.setPublisher(textBox11.Text);
                    curGame.setDeveloper(textBox10.Text);

                    curGame.setDescription(textBox18.Text);
                    curGame.setEsrbDescriptors(textBox19.Text);
                }
            }

            FileOps.saveDatabase(Program.databasePath);
        }

        private void button3_Click(object sender, EventArgs e)  //Restore default console settings
        {
            FileOps.loadDefaultConsoles();
            FileOps.saveDatabase(Program.databasePath);

        }



        private void button8_Click(object sender, EventArgs e)  //Delete console
        {

            Program.dat.consoleList.Remove(curConsole);
            foreach (Console c in Program.dat.consoleList)
            {
                listBox1.Items.Add(c.getName());
                listBox2.Items.Add(c.getName());
            }

        }

        private void button7_Click(object sender, EventArgs e)  //Add new emulator/Console
        {
            textBox1.Text = null;
            //textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox20.Text = null;
            textBox21.Text = null;
            textBox22.Text = null;
            Console c = new Console();
            c.setName(textBox9.Text);
            c.setEmuPath(textBox1.Text);
            //c.setRomPath(textBox3.Text);
            c.setRomExt(textBox4.Text);
            c.setLaunchParam(textBox5.Text);
            c.setReleaseDate(textBox22.Text);
            c.setConsoleInfo(textBox20.Text);
            Program.dat.consoleList.Add(c);
            foreach (Console con in Program.dat.consoleList)
            {
                listBox1.Items.Add(con.getName());
                listBox2.Items.Add(con.getName());
            }
            FileOps.saveDatabase(Program.databasePath);
        }

        private void button9_Click(object sender, EventArgs e)  //Force metadata rescrape (All games from console)
        {
            foreach (Game g in curConsole.getGameList())
            {
                WebOps.scrapeInfo(g);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow.settingsWindowActive = false;
            FileOps.savePreferences(Program.prefPath);

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
            MainWindow.settingsWindowActive = false;
            this.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Cursor.Hide();
            MainWindow.settingsWindowActive = false;
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
                if (comboBox1.SelectedItem.ToString().Contains("Everyone") || comboBox1.SelectedItem.ToString().Contains("Teen") || comboBox1.SelectedItem.ToString().Contains("Mature") || comboBox1.SelectedItem.ToString().Contains("Adults"))
                {
                    curGame.setEsrb(textBox6.Text);
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
                    Program.emuPath = textBox25.Text;
                    Program.mediaPath = textBox32.Text;
                    Program.romPath = textBox33.Text;
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
                FileOps.savePreferences(Program.prefPath);
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
            Program.databasePath = textBox31.Text;
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            Program.emuPath = textBox25.Text;
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            Program.mediaPath = textBox32.Text;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)  //User listbox changed
        {

            listBox5.Items.Clear();

            if (curUser != null)
            {
                label38.Text = "Current User: " + curUser.getUsername();

                

            }
            foreach (User u in Program.dat.userList)
            {
                if (u.getUsername().Equals(listBox4.SelectedItem.ToString()))
                {

                    if (u.favorites.Count > 0)
                    {
                        foreach (Game g in u.favorites)
                        {
                            listBox5.Items.Add(g.getTitle() + " - " + g.getConsole());
                        }
                    }

                    textBox23.Text = u.getUsername();
                    textBox24.Text = u.getEmail();
                    textBox26.Text = u.getUserInfo();
                    textBox27.Text = u.getLoginCount().ToString();
                    textBox28.Text = u.getLaunchCount().ToString();
                    comboBox2.Text = u.getAllowedEsrb();
                    if (u.getUsername().Equals(curUser.getUsername()))
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

            foreach (User us in Program.dat.userList)
            {
                if (curUser.getUsername().Equals(us.getUsername()))
                {
                    Program.dat.userList.Remove(us);
                    Program.dat.userList.Add(curUser);
                    break;

                }
            }

                UnicadeAccount uc = new UnicadeAccount(1);
            uc.ShowDialog();

                label38.Text = "Current User: " + curUser.getUsername();
                FileOps.savePreferences(Program.prefPath);

            listBox4.Items.Clear();
            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
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
            FileOps.savePreferences(Program.prefPath);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Program.dat.userList.Count <= 1)
            {
                MessageBox.Show("Must at least have one user");
                return;
            }
            Program.dat.userList.Remove(curUser);
            listBox4.Items.Clear();
            curUser = null;
            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
            }


        }

        private void button17_Click(object sender, EventArgs e)               //Save local user info
        {
            if (!curUser.getUsername().Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            
           

            if (comboBox2.SelectedItem.ToString().Contains("|") || textBox23.Text.Contains("|") || textBox24.Text.Contains("|") || textBox26.Text.Contains("|"))
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
                    curUser.setName(textBox23.Text);
                    curUser.setpass(textBox24.Text);
                    curUser.setUserInfo(textBox26.Text);
                }

                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("L") || textBox6.Text.Contains("|"))
                {
                    if (comboBox2.SelectedItem != null)
                    {
                        curUser.setAllowedEsrb(comboBox2.SelectedItem.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ESRB Rating");
                }
                
            }
            listBox4.Items.Clear();


            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
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
                        curConsole.setReleaseDate(textBox22.Text);
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
                    curConsole.setName(textBox9.Text);
                    curConsole.setEmuPath(textBox1.Text);
                    //curConsole.setRomPath(textBox3.Text);
                    curConsole.setRomExt(textBox4.Text);
                    curConsole.setLaunchParam(textBox5.Text);
                    curConsole.setConsoleInfo(textBox20.Text);
                }
            }

            listBox1.Items.Clear();
            foreach (Console c in Program.dat.consoleList)
            {
                listBox1.Items.Add(c.getName());
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
            foreach (Console c in Program.dat.consoleList)
            {
                foreach (Game g in c.getGameList())
                {
                    if (g.getFav() > 0)
                    {
                        listBox6.Items.Add(g.getTitle() + " (" + g.getConsole() + ")");
                    }
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                curGame.setFav(1);
            }
            else
            {
                curGame.setFav(0);
            }

        }

        private void button21_Click(object sender, EventArgs e)  //Delete User Favorite
        {

            if (!curUser.getUsername().Equals(listBox4.SelectedItem.ToString()))
            {
                MessageBox.Show("Must Login First");
                return;
            }

            curUser.favorites.RemoveAt(listBox5.SelectedIndex);
            listBox5.Items.Clear();
            foreach (Game g in curUser.favorites)
            {
                listBox5.Items.Add(g.getTitle() + " - " + g.getConsole());
            }
        }

        public void refreshEsrbIcon(Game g)
        {
            pictureBox4.Image = null;
            if (g.getEsrb().Equals("Everyone"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Everyone.png");
            }
            else if (g.getEsrb().Equals("Everyone (KA)"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Everyone.png");
            }
            else if (g.getEsrb().Equals("Everyone 10+"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Everyone 10+.png");
            }
            else if (g.getEsrb().Equals("Teen"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Teen.png");
            }
            else if (g.getEsrb().Equals("Mature"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Mature.png");
            }
            if (g.getEsrb().Equals("Adults Only (AO)"))
            {
                pictureBox4.Load(@"C:\UniCade\Media\Esrb\Adults Only (AO).png");
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
            MainWindow.settingsWindowActive = false;
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
            FileOps.scan(Program.romPath);
            MessageBox.Show("Global Rescan Successfull");
        }

        private void button24_Click(object sender, EventArgs e)  //Single console rescan button
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(listBox1.SelectedItem.ToString()))
                {
                    FileOps.scanDirectory(c.getRomPath(), Program.romPath);
                    MessageBox.Show(c.getName() + " Successfully Scanned");
                    break;
                }

            }

        }

        private void button26_Click(object sender, EventArgs e)
        {
            foreach (Console c in Program.dat.consoleList)
            {
                c.getGameList().Clear();
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
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(listBox1.SelectedItem.ToString()))
                {
                    c.getGameList().Clear();
                    MessageBox.Show(c.getName() + " Library cleared");
                    break;
                }

            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This may take a while... Please wait for a completed nofication.");
            foreach (Game g1 in curConsole2.getGameList())
            {
                WebOps.scrapeInfo(g1);
            }
            MessageBox.Show("Operation Successful");

        }

        private void button29_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = SQLclient.connectSQL();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            richTextBox2.Text = null;
            if (textBox14.Text.Length < 2)
            {
                MessageBox.Show("Command cannot be empty");
                return;
            }
            richTextBox2.Text = SQLclient.processSQLcommand(textBox14.Text);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            Game g = SQLclient.getSingleGame(textBox34.Text, textBox35.Text);
            if (g != null)
            {
                richTextBox2.Text = g.getFileName();
                foreach (Console c in Program.dat.consoleList)
                {
                    if (g.getConsole().Equals(c.getName()))
                    {
                        c.getGameList().Add(g);
                        c.gameCount++;
                        MessageBox.Show("Game Sucuesfully Added");
                    }
                }
            }
        }

        private void button32_Click(object sender, EventArgs e)  //Delete game button
        {
            if (curGame != null)
            {
                curConsole.getGameList().Remove(curGame);
                MessageBox.Show("Game Removed");
            }
        }

        private void button33_Click(object sender, EventArgs e)  //Upload game button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("SQL login required");
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
                MessageBox.Show("Login Required");
                return;
            }
        }

        private void button35_Click(object sender, EventArgs e)  //Delete user button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }

            if (SQLclient.deleteUser())
            {
                label56.Text = "Current Web User: ";
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            SQLclient.uploadAllGames();
            MessageBox.Show("Library successfully uploaded");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            MainWindow.settingsWindowActive = false;
            this.Close();
        }

        private void button41_Click(object sender, EventArgs e)//Delete all games in sql cloud button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            SQLclient.deletegames();
            MessageBox.Show("Library successfully deleted");

        }

        private void button40_Click(object sender, EventArgs e)  //Download all games function
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }

            SQLclient.downloadAllGames();
            MessageBox.Show("Library metadata sucuessfully updated");
        }

        private void button44_Click(object sender, EventArgs e)  //Download game button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }

            if (curGame == null)
            {
                MessageBox.Show("Must select a game");
                return;
            }



            Game gam = null;
            gam = SQLclient.getSingleGame(curGame.getConsole(), curGame.getTitle());
            if (gam != null)
            {

                for (int i = 0; i < curConsole2.getGameList().Count; i++)
                {
                    Game g = (Game)curConsole2.getGameList()[i];
                    if (gam.getFileName().Equals(g.getFileName()))
                    {
                        curConsole2.getGameList()[i] = gam;
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

            if (curConsole2 == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }
            foreach (Game g in curConsole2.getGameList())
            {
                SQLclient.uploadGame(g);
                MessageBox.Show("Console Uploaded");
            }
            System.Console.WriteLine("All " + curConsole2.getName() + " Uploaded");


        }

        private void button43_Click(object sender, EventArgs e)  //Download console button
        {
            if (SQLclient.sqlUser == null)
            {
                MessageBox.Show("Login Required");
                return;
            }

            if (curConsole2 == null)
            {
                MessageBox.Show("Please select console");
                return;
            }

            for (int i = 0; i < curConsole2.getGameList().Count; i++)
            {
                Game g = (Game)curConsole2.getGameList()[i];
                Game gam = null;
                gam = SQLclient.getSingleGame(g.getConsole(), g.getTitle());
                if (gam != null)
                {
                    if (gam.getFileName().Length > 3)
                    {
                        curConsole2.getGameList()[i] = gam;
                    }
                }

            }

            MessageBox.Show("Download successful");
            refreshGameInfo(curGame);
        }

        public void refreshGameInfo(Game g)
        {
            
                    textBox2.Text = g.getTitle();
                    textBox13.Text = g.getConsole();
                    textBox12.Text = g.getReleaseDate();
                    textBox15.Text = g.getCriticScore();
                    textBox11.Text = g.getPublisher();
                    textBox10.Text = g.getDeveloper();
                    textBox6.Text = g.getEsrb();
                    textBox17.Text = g.getPlayers();
                    textBox19.Text = g.getEsrbDescriptor();
                    textBox18.Text = g.getDescription();
                    if (g.getFav() == 1)
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

                    if (File.Exists(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_BoxFront.png"))
                    {
                        pictureBox1.Load(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_BoxFront.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_BoxBack.png"))
                    {
                        pictureBox2.Load(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_BoxBack.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_Screenshot.png"))
                    {
                        pictureBox3.Load(@"C:\UniCade\Media\Games\" + curConsole2.getName() + "\\" + g.getTitle() + "_Screenshot.png");

                    }
         

        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainWindow.settingsWindowActive = false;
            MainWindow.hookKeys();
        }



        private void button30_Click_1(object sender, EventArgs e)
        {
            LicenseEntry le = new LicenseEntry();
            le.Owner = this;
            le.ShowDialog();
            label35.Text = "Licensed to: " + Program.userLicenseName;
            label37.Text = "License Key: " + Program.userLicenseKey;
            if (Program.validLicense)
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

            foreach (User us in Program.dat.userList)
            {
                if (curUser.getUsername().Equals(us.getUsername()))   //Save curuser to datalist
                {
                    Program.dat.userList.Remove(us);
                    Program.dat.userList.Add(curUser);
                    break;

                }
            }


            Login l = new Login(1);
            l.ShowDialog();
            if (curUser != null)
            {
                label38.Text = "Current User: " + curUser.getUsername();
                FileOps.savePreferences(Program.prefPath);
            }
        }



        private void button45_Click(object sender, EventArgs e)
        {
            label38.Text = "Current User: " + curUser.getUsername();
        }
    }
}

