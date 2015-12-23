﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniCadeCmd
{
    public partial class SettingsWindow : Form
    {
        Console curConsole2;
        Console curConsole;
        public Game curGame;
        User curUser;

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
            populate();

        }

        private void populate()
        {
            richTextBox1.Text = "Readme \nContents";
            foreach (Console c in Program.dat.consoleList)
            {
                listBox1.Items.Add(c.getName());
                listBox2.Items.Add(c.getName());
            }
            comboBox1.Items.Add("None");
            comboBox1.Items.Add("Everyone");
            comboBox1.Items.Add("Everyone 10+");
            comboBox1.Items.Add("Teen");
            comboBox1.Items.Add("Mature");
            comboBox1.Items.Add("Adults Only (AO)");
            comboBox2.Items.Add("None");
            comboBox2.Items.Add("Everyone");
            comboBox2.Items.Add("Everyone 10+");
            comboBox2.Items.Add("Teen");
            comboBox2.Items.Add("Mature");
            comboBox2.Items.Add("Adults Only (AO)");

            if (restrictESRB == 1)
            {
                checkBox7.Checked = true;
            }
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
            if (WebOps.releaseDate>0)
            {
                checkBox8.Checked = true;
            }
            if (WebOps.critic>0)
            {
                checkBox9.Checked = true;
            }
            if (WebOps.publisher>0)
            {
                checkBox15.Checked = true;
            }
            if (WebOps.developer>0)
            {
                checkBox17.Checked = true;
            }
            if (WebOps.esrb>0)
            {
                checkBox18.Checked = true;
            }
            if (WebOps.esrbDescriptor>0)
            {
                checkBox19.Checked = true;
            }
            if (WebOps.players>0)
            {
                checkBox20.Checked = true;
            }
            if (WebOps.description>0)
            {
                checkBox21.Checked = true;
            }
            if (WebOps.boxFront>0)
            {
                checkBox22.Checked = true;
            }
            if (WebOps.boxBack>0)
            {
                checkBox23.Checked = true;
            }
            if (WebOps.screenshot>0)
            {
                checkBox24.Checked = true;
            }
            if (WebOps.metac>0)
            {
                checkBox4.Checked = true;
            }
            if (WebOps.mobyg>0)
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
            if (cmdOrGui == 1)
            {
                checkBox13.Checked = true;
            }
            if (payPerPlay > 0)
            {
                checkBox14.Checked = true;
            }
            if (perLaunch > 0)
            {
                checkBox16.Checked = true;
            }
            textBox29.Text = coins.ToString();
            textBox30.Text = playtime.ToString();

            foreach (User u in Program.dat.userList)
            {
                listBox4.Items.Add(u.getUsername());
            }

            refreshGlobalFavs();

            richTextBox1.Text = "(© Lenington Design)"; //Readme Info


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
            Close();
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
                    textBox2.Text = g.getTitle();
                    textBox13.Text = g.getConsole();
                    textBox12.Text = g.getReleaseDate();
                    textBox15.Text = g.getCriticScore();
                    textBox11.Text = g.getPublisher();
                    textBox10.Text = g.getDeveloper();
                    textBox6.Text = g.getEsrb();
                    textBox17.Text = g.getPlayers();
                    textBox19.Text = g.getEsrbDescriptor();
                    textBox18.Text = g.getTrivia();
                    if (g.getFav() == 1)
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }

                    refreshEsrbIcon(g);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)  //Close Button
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)   //Rescrape Info Button
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
            curGame.setReleaseDate(textBox12.Text);
            curGame.setCriticScore(textBox15.Text);
            curGame.setPublisher(textBox11.Text);
            curGame.setDeveloper(textBox10.Text);
            curGame.setEsrb(textBox6.Text);
            curGame.setPlayers(textBox17.Text);
            curGame.setTrivia(textBox18.Text);
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
            Close();
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
            Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button15_Click(object sender, EventArgs e)  //Save Global Settings 
        {
            Program.databasePath = textBox31.Text;
            Program.emuPath = textBox25.Text;
            Program.mediaPath = textBox32.Text;
            Program.romPath = textBox33.Text;

            int n = 0;
            Int32.TryParse(textBox7.Text, out n);
            if (n > 0)
            {
                passProtect = Int32.Parse(textBox7.Text);
            }
            Int32.TryParse(textBox29.Text, out n);
            if (n > 0)
            {
                coins = Int32.Parse(textBox7.Text);
            }
            Int32.TryParse(textBox30.Text, out n);
            if (n > 0)
            {
                playtime = Int32.Parse(textBox7.Text);
            }
            restrictESRB = calcEsrb(comboBox1.SelectedItem.ToString());
            FileOps.savePreferences(Program.prefPath);
        }



        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                viewEsrb = 1;
            }
            else
            {
                viewEsrb = 0;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                restrictESRB = 1;
            }
            else
            {
                restrictESRB = 0;
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
            if (checkBox10.Checked)
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
            if (checkBox10.Checked)
            {
                cmdOrGui = 1;
            }
            else
            {
                cmdOrGui = 0;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                payPerPlay = 1;
            }
            else
            {
                payPerPlay = 0;
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
                coins = Int32.Parse(textBox30.Text);
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

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curUser != null)
            {
              
                if (comboBox2.SelectedIndex > 0)
                {
                    curUser.seAge(calcEsrb(comboBox2.SelectedItem.ToString()));
                }
               
            }
            foreach (User u in Program.dat.userList)
            {
                if (u.getUsername().Equals(listBox4.SelectedItem.ToString()))
                {
                    curUser = u;
                    textBox23.Text = u.getUsername();
                    textBox24.Text = u.getPass();
                    textBox26.Text = u.getUserInfo();
                    textBox27.Text = u.getLoginCount().ToString();
                    textBox28.Text = u.getLaunchCount().ToString();
                }
            }
            listBox5.Items.Clear();
            foreach (string s in curUser.favorites)
            {
                listBox5.Items.Add(s.Replace('*', '-'));
            }

        }



        private void button10_Click(object sender, EventArgs e)  //Create new user
        {
            User u = new User();
            Program.dat.userList.Add(u);

            listBox4.Items.Clear();
            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            curUser.setName(textBox23.Text);
            curUser.setpass(textBox24.Text);
            curUser.setUserInfo(textBox26.Text);
            curUser.seAge(calcEsrb(comboBox2.SelectedItem.ToString()));
            FileOps.saveDatabase(Program.databasePath);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Program.dat.userList.Remove(curUser);
            listBox4.Items.Clear();
            curUser = null;
            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
            }


        }

        private void button17_Click(object sender, EventArgs e)
        {
            curUser.setName(textBox23.Text);
            curUser.setpass(textBox24.Text);
            curUser.setUserInfo(textBox26.Text);
            listBox4.Items.Clear();
            foreach (User us in Program.dat.userList)
            {
                listBox4.Items.Add(us.getUsername());
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            curGame.setReleaseDate(textBox12.Text);
            curGame.setCriticScore(textBox15.Text);
            if (curGame.getPublisher() != null)
            {
                curGame.setPublisher(textBox11.Text);
            }
            if (curGame.getDeveloper() != null)
            {
                curGame.setDeveloper(textBox10.Text);
            }
            if (curGame.getEsrb() != null)
            {
                curGame.setEsrb(textBox6.Text);
            }
            curGame.setPlayers(textBox17.Text);
            if (curGame.getTrivia() != null)
            {
                curGame.setTrivia(textBox18.Text);
            }

        }

        private void button19_Click(object sender, EventArgs e)  //Consoles - Save Info Button
        {
            curConsole.setName(textBox9.Text);
            curConsole.setEmuPath(textBox1.Text);
            //curConsole.setRomPath(textBox3.Text);
            curConsole.setRomExt(textBox4.Text);
            curConsole.setLaunchParam(textBox5.Text);
            curConsole.setReleaseDate(textBox22.Text);
            curConsole.setConsoleInfo(textBox20.Text);
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
            curUser.favorites.RemoveAt(listBox5.SelectedIndex);
            listBox5.Items.Clear();
            foreach (string s in curUser.favorites)
            {
                listBox5.Items.Add(s.Replace('*', '-'));
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
            Close();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {

        }
    }

    }
