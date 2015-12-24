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
        int conCount;
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BringToFront();
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
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


            pictureBox2.Load(@"C:\UniCade\Media\Backgrounds\UniCade Logo (Narrow).png");


            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyUp);


            updateGUI();

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
                        infoWindowActive = false;
                    }
                }
                else
                {
                    infoWindowActive = true;
                    displayConsoleInfo();

                }
            }
            else if (e.Alt&& (e.KeyCode == Keys.C))  //Display Command line and close gui
            {
                Close();
            }
            else if (e.Alt&& (e.KeyCode == Keys.P))  //Display preferences window
            {
                SettingsWindow sw = new SettingsWindow();
                sw.ShowDialog();
            }
            else if (e.Alt && (e.KeyCode == Keys.X))  //Close current process
            {
                
            }

            else if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))  //Close Current Window
            {

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
                    foreach (Game g in c.getGameList())
                    {

                        listBox1.Items.Add(g.getTitle());
                    }
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
            if(listBox1.SelectedItem == null)
            {
                return;
            }
            foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    richTextBox1.Text = Program.displayGameInfo(g);
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

        private void HandleHotkey()
        {
            System.Console.WriteLine("HOTKEY DETECTED");
            this.WindowState = FormWindowState.Normal;
            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BringToFront();

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
    }
}

