using System;
using System.Collections;
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
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using UniCade;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        public MainWindow()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("YES");
            //Taskbar.Hide();

            conList = new ArrayList();
            conCount = 0;
            index = 0;
            foreach (Console c in Program.dat.consoleList)
            {
                conList.Add(c.getName());
                conCount++;
            }

            if (SettingsWindow.payPerPlay > 0)
            {

                if (SettingsWindow.playtime > 0)
                {
                    //displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + SettingsWindow.coins);
                }
                else if (SettingsWindow.coins > 0)
                {

                    // displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                }

            }
            else
            {
                //label3.Visible = false;
            }
        }



        private void Window_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("KeyDown");
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
                          //Close Info Window

                        infoWindowActive = false;
                    }
                }
                else
                {
                    infoWindowActive = true;
                    displayConsoleInfo();

                }
            }

           /* else if (e.KeyCode == Keys.Space)  //Add or remove favorites
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
            }*/
            else if (e.KeyCode == Keys.F10)  // Insert coin
            {
                Program.coins++;
                if (SettingsWindow.payPerPlay > 0)
                {

                    if (SettingsWindow.playtime > 0)
                    {
                        displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + Program.coins);
                    }
                    else if (SettingsWindow.coins > 0)
                    {
                       displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
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
                //Application.Exit();
            }

            else if (e.Alt && (e.KeyCode == Keys.P))  //Display preferences window
            {

                if (SettingsWindow.passProtect > 0)
                {
                    PassWindow pw = new PassWindow();
                    DialogResult result = pw.ShowDialog();

                    if (true)//result == DialogResult.OK)
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
                        //richTextBox1.Visible = false;  //Close Info Window
                        //listBox1.Visible = true;
                        infoWindowActive = false;
                    }
                    else
                    {
                        //listBox1.Visible = false;  //Close Game Selection window
                        //pictureBox1.Visible = true;
                        gameSelectionActive = false;
                        //label1.Text = "Total Game Count: " + Database.totalGameCount;
                        //pictureBox4.Image = null;
                    }

                }
                else
                {

                    //richTextBox1.Visible = false;  //Close Info Window
                    //pictureBox1.Visible = true;
                    infoWindowActive = false;
                }

            }
        



            updateGUI();


        FileOps.refreshGameCount();
            //label1.Text = "Total Game Count: " + Database.totalGameCount;

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
                image.Source = new BitmapImage(new Uri(@"C: \UniCade\Media\Consoles\" + conList[index] + ".png", UriKind.Relative));
                if (File.Exists(@"C: \UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png"))
                {
                    image.Source = new BitmapImage(new Uri(@"C: \UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png", UriKind.Relative));
                }
                else
                {
                    image.Source = null;
                }
            }
            else
            {
                image.Source = null;
                image.Source = null;
            }


        }

        private void openGameSelection()
        {
            gameSelectionActive = true;
            image.Visibility = Visibility.Hidden;
            //listBox1.Items.Clear();
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(conList[index]))
                {
                    gameSelectionConsole = c;
                    //label1.Text = c.getName() + " Game Count: " + c.gameCount;
                    if (fav)
                    {
                        //listBox1.Items.Add(c.getName() + " Favorites:\n\n");
                    }
                    foreach (Game g in c.getGameList())
                    {
                        if (fav)
                        {

                            if (g.getFav() == 1)
                            {
                                //listBox1.Items.Add(g.getTitle());
                            }
                        }
                        else
                        {
                            //listBox1.Items.Add(g.getTitle());
                        }
                    }
                    break;
                }
            }

            //listBox1.Visible = true;
        }





        private void launchGame()
        {
            /*foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox1.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    FileOps.launch(g, gameSelectionConsole);
                    break;
                }
            }*/

        }

        private void displayGameInfo()
        {
           /* if (listBox1.SelectedItem == null)
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
                    //richTextBox1.Text = Program.displayGameInfo(g);

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png"))
                    {
                        //pictureBox5.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png"))
                    {
                        //pictureBox6.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png");

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png"))
                    {
                        //pictureBox7.Load(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png");

                    }

                    break;
                }*/
            }




            
        

        private void displayConsoleInfo()
        {
            /*foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(conList[index]))
                {
                    Program.displayConsoleInfo(c);
                    break;
                }
            }*/

        }

        public void createNotification(String notification)
        {
            /*label2.Visible = true;
            label2.Text = notification;
            label2.Focus();
            label2.BringToFront();
            */
        }

        public void closeNotification()
        {
            //label2.Text = null;
            //label2.Visible = false;
        }

        public void displayPayNotification(String s)
        {
            /*label2.Focus();
            label2.BringToFront();
            label3.Visible = true;
            label3.Text = s;*/
        }
        public void closePayNotification()
        {
           // label3.Visible = false;
          //  label3.Text = null;
        }





        public void loadImage() {

            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("C:\\Users\\Ben\\Dropbox\\Workspace\\Unicade\\UniCade\\Images\\Arcade.jpg");
            b.EndInit();
            image.Source = b;
            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            loadImage();
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
    }
}
