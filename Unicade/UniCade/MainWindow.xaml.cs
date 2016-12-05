using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static ArrayList conList;
        public static int index;
        public static string curCon;
        public static bool passValid;
        public static bool gameSelectionActive;
        public static bool gameRunning;
        public static bool infoWindowActive;
        public Console gameSelectionConsole;
        public static bool settingsWindowActive;
        public static bool fav;
        public static SettingsWindow sw;
        public static int conCount;
        public static GlobalKeyboardHook gkh;
        GameInfo gi;



        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Fatal Error");
                return;
            }
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\Interface Background.png"));
            this.Background = myBrush;
            sw = new SettingsWindow();
            gameRunning = false;
            //this.KeyDown += new System.Windows.Input.KeyEventHandler(OnButtonKeyDown);
            listBox.Visibility = Visibility.Hidden;
            listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            image2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            //SettingsWindow.label38.Text = "Current User: " + SettingsWindow.curUser.Username;
            gi = new GameInfo();

            gkh = new GlobalKeyboardHook();

            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
            gkh.HookedKeys.Add(Keys.Left);
            gkh.HookedKeys.Add(Keys.Right);
            gkh.HookedKeys.Add(Keys.Enter);
            gkh.HookedKeys.Add(Keys.I);
            gkh.HookedKeys.Add(Keys.Back);
            gkh.HookedKeys.Add(Keys.Space);
            gkh.HookedKeys.Add(Keys.Tab);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.Delete);
            gkh.HookedKeys.Add(Keys.F);
            gkh.HookedKeys.Add(Keys.G);
            gkh.HookedKeys.Add(Keys.C);
            gkh.HookedKeys.Add(Keys.P);
            gkh.HookedKeys.Add(Keys.B);
            gkh.HookedKeys.Add(Keys.S);
            gkh.HookedKeys.Add(Keys.E);
            gkh.HookedKeys.Add(Keys.Q);
            gkh.HookedKeys.Add(Keys.F10);
            gkh.HookedKeys.Add(Keys.F1);
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new System.Windows.Forms.KeyEventHandler(gkh_KeyUp);





            label3.Content = "Unlicensed Version";

            if (Program._validLicense)
            {
                label3.Visibility = Visibility.Hidden;
            }
            //Taskbar.Hide();

            refreshConList();

            if (SettingsWindow.payPerPlay > 0)
            {

                if (SettingsWindow.coins > 0)
                {
                    //label2.Visibility = Visibility.Visible;
                    displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program._coins);
                }

            }
            else
            {
                label2.Visibility = Visibility.Hidden;
            }


            FileOps.RefreshGameCount();
            label.Content = "Total Game Count: " + Database.TotalGameCount;
            updateGUI();
        }


        void gkh_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //lstLog.Items.Add("Up\t" + e.KeyCode.ToString());
            e.Handled = true;
        }

        public static void refreshConList()
        {
            conList = new ArrayList();
            conCount = 0;
            index = 0;
            foreach (Console c in Database.ConsoleList)
            {
                conList.Add(c.Name);
                conCount++;
            }
        }






        /*private void OnButtonKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Old method
        }*/

        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {


            if (e.KeyCode == Keys.F10)
            {
                if (gameRunning)
                {
                    System.Console.WriteLine("KILL");
                    NotificationWindow nfw = new NotificationWindow("UniCade System", "Game Successfully Closed");
                    nfw.Show();
                    FileOps.KillProcess();
                    this.Activate();
                }
            }

            if ((!gameRunning))
            {
                if (e.KeyCode == Keys.I)  //Display info
                {
                    if (gameSelectionActive)
                    {
                        displayGameInfo();
                    }
                    else if (infoWindowActive)
                    {
                        gi.Hide();
                        infoWindowActive = false;
                    }
                }
            }

            if ((!gameRunning) && (!infoWindowActive))
            {
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


                else if (e.KeyCode == Keys.Space)  //Add or remove Favorites
                {
                    if (gameSelectionActive)
                    {
                        if (listBox.SelectedItem != null)
                        {
                            foreach (Game g in gameSelectionConsole.GameList)
                            {
                                if (listBox.SelectedItem.ToString().Equals(g.Title))
                                {
                                    if (SettingsWindow.curUser.Favorites.Count < 1)
                                    {
                                        SettingsWindow.curUser.Favorites.Add(g);
                                        NotificationWindow nfw = new NotificationWindow("UniCade", SettingsWindow.curUser.Username + " :Added To Favorites");
                                        nfw.Show();
                                        return;
                                    }
                                    foreach (Game g1 in SettingsWindow.curUser.Favorites)
                                    {
                                        if (g1.Title.Equals(g.Title) && g.Console.Equals(g1.Console))
                                        {
                                            SettingsWindow.curUser.Favorites.Add(g);
                                            NotificationWindow nfw = new NotificationWindow("UniCade", SettingsWindow.curUser.Username + ": Removed From Favorites");
                                            nfw.Show();
                                        }

                                        else
                                        {
                                            SettingsWindow.curUser.Favorites.Add(g);
                                            NotificationWindow nfw = new NotificationWindow("UniCade", SettingsWindow.curUser.Username + ": Added To Favorites");
                                            nfw.Show();

                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.G)  //Add or remove global favorite
                {
                    if (gameSelectionActive)
                    {
                        foreach (Game g in gameSelectionConsole.GameList)
                        {
                            if (listBox.SelectedItem.ToString().Equals(g.Title))
                            {
                                if (g.Favorite > 0)
                                {
                                    g.Favorite = 0;
                                    NotificationWindow nfw = new NotificationWindow("UniCade","Removed From Global Favorites");
                                    nfw.Show();
                                }
                                else
                                {
                                    g.Favorite = 1;
                                    NotificationWindow nfw = new NotificationWindow("UniCade", "Added To Global Favorites");
                                    nfw.Show();
                                }
                            }
                        }
                    }
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
                else if ((e.KeyCode == Keys.C) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Display Command line and close gui
                {
                    System.Windows.Application.Current.Shutdown();
                }

                else if ((e.KeyCode == Keys.P) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Display preferences window
                {

                    if (SettingsWindow.passProtect > 0)
                    {
                        passValid = false;
                        NotificationWindow nfw = new NotificationWindow("Security", "Enter Password");
                        nfw.Show();
                        PassWindow pw = new PassWindow();
                        pw.ShowDialog();



                        if (passValid)
                        {
                            sw = new SettingsWindow();
                            unhookKeys();
                            settingsWindowActive = true;
                            sw.ShowDialog();

                        }
                    }
                    else
                    {
                        sw = new SettingsWindow();
                        settingsWindowActive = true;
                        unhookKeys();
                        sw.ShowDialog();
                    }
                    if (Program._validLicense)
                    {
                        label3.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        label3.Visibility = Visibility.Visible;
                    }

                }
                }
                if (!gameRunning && infoWindowActive)
                {
                    if (e.KeyCode == Keys.F)
                    {
                        gi.expand();
                    }
                    else if (e.KeyCode == Keys.B)
                    {
                        gi.expand1();
                    }
                    else if (e.KeyCode == Keys.S)
                    {
                        gi.expand2();
                    }
                    else if (e.KeyCode == Keys.E)
                    {
                        gi.expand3();
                    }


                }
                if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))  //Close Game Selection Window
                {

                    if (infoWindowActive)
                    {
                        gi.Hide();
                        infoWindowActive = false;
                    }
                    else
                    {
                        listBox.Visibility = Visibility.Hidden;  //Close Game Selection window
                        image2.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;

                        gameSelectionActive = false;
                        label.Content = "Total Game Count: " + Database.TotalGameCount;
                    }

                }
                else if (e.KeyCode == Keys.Tab)  // Insert coin
                {
                    //this.Activate();
                    Program._coins++;
                    if (SettingsWindow.payPerPlay > 0)
                    {

                        if (SettingsWindow.playtime > 0)
                        {
                            //displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + Program.coins);
                        }
                        if (SettingsWindow.coins > 0)
                        {
                            //displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                            label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program._coins;

                        }
                        NotificationWindow nfw = new NotificationWindow("Pay Per Play", "Coin Inserted\n Current: " + Program._coins);
                        nfw.Show();
                    }
                    else
                    {
                        NotificationWindow nfw = new NotificationWindow("UniCade", "Free Play Enabled. NO COIN REQUIRED");
                        nfw.Show();
                        //this.Activate();
                    }

                }






                updateGUI();

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



        private void updateGUI()
        {

            if (SettingsWindow.payPerPlay > 0)
            {
                if (SettingsWindow.coins > 0)
                {
                    //label2.Visibility = Visibility.Visible;
                    label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program._coins;
                }
            }
            else
            {
                label2.Visibility = Visibility.Hidden;
            }

            BitmapImage b;

            if (!FileOps.processActive)
            {
                //Program.gui.TopMost = true;
            }
            curCon = (string)conList[index];
            //System.Console.WriteLine(@"C:\UniCade\Media\Consoles\" + conList[index] + ".png");
            if ((File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + conList[index] + ".png"))&& (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png")))
            {
                label1.Visibility = Visibility.Hidden;
                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + conList[index] + ".png");
                b.EndInit();
                image.Source = b;


                    b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png");
                    b.EndInit();
                    image1.Source = b;
              
            }
            else
            {
                image.Source = null;
                image1.Source = null;
                label1.Content = conList[index] + " Missing Console Image/Logo ";
                label1.Visibility = Visibility.Visible;
            }


        }



        private void openGameSelection()
        {
            gameSelectionActive = true;
            image.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;

            if (!fav)
            {
                label1.Content = (conList[index] + " Library");
            }
            else
            {
                label1.Content = SettingsWindow.curUser.Username + "'s Favorites List";
            }

            listBox.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(conList[index]))
                {
                    gameSelectionConsole = c;
                    label.Content = c.Name + " Game Count: " + c.GameCount;

                    foreach (Game g in c.GameList)
                    {
                        if (fav)
                        {
                            foreach (Game g1 in SettingsWindow.curUser.Favorites)
                            {
                                if (g.Title.Equals(g1.Title) && g.Console.Equals(g1.Console))
                                {
                                    listBox.Items.Add(g.Title);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            if (SettingsWindow.viewEsrb > 0)
                            {
                                int EsrbNum = SettingsWindow.calcEsrb(g.Esrb);
                                if (EsrbNum <= SettingsWindow.calcEsrb(SettingsWindow.curUser.AllowedEsrb))
                                {
                                    listBox.Items.Add(g.Title);
                                }
                            }
                            else
                            {
                                listBox.Items.Add(g.Title);
                            }
                        }
                        //break;
                    }
                }
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png"))
            {

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png");
                b.EndInit();
                image2.Source = b;

                label1.Content = (conList[index] + "Library");
            }
            else
            {
                image.Source = null;
            }


            listBox.Visibility = Visibility.Visible;
            if (listBox.Items.Count > 0)               //Auto set initial index to first item
            {
                listBox.SelectedIndex = 0;
                ListBoxItem item = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromIndex(0);
                Keyboard.Focus(listBox);
                Keyboard.Focus(item);
            }
        }



        private void launchGame()
        {
            if (SettingsWindow.payPerPlay > 0)
            {
                if (SettingsWindow.playtime > 0)
                {
                    if (!Program._playtimeRemaining)
                    {
                        // Program.gui.createNotification("Playtime Expired: Insert More coins");
                        return;
                    }
                }
                if (SettingsWindow.coins > 0)
                {

                    if (Program._coins < SettingsWindow.coins)
                    {

                        NotificationWindow nfw = new NotificationWindow("Pay Per Play", "Insert Coins");
                        nfw.Show();
                        return;
                    }

                    Program._coins = Program._coins - SettingsWindow.coins;
                    displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program._coins);


                }
            }

            
            foreach (Game g in gameSelectionConsole.GameList)
            {
                if (listBox.SelectedItem.ToString().Equals(g.Title))
                {
                    
                    //pause(3000);
                    Task.Delay(3000);
                    FileOps.launch(g, gameSelectionConsole);
                    break;
                }
            }

        }

        private void displayGameInfo()
        {
            if (listBox.SelectedItem == null)
            {
                return;
            }
            infoWindowActive = true;
            BitmapImage b;

            //gi = new GameInfo();
            gi.textBlock1.Text = null;
            gi.textBlock.Text = null;
            gi.image.Source = null;
            gi.image1.Source = null;
            gi.image2.Source = null;
            gi.image3.Source = null;

            foreach (Game g in gameSelectionConsole.GameList)
            {
                if (listBox.SelectedItem.ToString().Equals(g.Title))
                {
                    
                    gi.textBlock1.Text = g.Console + " - " + g.Title;
                    gi.textBlock.Text = Program.displayGameInfo(g);

                    

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_BoxFront.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_BoxFront.png");
                        b.EndInit();
                        gi.image.Source = b;

                    }

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_BoxBack.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_BoxBack.png");
                        b.EndInit();
                        gi.image1.Source = b;

                    }

                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_Screenshot.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + gameSelectionConsole.Name + "\\" + g.Title + "_Screenshot.png");
                        b.EndInit();
                        gi.image2.Source = b;

                    }
                    String EsrbPath = "";
                    if (g.Esrb.Equals("Everyone"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png";
                    }
                    else if (g.Esrb.Equals("Everyone (KA)"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png";
                    }
                    else if (g.Esrb.Equals("Everyone 10+"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png";
                    }
                    else if (g.Esrb.Equals("Teen"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png";
                    }
                    else if (g.Esrb.Equals("Mature"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png";
                    }
                    if (g.Esrb.Equals("Adults Only (AO)"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() +  @"\Media\Esrb\Adults Only (AO).png";
                    }

                    if (EsrbPath.Length > 2)
                    {

                        gi.displayEsrb(EsrbPath);
                    }

                }
            }
            gi.Show();
        }


        public static void hookKeys()
        {
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
            gkh.HookedKeys.Add(Keys.G);
            gkh.HookedKeys.Add(Keys.Left);
            gkh.HookedKeys.Add(Keys.Right);
            gkh.HookedKeys.Add(Keys.Enter);
            gkh.HookedKeys.Add(Keys.I);
            gkh.HookedKeys.Add(Keys.Back);
            gkh.HookedKeys.Add(Keys.Space);
            gkh.HookedKeys.Add(Keys.Tab);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.Delete);
            gkh.HookedKeys.Add(Keys.F);
            gkh.HookedKeys.Add(Keys.C);
            gkh.HookedKeys.Add(Keys.P);
            gkh.HookedKeys.Add(Keys.B);
            gkh.HookedKeys.Add(Keys.S);
            gkh.HookedKeys.Add(Keys.E);
            gkh.HookedKeys.Add(Keys.Q);
            gkh.HookedKeys.Add(Keys.F1);

        }

        public static void unhookKeys()
        {
            gkh.HookedKeys.Remove(Keys.A);
            gkh.HookedKeys.Remove(Keys.B);
            gkh.HookedKeys.Add(Keys.G);
            gkh.HookedKeys.Remove(Keys.Left);
            gkh.HookedKeys.Remove(Keys.Right);
            gkh.HookedKeys.Remove(Keys.Enter);
            gkh.HookedKeys.Remove(Keys.Space);
            gkh.HookedKeys.Remove(Keys.I);
            gkh.HookedKeys.Remove(Keys.Back);
            gkh.HookedKeys.Remove(Keys.Tab);
            gkh.HookedKeys.Remove(Keys.Escape);
            gkh.HookedKeys.Remove(Keys.Delete);
            gkh.HookedKeys.Remove(Keys.F);
            gkh.HookedKeys.Remove(Keys.P);
            gkh.HookedKeys.Remove(Keys.C);
            gkh.HookedKeys.Remove(Keys.B);
            gkh.HookedKeys.Remove(Keys.S);
            gkh.HookedKeys.Remove(Keys.E);
            gkh.HookedKeys.Remove(Keys.Q);
            gkh.HookedKeys.Remove(Keys.F1);

        }



















        public void displayPayNotification(String s)
        {
            label2.Content = s;
        }


        public static void pause(int time)
        {

            Stopwatch sw = new Stopwatch(); // sw cotructor
            sw.Start(); // starts the stopwatch
            for (int i = 0; ; i++)
            {
                if (i % 1000 == 0) 
                {
                    sw.Stop(); 
                    if (sw.ElapsedMilliseconds > time) 
                    {
                        break; 
                    }
                    else
                    {
                        sw.Start(); 
                    }
                }
            }
        }

 






    }
}
