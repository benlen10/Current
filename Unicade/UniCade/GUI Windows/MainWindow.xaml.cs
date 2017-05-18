using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniCade.Windows;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public static ArrayList _consoleList;
        public static int _index;
        public static string _currentConsole;
        public static bool _validPAss;
        public static bool _gameSelectionActive;
        public static bool _gameRunning;
        public static bool _infoWindowActive;
        public IConsole _gameSelectionConsole;
        public static bool _settingsWindowActive;
        public static bool _favorite;
        public static SettingsWindow sw;
        public static int _consoleCount;
        public static GlobalKeyboardHook gkh;
        private static GameInfo _gameInfo;

        #endregion

        /// <summary>
        /// Primary constructor for the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeMainWindow();
        }

        /// <summary>
        /// Initialize the primary GUI window on launch
        /// </summary>
        private void InitializeMainWindow()
        {
            //Load the GUI background image
            ImageBrush myBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\Interface Background.png"))
            };
            this.Background = myBrush;

            //Intialize a new settings window instance
            sw = new SettingsWindow();

            //Hide currently inactive elements and labels
            listBox.Visibility = Visibility.Hidden;
            listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            image2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            //Taskbar.Hide();

            //Initialize flags and a new class variable for GameInfo 
            _gameInfo = new GameInfo();
            _gameRunning = false;

            //Hook all required hotkeys
            InitilizeGhkHook();

            //Initialize license flag
            label3.Content = "Unlicensed Version";
            if (Program.IsLicenseValid)
            {
                label3.Visibility = Visibility.Hidden;
            }

            //Refresh the list of currently active consoles
            RefreshConsoleList();

            //If payPerPlay setting is activated, display a notification in the main GUI
            if (SettingsWindow.PayPerPlayEnabled > 0)
            {
                if (SettingsWindow.CoinsRequired > 0)
                {
                    DisplayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.CoinsRequired + " Current: " + Program.CoinsRequired);
                }
            }
            else
            {
                label2.Visibility = Visibility.Hidden;
            }

            //Refresh the current gamecount and update the GUI
            FileOps.RefreshGameCount();
            label.Content = "Total Game Count: " + Program.Database.TotalGameCount;
            UpdateGUI();
        }

        /// <summary>
        /// Refresh the list of currently active consoles
        /// </summary>
        public static void RefreshConsoleList()
        {
            _consoleList = new ArrayList();
            _consoleCount = 0;
            _index = 0;
            foreach (IConsole console in Program.Database.ConsoleList)
            {
                _consoleList.Add(console.ConsoleName);
                _consoleCount++;
            }
        }

        /// <summary>
        /// Mark the key event as handled on key up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Gkh_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Handle the key down event for hooked keys
        /// </summary>
        void Gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //If F10 is pressed, kill the currently running game process
            if (e.KeyCode == Keys.F10)
            {
                if (_gameRunning)
                {
                    ShowNotification("UniCade System", "Game Successfully Closed");
                    FileOps.KillProcess();
                    this.Activate();
                }
            }

            if ((!_gameRunning))
            {
                //If key I is pressed when a game is not active, display the game info window
                if (e.KeyCode == Keys.I)
                {
                    //Toggle info window visibility
                    if (_gameSelectionActive)
                    {
                        DisplayGameInfo();
                    }
                    else if (_infoWindowActive)
                    {
                        _gameInfo.Hide();
                        _infoWindowActive = false;
                    }
                }
            }

            if ((!_gameRunning) && (!_infoWindowActive))
            {
                //If left arrow is pressed within the GUI home page, navigate to the left
                if (e.KeyCode == Keys.Left)
                {
                    if (!_gameSelectionActive)
                    {
                        Left();
                    }
                }
                //If right arrow is pressed within the GUI home page, navigate to the right
                else if (e.KeyCode == Keys.Right)
                {
                    if (!_gameSelectionActive)
                    {
                        Right();
                    }
                }
                //If enter is pressed within the GUI home page, open the game library for the selected console
                //if a game is highlighted, launch the game
                else if (e.KeyCode == Keys.Enter)
                {
                    if (_gameSelectionActive)
                    {
                        LaunchGame();
                    }
                    else
                    {
                        OpenGameSelection();
                        _gameSelectionActive = true;
                    }
                }

                //If space is pressed within the game library page, toggle the selected game from the global favorites
                else if (e.KeyCode == Keys.Space)
                {
                    if (_gameSelectionActive)
                    {
                        if (listBox.SelectedItem != null)
                        {
                            foreach (IGame g in _gameSelectionConsole.GameList)
                            {
                                if (listBox.SelectedItem.ToString().Equals(g.Title))
                                {
                                    if (SettingsWindow.CurrentUser.Favorites.Count < 1)
                                    {
                                        SettingsWindow.CurrentUser.Favorites.Add(g);
                                        ShowNotification("UniCade", SettingsWindow.CurrentUser.Username + " :Added To Favorites");
                                        return;
                                    }
                                    foreach (IGame game1 in SettingsWindow.CurrentUser.Favorites)
                                    {
                                        if (game1.Title.Equals(g.Title) && g.ConsoleName.Equals(game1.ConsoleName))
                                        {
                                            SettingsWindow.CurrentUser.Favorites.Add(g);
                                            ShowNotification("UniCade", SettingsWindow.CurrentUser.Username + ": Removed From Favorites");
                                        }
                                        else
                                        {
                                            SettingsWindow.CurrentUser.Favorites.Add(g);
                                            ShowNotification("UniCade", SettingsWindow.CurrentUser.Username + ": Added To Favorites");
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                //If the game library window is active, add or remove the current game selection from global favorites
                else if (e.KeyCode == Keys.G)  
                {
                    if (_gameSelectionActive)
                    {
                        foreach (IGame game in _gameSelectionConsole.GameList)
                        {
                            if (listBox.SelectedItem.ToString().Equals(game.Title))
                            {
                                if (game.Favorite > 0)
                                {
                                    game.Favorite = 0;
                                    ShowNotification("UniCade", "Removed From Global Favorites");
                                }
                                else
                                {
                                    game.Favorite = 1;
                                    ShowNotification("UniCade", "Added To Global Favorites");
                                }
                            }
                        }
                    }
                }
                //If the game library window is active, toggle the favorites filter view
                else if (e.KeyCode == Keys.F)  
                {
                    if (_gameSelectionActive)
                    {
                        if (_favorite)
                        {
                            _favorite = false;
                        }
                        else
                        {
                            _favorite = true;
                        }

                        OpenGameSelection();
                    }
                }
                //Exit the Program
                else if ((e.KeyCode == Keys.C) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    System.Windows.Application.Current.Shutdown();
                }

                //Launch the settings window
                else if ((e.KeyCode == Keys.P) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)  //Display preferences window
                {
                    if (SettingsWindow.PasswordProtection > 0)
                    {
                        _validPAss = false;
                        PassWindow pw = new PassWindow();
                        pw.ShowDialog();

                        if (_validPAss)
                        {
                            sw = new SettingsWindow();
                            UnhookKeys();
                            _settingsWindowActive = true;
                            sw.ShowDialog();
                        }
                    }
                    else
                    {
                        sw = new SettingsWindow();
                        _settingsWindowActive = true;
                        UnhookKeys();
                        sw.ShowDialog();
                    }
                    if (Program.IsLicenseValid)
                    {
                        label3.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        label3.Visibility = Visibility.Visible;
                    }
                }
            }

            //Toggle expanded images view within the game info page
            if (!_gameRunning && _infoWindowActive)
            {
                if (e.KeyCode == Keys.F)
                {
                    _gameInfo.expand();
                }
                else if (e.KeyCode == Keys.B)
                {
                    _gameInfo.expand1();
                }
                else if (e.KeyCode == Keys.S)
                {
                    _gameInfo.expand2();
                }
                else if (e.KeyCode == Keys.E)
                {
                    _gameInfo.expand3();
                }
            }

            //Close the current window unless you are already on the home page
            if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))  
            {
                if (_infoWindowActive)
                {
                    _gameInfo.Hide();
                    _infoWindowActive = false;
                }
                else
                {
                    //Close Game Selection window
                    listBox.Visibility = Visibility.Hidden;  
                    image2.Visibility = Visibility.Hidden;
                    label1.Visibility = Visibility.Hidden;
                    image.Visibility = Visibility.Visible;
                    image1.Visibility = Visibility.Visible;

                    //Restore the flags for the main GUI view
                    _gameSelectionActive = false;
                    label.Content = "Total Game Count: " + Program.Database.TotalGameCount;
                }
            }

            // Insert coin
            else if (e.KeyCode == Keys.Tab)  
            {
                Program.CoinsRequired++;
                if (SettingsWindow.PayPerPlayEnabled > 0)
                {
                    if (SettingsWindow.CoinsRequired > 0)
                    {
                        label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.CoinsRequired + " Current: " + Program.CoinsRequired;
                    }

                    //Display a popup payPerPlay notification
                    ShowNotification("Pay Per Play", "Coin Inserted\n Current: " + Program.CoinsRequired);
                }
                else
                {
                    ShowNotification("UniCade", "Free Play Enabled. NO COIN REQUIRED");
                }
            }
            UpdateGUI();
        }

        /// <summary>
        /// Navigate to the right on the home console selection screen
        /// </summary>
        private void Right()
        {
            if (_index < (_consoleCount - 1))
            {
                _index++;
            }
            else
            {
                _index = 0;
            }

            UpdateGUI();
        }

        /// <summary>
        /// Navigate to the right on the home console selection screen
        /// </summary>
        private new void Left()
        {
            if (_index > 0)
            {
                _index--;
            }
            else
            {
                _index = (_consoleCount - 1);
            }

            UpdateGUI();
        }

        /// <summary>
        /// Update the info on the primary GUI
        /// </summary>
        private void UpdateGUI()
        {
            //Update payPerPlay notifications
            if (SettingsWindow.PayPerPlayEnabled > 0)
            {
                if (SettingsWindow.CoinsRequired > 0)
                {
                    label2.Content = "(PayPerPlay) Coins Per Launch: " + SettingsWindow.CoinsRequired + " Current: " + Program.CoinsRequired;
                }
                else
                {
                    label2.Visibility = Visibility.Hidden;
                }
            }

            //Update current console position
            _currentConsole = (string)_consoleList[_index];

            //Display the image for the currently selected console
            if ((File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + _consoleList[_index] + ".png")) && (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + _consoleList[_index] + " Logo" + ".png")))
            {
                //Display the console image
                label1.Visibility = Visibility.Hidden;
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + _consoleList[_index] + ".png");
                b.EndInit();
                image.Source = b;

                //Display the console logo
                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + _consoleList[_index] + " Logo" + ".png");
                b.EndInit();
                image1.Source = b;
            }
            else
            {
                //If the console image does not exist, display a notification
                image.Source = null;
                image1.Source = null;
                label1.Content = _consoleList[_index] + " Missing Console Image/Logo ";
                label1.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Opens the game library page for the currently selected console
        /// </summary>
        private void OpenGameSelection()
        {
            _gameSelectionActive = true;
            image.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;

            //Check if the favorites view filter is enabled
            if (!_favorite)
            {
                label1.Content = (_consoleList[_index] + " Library");
            }
            else
            {
                label1.Content = SettingsWindow.CurrentUser.Username + "'s Favorites List";
            }

            //Populate the game library 
            listBox.Items.Clear();
            foreach (IConsole console in Program.Database.ConsoleList)
            {
                if (console.ConsoleName.Equals(_consoleList[_index]))
                {
                    _gameSelectionConsole = console;
                    label.Content = console.ConsoleName + " Game Count: " + console.GameCount;

                    foreach (IGame game in console.GameList)
                    {
                        //Check if the global favorites filter is enabled
                        if (_favorite)
                        {
                            foreach (IGame game1 in SettingsWindow.CurrentUser.Favorites)
                            {
                                if (game.Title.Equals(game1.Title) && game.ConsoleName.Equals(game1.ConsoleName))
                                {
                                    //Add the game if it is present in the favorites list
                                    listBox.Items.Add(game.Title);
                                    break;
                                }
                            }
                        }

                        else
                        {
                            //Filter the viewable games if the restrict esrb view filter is enabled
                            if (SettingsWindow.DisplayEsrbWhileBrowsing > 0)
                            {
                                //Display the game if it has an allowed ESRB rating
                                if (SettingsWindow.CalcEsrb(game.EsrbRating) <= SettingsWindow.CalcEsrb(SettingsWindow.CurrentUser.AllowedEsrb))
                                {
                                    listBox.Items.Add(game.Title);
                                }
                            }
                            else
                            {
                                //Add the game regardless if the view ESRB 
                                listBox.Items.Add(game.Title);
                            }
                        }
                    }
                }
            }

            //Update the console image within the game library page
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + _consoleList[_index] + " Logo" + ".png"))
            {
                //Load the console image
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + _consoleList[_index] + " Logo" + ".png");
                b.EndInit();
                image2.Source = b;

                //Populate the title label at the top of the page
                label1.Content = (_consoleList[_index] + "Library");
            }
            else
            {
                image.Source = null;
            }

            listBox.Visibility = Visibility.Visible;
            if (listBox.Items.Count > 0)               
            {
                listBox.SelectedIndex = 0;
                ListBoxItem item = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromIndex(0);
                Keyboard.Focus(listBox);
                Keyboard.Focus(item);
            }
        }

        /// <summary>
        /// Launch the currently selected games using the specified emulator
        /// </summary>
        private void LaunchGame()
        {
            if (SettingsWindow.PayPerPlayEnabled > 0)
            {
                if (SettingsWindow.CoinsRequired > 0)
                {
                    if (Program.CoinsRequired < SettingsWindow.CoinsRequired) { 
                        ShowNotification("Pay Per Play", "Insert Coins");
                        return;
                    }
                }
            }

            Program.CoinsRequired = Program.CoinsRequired - SettingsWindow.CoinsRequired;
                    DisplayPayNotification("(PayPerPlay) Coins Per Launch: " + SettingsWindow.CoinsRequired + " Current: " + Program.CoinsRequired);

            //Search for the selected game title within the game library
            foreach (IGame game in _gameSelectionConsole.GameList)
            {
                if (listBox.SelectedItem.ToString().Equals(game.Title))
                {
                    //If the specified game is found, launch the game and return
                    Task.Delay(3000);
                    FileOps.Launch(game);
                    return;
                }
            }
        }

        /// <summary>
        /// Display detailed info for the current game
        /// </summary>
        private void DisplayGameInfo()
        {
            //Check for bad input or an empty game library
            if (listBox.SelectedItem == null)
            {
                return;
            }

            _infoWindowActive = true;
            BitmapImage bitmapImage;

            foreach (IGame game in _gameSelectionConsole.GameList)
            {
                if (listBox.SelectedItem.ToString().Equals(game.Title))
                {
                    //Populate the game info text
                    _gameInfo.textBlock1.Text = game.ConsoleName + " - " + game.Title;
                    _gameInfo.textBlock.Text = Program.DisplayGameInfo(game);

                    //Load the box front for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png");
                        bitmapImage.EndInit();
                        _gameInfo.image.Source = bitmapImage;
                    }

                    //Load the box back image for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png");
                        bitmapImage.EndInit();
                        _gameInfo.image1.Source = bitmapImage;
                    }

                    //Load the screenshot for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + _gameSelectionConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png");
                        bitmapImage.EndInit();
                        _gameInfo.image2.Source = bitmapImage;
                    }

                    //Load the ESRB logo for the curent rating
                    String EsrbPath = "";
                    if (game.EsrbRating.Equals("Everyone"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png";
                    }
                    else if (game.EsrbRating.Equals("Everyone (KA)"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png";
                    }
                    else if (game.EsrbRating.Equals("Everyone 10+"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png";
                    }
                    else if (game.EsrbRating.Equals("Teen"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png";
                    }
                    else if (game.EsrbRating.Equals("Mature"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png";
                    }
                    else if (game.EsrbRating.Equals("Adults Only (AO)"))
                    {
                        EsrbPath = Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png";
                    }

                    if (EsrbPath.Length > 2)
                    {
                        _gameInfo.displayEsrb(EsrbPath);
                    }
                }
            }

            //Display the game info
            _gameInfo.Show();
        }

        #region Helper Methods

        /// <summary>
        /// Define the keys to hook
        /// </summary>
        private void InitilizeGhkHook()
        {
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
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(Gkh_KeyDown);
            gkh.KeyUp += new System.Windows.Forms.KeyEventHandler(Gkh_KeyUp);
        }

        /// <summary>
        /// Re-hook specified global keyboard hook hotkeys
        /// </summary>
        public static void ReHookKeys()
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

        /// <summary>
        /// Unhook specified global hotkeys when lunching
        /// </summary>
        public static void UnhookKeys()
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

        /// <summary>
        /// Display a timed popup notification within the main GUI
        /// </summary>
        private void ShowNotification(string title, string body)
        {
            NotificationWindow nfw = new NotificationWindow(title, body);
            nfw.Show();
        }

        /// <summary>
        /// Display a fide payPerPlay notification within the main GUI
        /// </summary>
        private void DisplayPayNotification(String s)
        {
            label2.Content = s;
        }

        #endregion
    }
}

