using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace UniCadeCmd
{
    public partial class GUI : Form
    {
        private KeyHandler ghk;
        public static ArrayList conList;
        public static int index;
        public static string curCon;
        public static bool gameSelectionActive;
        public static bool infoWindowActive;
        public Console gameSelectionConsole;
        public static bool fav;
        public static SettingsWindow sw;
        int conCount;
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            //Cursor.Hide();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BringToFront();
            label2.Visible = false;

            BackgroundThread bt = new BackgroundThread();  //Start a new background thread
            //Thread t1 = new Thread(bt.BackgroundProcess);
            //t1.Start();


            Taskbar.Hide();
           // this.TopMost = true;
            ghk = new KeyHandler(Keys.F4, this);
            ghk.Register();
            listBox1.Visible = false;
            // textBox1.BackColor = Color.Transparent;
            gameSelectionActive = false;
            infoWindowActive = false;
            richTextBox1.Visible = false;

            if (SettingsWindow.showSplash == 1)
            {
                Splash spl = new Splash();
                spl.ShowDialog();
            }

            conList = new ArrayList();
            conCount = 0;
            index = 0;
            foreach (Console c in Program.dat.consoleList)
            {
                conList.Add(c.getName());
                conCount++;
            }

            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.BackColor = Color.Transparent;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox4.BackColor = Color.Transparent;
            pictureBox5.BackColor = Color.Transparent;
            pictureBox6.BackColor = Color.Transparent;
            pictureBox7.BackColor = Color.Transparent;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;

            if (SettingsWindow.payPerPlay > 0)
            {

                if (SettingsWindow.playtime > 0)
                {
                    displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + SettingsWindow.coins);
                }
                else if (SettingsWindow.coins > 0)
                {

                    displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                }

            }
            else
            {
                label3.Visible = false;
            }


            pictureBox2.Load(@"C:\UniCade\Media\Backgrounds\UniCade Logo (Narrow).png");


            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyUp);


            updateGUI();
            this.Focus();
            this.Activate();

            FileOps.refreshGameCount();
            label1.Text = "Total Game Count: " + Database.totalGameCount;

        }


        private void GUI_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Determine whether the key entered is the F1 key. Display help if it is.
            if (e.KeyCode == Keys.Left)
            {
                if (!gameSelectionActive)
                {
                    left();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (!gameSelectionActive)
                {
                    right();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (gameSelectionActive)
                {

                    launchGame();
                }
                else
                {
                    openGameSelection();
                    gameSelectionActive = true;
                }

            }
            else if (e.KeyCode == Keys.I)  //Display info
            {
                if (gameSelectionActive)
                {
                    if (!infoWindowActive)
                    {
                        displayGameInfo();
                        infoWindowActive = true;
                    }
                    else
                    {
                        richTextBox1.Visible = false;  //Close Info Window
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = false;
                        pictureBox7.Visible = false;
                        infoWindowActive = false;
                    }
                }
                else
                {
                    infoWindowActive = true;
                    displayConsoleInfo();

                }
            }

            else if (e.KeyCode == Keys.Space)  //Add or remove favorites
            {
                if (gameSelectionActive)
                {
                    if (listBox1.SelectedItem != null)
                    {
                        foreach (Game g in gameSelectionConsole.getGameList())
                        {
                            if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
                            {
                                if (g.getFav() > 0)
                                {
                                    g.setFav(0);
                                    createNotification("Removed from Favorites");
                                }
                                else
                                {
                                    g.setFav(1);
                                    createNotification("Added to Favorites");
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.F10)  // Insert coin
            {
                Program.coins++;
                if (SettingsWindow.payPerPlay > 0)
                {

                    if (SettingsWindow.playtime > 0)
                    {
                        Program.gui.displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + Program.coins);
                    }
                    else if (SettingsWindow.coins > 0)
                    {
                        Program.gui.displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                    }

                }
                createNotification("Coin Inserted - Total Coins: " + Program.coins);
            }

            else if (e.KeyCode == Keys.F)  //Toggle Favorites view
            {
                if (gameSelectionActive)
                {
                    if (fav)
                    {
                        fav = false;
                    }
                    else
                    {
                        fav = true;
                    }
                    openGameSelection();
                }
            }
            else if (e.Alt && (e.KeyCode == Keys.C))  //Display Command line and close gui
            {
                Taskbar.Show();
                Application.Exit();
            }

            else if (e.Alt && (e.KeyCode == Keys.P))  //Display preferences window
            {

                if (SettingsWindow.passProtect > 0)
                {
                    PassWindow pw = new PassWindow();
                    DialogResult result = pw.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        SettingsWindow sw = new SettingsWindow();
                        sw.ShowDialog();
                    }
                }
                else
                {
                    sw = new SettingsWindow();
                    sw.ShowDialog();
                }

            }
            else if (e.Alt && (e.KeyCode == Keys.X))  //Close current process
            {

            }

            else if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))  //Close Current Window
            {

                closeNotification();
                if (gameSelectionActive)
                {
                    if (infoWindowActive)
                    {
                        richTextBox1.Visible = false;  //Close Info Window
                        listBox1.Visible = true;
                        infoWindowActive = false;
                    }
                    else
                    {
                        listBox1.Visible = false;  //Close Game Selection window
                        pictureBox1.Visible = true;
                        gameSelectionActive = false;
                        label1.Text = "Total Game Count: " + Database.totalGameCount;
                        pictureBox4.Image = null;
                    }

                }
                else
                {

                    richTextBox1.Visible = false;  //Close Info Window
                    pictureBox1.Visible = true;
                    infoWindowActive = false;
                }

            }
        }











        private void button3_Click(object sender, EventArgs e)  //Right
        {
            right();

        }

        private void button2_Click(object sender, EventArgs e)  //Left
        {
            left();
        }

        private void right()
        {
            closeNotification();
            if (index < (conCount - 1))
            {
                index++;
            }
            else
            {
                index = 0;
            }
            updateGUI();
        }

        private void left()
        {
            closeNotification();
            if (index > 0)
            {
                index--;
            }
            else
            {
                index = (conCount - 1);
            }
            updateGUI();
        }

        private void enter()
        {

        }

        private void button1_Click(object sender, EventArgs e)  //Select/Enter
        {

        }

        private void updateGUI()
        {
            if (!FileOps.processActive)
            {
                //Program.gui.TopMost = true;
            }
            curCon = (string)conList[index];
            //System.Console.WriteLine(@"C: \UniCade\Media\Consoles\" + conList[index] + ".png");
            if ((File.Exists(@"C: \UniCade\Media\Consoles\" + conList[index] + ".png")))
            {
                pictureBox1.Load(@"C: \UniCade\Media\Consoles\" + conList[index] + ".png");
                if (File.Exists(@"C: \UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png"))
                {
                    pictureBox3.Load(@"C: \UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png");
                }
                else
                {
                    pictureBox3.Image.Dispose();
                    pictureBox3.Image = null;
                }
            }
            else
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }


        }

        private void openGameSelection()
        {
            gameSelectionActive = true;
            pictureBox1.Visible = false;
            listBox1.Items.Clear();
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(conList[index]))
                {
                    gameSelectionConsole = c;
                    label1.Text = c.getName() + " Game Count: " + c.gameCount;
                    if (fav)
                    {
                        listBox1.Items.Add(c.getName() + " Favorites:\n\n");
                    }
                    foreach (Game g in c.getGameList())
                    {
                        if (fav)
                        {

                            if (g.getFav() == 1)
                            {
                                listBox1.Items.Add(g.getTitle());
                            }
                        }
                        else
                        {
                            listBox1.Items.Add(g.getTitle());
                        }
                    }
                    break;
                }
            }

            listBox1.Visible = true;
        }





        private void launchGame()
        {
            foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    FileOps.launch(g, gameSelectionConsole);
                    break;
                }
            }

        }

        private void displayGameInfo()
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            pictureBox5.Visible = true;
            pictureBox6.Visible = true;
            pictureBox7.Visible = true;

            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;

            foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    richTextBox1.Text = Program.displayGameInfo(g);

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png"))
                    {
                        pictureBox5.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png"))
                    {
                        pictureBox6.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png"))
                    {
                        pictureBox7.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png");

                    }

                    break;
                }
            }




            richTextBox1.Visible = true;
        }

        private void displayConsoleInfo()
        {
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(conList[index]))
                {
                    Program.displayConsoleInfo(c);
                    break;
                }
            }

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    break;
            }

            base.OnFormClosing(e);
        }

        private void HandleHotkey()
        {
            System.Console.WriteLine("HOTKEY DETECTED");
            FileOps.killLaunch();
            Thread.Sleep(1500);
            this.WindowState = FormWindowState.Normal;
            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BringToFront();
            this.TopMost = true;
            this.Activate();

        }

        public void activate()
        {
            this.Activate();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeNotification();
            foreach (Game g in gameSelectionConsole.getGameList())
            {

                if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
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
                    else if (g.getEsrb().Equals("Adults Only (AO)"))
                    {
                        pictureBox4.Load(@"C:\UniCade\Media\Esrb\Adults Only (AO).png");
                    }

                }
            }


        }

        public void createNotification(String notification)
        {
            label2.Visible = true;
            label2.Text = notification;
            label2.Focus();
            label2.BringToFront();
        }

        public void closeNotification()
        {
            label2.Text = null;
            label2.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
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

        private void pictureBox6_Click(object sender, EventArgs e)
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

        private void pictureBox7_Click(object sender, EventArgs e)
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

        public void displayPayNotification(String s)
        {
            label2.Focus();
            label2.BringToFront();
            label3.Visible = true;
            label3.Text = s;
        }
        public void closePayNotification()
        {
            label3.Visible = false;
            label3.Text = null;
        }


    }
}

