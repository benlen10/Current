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
using UniCade.Backend;
using UniCade.Windows;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        /// <summary>
        /// The list of currently active consoles
        /// </summary>
        public static ArrayList ActiveConsoleList;

        /// <summary>
        /// The index number for the currently displayed console
        /// </summary>
        public static int IndexNumber;

        /// <summary>
        /// True if the password entered in the passWindow is valid
        /// </summary>
        public static bool IsPasswordValid;

        /// <summary>
        /// True if the game selection screen is the current screen
        /// </summary>
        public static bool IsGameSelectionPageActive;

        /// <summary>
        /// True if a game is currently running
        /// </summary>
        public static bool IsGameRunning;

        /// <summary>
        /// True if the game info window is currently active
        /// </summary>
        public static bool IsInfoWindowActive;

        /// <summary>
        /// The current console that is selected
        /// </summary>
        public IConsole CurrentConsole;

        /// <summary>
        /// True if the SettingsWindow is currently visible
        /// </summary>
        public static bool IsSettingsWindowActive;

        /// <summary>
        /// True if currenly only favorites are being displayed
        /// </summary>
        public static bool IsFavoritesViewActive;

        /// <summary>
        /// The current GlobalKeyboardHook object
        /// </summary>
        public static GlobalKeyboardHook KeyboardHook;

        /// <summary>
        /// The current SettingsWindow object
        /// </summary>
        public static SettingsWindow SettingsWindow;

        /// <summary>
        /// The current GameInfoWindow instance
        /// </summary>
        private static GameInfo GameInfoWindow;

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
            SettingsWindow = new SettingsWindow();
            Program.SettingsWindow = SettingsWindow;

            //Hide currently inactive elements and labels
            listBox.Visibility = Visibility.Hidden;
            listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            image2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            //Taskbar.Hide();

            //Initialize flags and a new class variable for GameInfo 
            GameInfoWindow = new GameInfo();
            IsGameRunning = false;

            //Hook all required hotkeys
            InitilizeGhkHook();

            //Initialize license flag
            label3.Content = "Unlicensed Version";
            if (LicenseEngine.IsLicenseValid)
            {
                label3.Visibility = Visibility.Hidden;
            }

            //Refresh the list of currently active consoles
            RefreshConsoleList();

            //If payPerPlay setting is activated, display a notification in the main GUI
            if (SettingsWindow.PayPerPlayEnabled == true)
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
            //Program.RefreshTotalGameCount();
            label.Content = "Total Game Count: " + Program.TotalGameCount;
            UpdateGUI();
        }

        /// <summary>
        /// Refresh the list of currently active consoles
        /// </summary>
        public static void RefreshConsoleList()
        {
            ActiveConsoleList = new ArrayList();
            foreach (IConsole console in Program.ConsoleList)
            {
                ActiveConsoleList.Add(console.ConsoleName);
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
        public void Gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //If F10 is pressed, kill the currently running game process
            if (e.KeyCode == Keys.F10)
            {
                if (IsGameRunning)
                {
                    ShowNotification("UniCade System", "Game Successfully Closed");
                    FileOps.KillProcess();
                    this.Activate();
                }
            }

            if ((!IsGameRunning))
            {
                //If key I is pressed when a game is not active, display the game info window
                if (e.KeyCode == Keys.I)
                {
                    //Toggle info window visibility
                    if (IsGameSelectionPageActive)
                    {
                        DisplayGameInfo();
                    }
                    else if (IsInfoWindowActive)
                    {
                        GameInfoWindow.Hide();
                        IsInfoWindowActive = false;
                    }
                }
            }

            if ((!IsGameRunning) && (!IsInfoWindowActive))
            {
                //If left arrow is pressed within the GUI home page, navigate to the left
                if (e.KeyCode == Keys.Left)
                {
                    if (!IsGameSelectionPageActive)
                    {
                        Left();
                    }
                }
                //If right arrow is pressed within the GUI home page, navigate to the right
                else if (e.KeyCode == Keys.Right)
                {
                    if (!IsGameSelectionPageActive)
                    {
                        Right();
                    }
                }
                //If enter is pressed within the GUI home page, open the game library for the selected console
                //if a game is highlighted, launch the game
                else if (e.KeyCode == Keys.Enter)
                {
                    if (IsGameSelectionPageActive)
                    {
                        LaunchGame();
                    }
                    else
                    {
                        OpenGameSelection();
                        IsGameSelectionPageActive = true;
                    }
                }

                //If space is pressed within the game library page, toggle the selected game from the global favorites
                else if (e.KeyCode == Keys.Space)
                {
                    if (IsGameSelectionPageActive)
                    {
                        if (listBox.SelectedItem != null)
                        {
                            foreach (IGame g in CurrentConsole.GameList)
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
                    if (IsGameSelectionPageActive)
                    {
                        foreach (IGame game in CurrentConsole.GameList)
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
                    if (IsGameSelectionPageActive)
                    {
                        if (IsFavoritesViewActive)
                        {
                            IsFavoritesViewActive = false;
                        }
                        else
                        {
                            IsFavoritesViewActive = true;
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
                        IsPasswordValid = false;
                        PassWindow pw = new PassWindow();
                        pw.ShowDialog();

                        if (IsPasswordValid)
                        {
                            SettingsWindow = new SettingsWindow();
                            UnhookKeys();
                            IsSettingsWindowActive = true;
                            SettingsWindow.ShowDialog();
                        }
                    }
                    else
                    {
                        SettingsWindow = new SettingsWindow();
                        IsSettingsWindowActive = true;
                        UnhookKeys();
                        SettingsWindow.ShowDialog();
                    }
                    if (LicenseEngine.IsLicenseValid)
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
            if (!IsGameRunning && IsInfoWindowActive)
            {
                if (e.KeyCode == Keys.F)
                {
                    GameInfoWindow.ExpandImage1();
                }
                else if (e.KeyCode == Keys.B)
                {
                    GameInfoWindow.ExpandImage2();
                }
                else if (e.KeyCode == Keys.S)
                {
                    GameInfoWindow.ExpandImage3();
                }
                else if (e.KeyCode == Keys.E)
                {
                    GameInfoWindow.ExpandImage4();
                }
            }

            //Close the current window unless you are already on the home page
            if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))  
            {
                if (IsInfoWindowActive)
                {
                    GameInfoWindow.Hide();
                    IsInfoWindowActive = false;
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
                    IsGameSelectionPageActive = false;
                    label.Content = "Total Game Count: " + Program.TotalGameCount;
                }
            }

            // Insert coin
            else if (e.KeyCode == Keys.Tab)  
            {
                Program.CoinsRequired++;
                if (SettingsWindow.PayPerPlayEnabled == true)
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
            if (IndexNumber < (ActiveConsoleList.Count - 1))
            {
                IndexNumber++;
            }
            else
            {
                IndexNumber = 0;
            }

            UpdateGUI();
        }

        /// <summary>
        /// Navigate to the right on the home console selection screen
        /// </summary>
        private new void Left()
        {
            if (IndexNumber > 0)
            {
                IndexNumber--;
            }
            else
            {
                IndexNumber = (ActiveConsoleList.Count - 1);
            }

            UpdateGUI();
        }

        /// <summary>
        /// Update the info on the primary GUI
        /// </summary>
        private void UpdateGUI()
        {
            //Update payPerPlay notifications
            if (SettingsWindow.PayPerPlayEnabled == true)
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

            //Display the image for the currently selected console
            if ((File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + ActiveConsoleList[IndexNumber] + ".png")) && (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + ActiveConsoleList[IndexNumber] + " Logo" + ".png")))
            {
                //Display the console image
                label1.Visibility = Visibility.Hidden;
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\" + ActiveConsoleList[IndexNumber] + ".png");
                b.EndInit();
                image.Source = b;

                //Display the console logo
                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + ActiveConsoleList[IndexNumber] + " Logo" + ".png");
                b.EndInit();
                image1.Source = b;
            }
            else
            {
                //If the console image does not exist, display a notification
                image.Source = null;
                image1.Source = null;
                label1.Content = ActiveConsoleList[IndexNumber] + " Missing Console Image/Logo ";
                label1.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Opens the game library page for the currently selected console
        /// </summary>
        private void OpenGameSelection()
        {
            IsGameSelectionPageActive = true;
            image.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;

            //Check if the favorites view filter is enabled
            if (!IsFavoritesViewActive)
            {
                label1.Content = (ActiveConsoleList[IndexNumber] + " Library");
            }
            else
            {
                label1.Content = SettingsWindow.CurrentUser.Username + "'s Favorites List";
            }

            //Populate the game library 
            listBox.Items.Clear();
            foreach (IConsole console in Program.ConsoleList)
            {
                if (console.ConsoleName.Equals(ActiveConsoleList[IndexNumber]))
                {
                    CurrentConsole = console;
                    label.Content = console.ConsoleName + " Game Count: " + console.GameCount;

                    foreach (IGame game in console.GameList)
                    {
                        //Check if the global favorites filter is enabled
                        if (IsFavoritesViewActive)
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
                            if (SettingsWindow.DisplayEsrbWhileBrowsing == true)
                            {
                                //Display the game if it has an allowed ESRB rating
                                if (game.EsrbRating <= SettingsWindow.CurrentUser.AllowedEsrb)
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
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + ActiveConsoleList[IndexNumber] + " Logo" + ".png"))
            {
                //Load the console image
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos\" + ActiveConsoleList[IndexNumber] + " Logo" + ".png");
                b.EndInit();
                image2.Source = b;

                //Populate the title label at the top of the page
                label1.Content = (ActiveConsoleList[IndexNumber] + "Library");
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
            if (SettingsWindow.PayPerPlayEnabled == true)
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
            foreach (IGame game in CurrentConsole.GameList)
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

            IsInfoWindowActive = true;
            BitmapImage bitmapImage;

            foreach (IGame game in CurrentConsole.GameList)
            {
                if (listBox.SelectedItem.ToString().Equals(game.Title))
                {
                    //Populate the game info text
                    GameInfoWindow.textBlock1.Text = game.ConsoleName + " - " + game.Title;
                    GameInfoWindow.textBlock.Text = Program.DisplayGameInfo(game);

                    //Load the box front for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png");
                        bitmapImage.EndInit();
                        GameInfoWindow.image.Source = bitmapImage;
                    }

                    //Load the box back image for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png");
                        bitmapImage.EndInit();
                        GameInfoWindow.image1.Source = bitmapImage;
                    }

                    //Load the screenshot for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png");
                        bitmapImage.EndInit();
                        GameInfoWindow.image2.Source = bitmapImage;
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
                        GameInfoWindow.DisplayEsrb(EsrbPath);
                    }
                }
            }

            //Display the game info
            GameInfoWindow.Show();
        }

        #region Helper Methods

        /// <summary>
        /// Define the keys to hook
        /// </summary>
        private void InitilizeGhkHook()
        {
            KeyboardHook = new GlobalKeyboardHook();
            KeyboardHook.KeyDown += new System.Windows.Forms.KeyEventHandler(Gkh_KeyDown);
            KeyboardHook.KeyUp += new System.Windows.Forms.KeyEventHandler(Gkh_KeyUp);
        }

        /// <summary>
        /// Unhook specified global hotkeys when lunching
        /// </summary>
        public static void UnhookKeys()
        {
            KeyboardHook.HookedKeys.Clear();
            KeyboardHook.HookedKeys.Add(Keys.F10);
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

