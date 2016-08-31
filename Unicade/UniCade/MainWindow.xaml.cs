﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using UniCade;
using System.Windows.Threading;
using Unicade;
using System.Diagnostics;

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
        public static bool passValid;
        public static bool gameSelectionActive;
        public static bool infoWindowActive;
        public Console gameSelectionConsole;
        public static bool fav;
        public static SettingsWindow sw;
        int conCount;

        public MainWindow()
        {
            InitializeComponent();
            sw = new SettingsWindow();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            listBox.Visibility = Visibility.Hidden;
            listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            image2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;

            label3.Content = "Unlicensed Version";

            if (Program.validLicense)
            {
                label3.Visibility = Visibility.Hidden;
            }
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

                 if (SettingsWindow.coins > 0)
                {
                    //label2.Visibility = Visibility.Visible;
                    displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                }

            }
            else
            {
                label2.Visibility = Visibility.Hidden;
            }


            FileOps.refreshGameCount();
            label.Content = "Total Game Count: " + Database.totalGameCount;
            updateGUI();
        }



        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (!gameSelectionActive)
                {
                    left();
                }
            }
            else if (e.Key == Key.Right)
            {
                if (!gameSelectionActive)
                {
                    right();
                }
            }
            else if (e.Key == Key.Enter)

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
            else if (e.Key == Key.I)  //Display info
            {
                if (gameSelectionActive)
                {
                    displayGameInfo();
                }
            }

            else if (e.Key == Key.Space)  //Add or remove favorites
            {
                if (gameSelectionActive)
                {
                    if (listBox.SelectedItem != null)
                    {
                        foreach (Game g in gameSelectionConsole.getGameList())
                        {
                            if (listBox.SelectedItem.ToString().Equals(g.getTitle()))
                            {
                                if (g.getFav() > 0)
                                {
                                    g.setFav(0);
                                    NotificationWindow nfw = new NotificationWindow("UniCade", "Removed From Favorites");
                                    nfw.Show();
                                }
                                else
                                {
                                    g.setFav(1);
                                    NotificationWindow nfw = new NotificationWindow("UniCade", "Added To Favorites");
                                    nfw.Show();

                                }
                                break;
                            }
                        }
                    }
                }
            }
            else if (e.Key == Key.Tab)  // Insert coin
            {
                Program.coins++;
                if (SettingsWindow.payPerPlay > 0)
                {

                    if (SettingsWindow.playtime > 0)
                    {
                        //displayPayNotification("(PayPerPlay) Total Playtime: " + SettingsWindow.playtime + " Mins" + "Coins Required:" + Program.coins);
                    }
                    if (SettingsWindow.coins > 0)
                    {
                        //displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);
                        label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins;
                        
                    }
                    NotificationWindow nfw = new NotificationWindow("Pay Per Play", "Coin Inserted\n Current: " + Program.coins);
                    nfw.Show();
                }
                else
                {
                    NotificationWindow nfw = new NotificationWindow("UniCade", "Free Play Enabled. NO COIN REQUIRED");
                    nfw.Show();
                }
                
            }

            else if (e.Key == Key.F)  //Toggle Favorites view
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
            else if ((e.Key == Key.C) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Display Command line and close gui
            {
                Taskbar.Show();
                System.Windows.Application.Current.Shutdown();
            }

            else if ((e.Key == Key.P) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Display preferences window
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
                        //SettingsWindow sw = new SettingsWindow();
                        sw.ShowDialog();
                    }
                }
                else
                {
                    //sw = new SettingsWindow();
                    sw.ShowDialog();
                }
                if (Program.validLicense)
                {
                    label3.Visibility = Visibility.Hidden;
                }
                else
                {
                    label3.Visibility = Visibility.Visible;
                }

            }
            else if ((e.Key == Key.X) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Close current process
            {

            }

            else if ((e.Key == Key.Escape) || (e.Key == Key.Delete) || (e.Key == Key.Back))  //Close Current Window
            {


                if (gameSelectionActive)
                {

                    {
                        listBox.Visibility = Visibility.Hidden;  //Close Game Selection window
                        image2.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Hidden;

                        gameSelectionActive = false;
                        label.Content = "Total Game Count: " + Database.totalGameCount;
                        //pictureBox4.Image = null;
                    }

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
                    label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins;
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
            if ((File.Exists(@"C:\UniCade\Media\Consoles\" + conList[index] + ".png")))
            {
                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(@"C:\UniCade\Media\Consoles\" + conList[index] + ".png");
                b.EndInit();
                image.Source = b;

                if (File.Exists(@"C:\UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png"))
                {
                    b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(@"C:\UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png");
                    b.EndInit();
                    image1.Source = b;
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
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            image3.Visibility = Visibility.Visible;
            image3.Source = null;
            label1.Content = (conList[index] + " Library");

            listBox.Items.Clear();
            foreach (Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(conList[index]))
                {
                    gameSelectionConsole = c;
                    label.Content = c.getName() + " Game Count: " + c.gameCount;
                    if (fav)
                    {
                        listBox.Items.Add(c.getName() + " Favorites:\n\n");
                    }
                    foreach (Game g in c.getGameList())
                    {
                        if (fav)
                        {

                            if (g.getFav() == 1)
                            {
                                listBox.Items.Add(g.getTitle());
                            }
                        }
                        else
                        {
                            listBox.Items.Add(g.getTitle());
                        }
                    }
                    break;
                }
            }

            if (File.Exists(@"C:\UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png"))
            {

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(@"C:\UniCade\Media\Consoles\Logos\" + conList[index] + " Logo" + ".png");
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SettingsWindow.viewEsrb > 0) {
                foreach (Game g in gameSelectionConsole.getGameList())
                {

                    if (listBox.SelectedItem.ToString().Equals(g.getTitle()))
                    {

                    }
                    String EsrbPath = "";
                    if (g.getEsrb().Equals("Everyone"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone.png";
                    }
                    else if (g.getEsrb().Equals("Everyone (KA)"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone.png";
                    }
                    else if (g.getEsrb().Equals("Everyone 10+"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone 10+.png";
                    }
                    else if (g.getEsrb().Equals("Teen"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Teen.png";
                    }
                    else if (g.getEsrb().Equals("Mature"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Mature.png";
                    }
                    if (g.getEsrb().Equals("Adults Only (AO)"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Adults Only (AO).png";
                    }

                    if (EsrbPath.Length > 2)
                    {
                        BitmapImage b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png");
                        b.EndInit();
                        image3.Source = b;
                    }
                    else
                    {
                        image3.Source = null;
                    }

                
                }
            }
            else
            {
                image3.Visibility = Visibility.Hidden;
            }


        }





        private void launchGame()
        {

            if (SettingsWindow.payPerPlay > 0)
            {
                if (SettingsWindow.playtime > 0)
                {
                    if (!Program.playtimeRemaining)
                    {
                        // Program.gui.createNotification("Playtime Expired: Insert More coins");
                        return;
                    }
                }
                if (SettingsWindow.coins > 0)
                {

                    if (Program.coins < SettingsWindow.coins)
                    {

                        NotificationWindow nfw = new NotificationWindow("Pay Per Play", "Insert Coins");
                        nfw.Show();
                        return;
                    }

                    Program.coins = Program.coins - SettingsWindow.coins;
                    displayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.coins + " Current: " + Program.coins);


                }
            }

            
            foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    NotificationWindow nfw = new NotificationWindow("System", "Loading ROM File");
                    nfw.Show();
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
            //infoWindowActive = true;
            BitmapImage b;

            GameInfo gi = new GameInfo();
            

            foreach (Game g in gameSelectionConsole.getGameList())
            {
                if (listBox.SelectedItem.ToString().Equals(g.getTitle()))
                {
                    
                    gi.textBlock1.Text = g.getConsole() + " - " + g.getTitle();
                    gi.textBlock.Text = Program.displayGameInfo(g);

                    

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxFront.png");
                        b.EndInit();
                        gi.image.Source = b;

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_BoxBack.png");
                        b.EndInit();
                        gi.image1.Source = b;

                    }

                    if (File.Exists(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png"))
                    {
                        b = new BitmapImage();
                        b.BeginInit();
                        b.UriSource = new Uri(@"C:\UniCade\Media\Games\" + gameSelectionConsole.getName() + "\\" + g.getTitle() + "_Screenshot.png");
                        b.EndInit();
                        gi.image2.Source = b;

                    }
                    String EsrbPath = "";
                    if (g.getEsrb().Equals("Everyone"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone.png";
                    }
                    else if (g.getEsrb().Equals("Everyone (KA)"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone.png";
                    }
                    else if (g.getEsrb().Equals("Everyone 10+"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Everyone 10+.png";
                    }
                    else if (g.getEsrb().Equals("Teen"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Teen.png";
                    }
                    else if (g.getEsrb().Equals("Mature"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Mature.png";
                    }
                    if (g.getEsrb().Equals("Adults Only (AO)"))
                    {
                        EsrbPath = @"C:\UniCade\Media\Esrb\Adults Only (AO).png";
                    }

                    if (EsrbPath.Length > 2)
                    {

                        gi.displayEsrb(EsrbPath);
                    }

                }
            }
            gi.ShowDialog();
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
