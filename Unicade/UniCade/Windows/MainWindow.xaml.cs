using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Exceptions;
using UniCade.Interfaces;
using UniCade.Windows;

// ReSharper disable once CheckNamespace
namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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
        /// Specifies if the ESRB logo should be displayed while browsing games
        /// </summary>
        public static bool DisplayEsrbWhileBrowsing;

        /// <summary>
        /// Specifies if games with a restricted ESRB rating should be hidden 
        /// </summary>
        public static bool HideRestrictedEsrbGames;

        /// <summary>
        /// The current GlobalKeyboardHook object
        /// </summary>
        internal static GlobalKeyboardHook KeyboardHook;

        /// <summary>
        /// The current SettingsWindow object
        /// </summary>
        public static SettingsWindow SettingsWindow;

        #endregion

        #region  Private Instance Variables

        /// <summary>
        /// The current GameInfoWindow instance
        /// </summary>
        private static GameInfo _gameInfoWindow;

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
            //Intialize a new settings window instance
            SettingsWindow = new SettingsWindow();

            //Hide currently inactive elements and labels
            ListBox.Visibility = Visibility.Hidden;
            ListBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            ListBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            Image2.Visibility = Visibility.Hidden;
            Label1.Visibility = Visibility.Hidden;
            //Taskbar.Hide();

            //Initialize flags and a new class variable for GameInfo 
            _gameInfoWindow = new GameInfo();
            IsGameRunning = false;

            //Hook all required hotkeys
            InitilizeGhkHook();

            //Initialize license flag
            Label3.Content = "Unlicensed Version";
            if (Program.IsLicenseValid)
            {
                Label3.Visibility = Visibility.Hidden;
            }

            //Refresh the list of currently active consoles
            RefreshConsoleList();

            //If payPerPlay setting is activated, display a notification in the main GUI
            if (PayPerPlay.PayPerPlayEnabled)
            {
                if (PayPerPlay.CoinsRequired > 0)
                {
                    DisplayPayNotification("(PayPerPlay) Coins Per Launch: " + PayPerPlay.CoinsRequired + " Current: " + PayPerPlay.CurrentCoins);
                }
            }
            else
            {
                Label2.Visibility = Visibility.Hidden;
            }

            //Refresh the current gamecount and update the GUI
            //Program.RefreshTotalGameCount();
            Label.Content = "Total Game Count: " + Database.GetTotalGameCount();
            UpdateGui();
        }

        /// <summary>
        /// Refresh the list of currently active consoles
        /// </summary>
        public static void RefreshConsoleList()
        {
            ActiveConsoleList = new ArrayList();
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
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
                    Activate();
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
                        _gameInfoWindow.Hide();
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
                        if (ListBox.SelectedItem != null)
                        {
                            var gameList = CurrentConsole.GetGameList();
                            foreach (string gameTitle in gameList)
                            {
                                IGame g = CurrentConsole.GetGame(gameTitle);
                                if (ListBox.SelectedItem.ToString().Equals(g.Title))
                                {
                                    if (Database.GetCurrentUser().GetFavoritesList().Count < 1)
                                    {
                                        Database.GetCurrentUser().AddFavorite(g);
                                        ShowNotification("UniCade", Database.GetCurrentUser().Username + " :Added To Favorites");
                                        return;
                                    }
                                    foreach (IGame game1 in Database.GetCurrentUser().GetFavoritesList())
                                    {
                                        if (game1.Title.Equals(g.Title) && g.ConsoleName.Equals(game1.ConsoleName))
                                        {
                                            Database.GetCurrentUser().AddFavorite(g);
                                            ShowNotification("UniCade", Database.GetCurrentUser().Username + ": Removed From Favorites");
                                        }
                                        else
                                        {
                                            Database.GetCurrentUser().AddFavorite(g);
                                            ShowNotification("UniCade", Database.GetCurrentUser().Username + ": Added To Favorites");
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
                        var gameList = CurrentConsole.GetGameList();
                        foreach (string gameTitle in gameList)
                        {
                            IGame game = CurrentConsole.GetGame(gameTitle);
                            if (ListBox.SelectedItem.ToString().Equals(game.Title))
                            {
                                if (game.Favorite)
                                {
                                    game.Favorite = false;
                                    ShowNotification("UniCade", "Removed From Global Favorites");
                                }
                                else
                                {
                                    game.Favorite = true;
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
                        IsFavoritesViewActive = !IsFavoritesViewActive;

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
                    if (Program.PasswordProtection != null)
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
                    Label3.Visibility = Program.IsLicenseValid ? Visibility.Hidden : Visibility.Visible;
                }
            }

            //Toggle expanded images view within the game info page
            if (!IsGameRunning && IsInfoWindowActive)
            {
                if (e.KeyCode == Keys.F)
                {
                    _gameInfoWindow.ExpandImage1();
                }
                else if (e.KeyCode == Keys.B)
                {
                    _gameInfoWindow.ExpandImage2();
                }
                else if (e.KeyCode == Keys.S)
                {
                    _gameInfoWindow.ExpandImage3();
                }
            }

            //Close the current window unless you are already on the home page
            if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                if (IsInfoWindowActive)
                {
                    _gameInfoWindow.Hide();
                    IsInfoWindowActive = false;
                }
                else
                {
                    //Close Game Selection window
                    ListBox.Visibility = Visibility.Hidden;
                    Image2.Visibility = Visibility.Hidden;
                    Label1.Visibility = Visibility.Hidden;
                    Image.Visibility = Visibility.Visible;
                    Image1.Visibility = Visibility.Visible;

                    //Restore the flags for the main GUI view
                    IsGameSelectionPageActive = false;
                    Label.Content = "Total Game Count: " + Database.GetTotalGameCount();
                }
            }

            // Insert coin
            else if (e.KeyCode == Keys.Tab)
            {
                PayPerPlay.CoinsRequired++;
                if (PayPerPlay.PayPerPlayEnabled)
                {
                    if (PayPerPlay.CoinsRequired > 0)
                    {
                        Label2.Content = "(PayPerPlay) Coins Per Launch: " + PayPerPlay.CoinsRequired + " Current: " + PayPerPlay.CoinsRequired;
                    }

                    //Display a popup payPerPlay notification
                    ShowNotification("Pay Per Play", "Coin Inserted\n Current: " + PayPerPlay.CoinsRequired);
                }
                else
                {
                    ShowNotification("UniCade", "Free Play Enabled. NO COIN REQUIRED");
                }
            }
            UpdateGui();
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

            UpdateGui();
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

            UpdateGui();
        }

        /// <summary>
        /// Update the info on the primary GUI
        /// </summary>
        private void UpdateGui()
        {
            //Update payPerPlay notifications
            if (PayPerPlay.PayPerPlayEnabled)
            {
                if (PayPerPlay.CoinsRequired > 0)
                {
                    Label2.Content = "(PayPerPlay) Coins Per Launch: " + PayPerPlay.CoinsRequired + " Current: " + PayPerPlay.CurrentCoins;
                }
                else
                {
                    Label2.Visibility = Visibility.Hidden;
                }
            }

            //Display the image for the currently selected console
            if ((File.Exists(Directory.GetCurrentDirectory() + ConstValues.ConsoleImagesPath + ActiveConsoleList[IndexNumber] + ".png")) && (File.Exists(Directory.GetCurrentDirectory() + ConstValues.ConsoleLogoImagesPath + ActiveConsoleList[IndexNumber] + ".png")))
            {
                //Display the console image
                Label1.Visibility = Visibility.Hidden;
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + ConstValues.ConsoleImagesPath + ActiveConsoleList[IndexNumber] + ".png");
                b.EndInit();
                Image.Source = b;

                //Display the console logo
                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(Directory.GetCurrentDirectory() + ConstValues.ConsoleLogoImagesPath + ActiveConsoleList[IndexNumber] + ".png");
                b.EndInit();
                Image1.Source = b;
            }
            else
            {
                //If the console image does not exist, display a notification
                Image.Source = null;
                Image1.Source = null;
                Label1.Content = ActiveConsoleList[IndexNumber] + " Missing Console Image/Logo ";
                Label1.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Opens the game library page for the currently selected console
        /// </summary>
        private void OpenGameSelection()
        {
            IsGameSelectionPageActive = true;
            Image.Visibility = Visibility.Hidden;
            Image1.Visibility = Visibility.Hidden;
            Image2.Visibility = Visibility.Visible;
            Label1.Visibility = Visibility.Visible;

            //Check if the favorites view filter is enabled
            if (!IsFavoritesViewActive)
            {
                Label1.Content = (ActiveConsoleList[IndexNumber] + " Library");
            }
            else
            {
                Label1.Content = Database.GetCurrentUser().Username + "'s Favorites List";
            }

            //Populate the game library 
            ListBox.Items.Clear();
            var consoleList = Database.GetConsoleList();
            foreach (string consoleName in consoleList)
            {
                IConsole console = Database.GetConsole(consoleName);
                if (console.ConsoleName.Equals(ActiveConsoleList[IndexNumber]))
                {
                    CurrentConsole = console;
                    Label.Content = console.ConsoleName + " Game Count: " + console.GetGameCount();

                    var gameList = console.GetGameList();
                    foreach (string gameTitle in gameList)
                    {
                        IGame game = console.GetGame(gameTitle);
                        //Check if the global favorites filter is enabled
                        if (IsFavoritesViewActive)
                        {
                            foreach (IGame game1 in Database.GetCurrentUser().GetFavoritesList())
                            {
                                if (game.Title.Equals(game1.Title) && game.ConsoleName.Equals(game1.ConsoleName))
                                {
                                    //Add the game if it is present in the favorites list
                                    ListBox.Items.Add(game.Title);
                                    break;
                                }
                            }
                        }

                        else
                        {
                            //Filter the viewable games if the restrict esrb view filter is enabled
                            if (HideRestrictedEsrbGames)
                            {
                                //Display the game if it has an allowed ESRB rating
                                if (game.EsrbRating <= Database.GetCurrentUser().AllowedEsrbRatings)
                                {
                                    ListBox.Items.Add(game.Title);
                                }
                            }
                            else
                            {
                                //Add the game regardless if the view ESRB 
                                ListBox.Items.Add(game.Title);
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
                Image2.Source = b;

                //Populate the title label at the top of the page
                Label1.Content = (ActiveConsoleList[IndexNumber] + "Library");
            }
            else
            {
                Image.Source = null;
            }

            ListBox.Visibility = Visibility.Visible;
            if (ListBox.Items.Count > 0)
            {
                ListBox.SelectedIndex = 0;
                ListBoxItem item = (ListBoxItem)ListBox.ItemContainerGenerator.ContainerFromIndex(0);
                Keyboard.Focus(ListBox);
                Keyboard.Focus(item);
            }
        }

        /// <summary>
        /// Launch the currently selected games using the specified emulator
        /// </summary>
        private void LaunchGame()
        {

            //Search for the selected game title within the game library
            IGame game = CurrentConsole.GetGame(ListBox.SelectedItem.ToString());

            //If the specified game is found, launch the game and return
            try
            {
                FileOps.Launch(game);
            }
            catch(LaunchException e)
            {
                ShowNotification("Launch Error", e.Message);
            }
            if (PayPerPlay.PayPerPlayEnabled)
            {
                DisplayPayNotification("(PayPerPlay) Coins Per Launch: " + PayPerPlay.CoinsRequired + " Current: " + PayPerPlay.CurrentCoins);
            }
        }

        /// <summary>
        /// Display detailed info for the current game
        /// </summary>
        private void DisplayGameInfo()
        {
            //Check for bad input or an empty game library
            if (ListBox.SelectedItem == null)
            {
                return;
            }

            IsInfoWindowActive = true;

            var gameList = CurrentConsole.GetGameList();
            foreach (string gameTitle in gameList)
            {
                IGame game = CurrentConsole.GetGame(gameTitle);
                if (ListBox.SelectedItem.ToString().Equals(game.Title))
                {
                    //Populate the game info text
                    _gameInfoWindow.TextBlock1.Text = game.ConsoleName + " - " + game.Title;
                    _gameInfoWindow.TextBlock.Text = GameInfo.DisplayGameInfo(game);

                    //Load the box front for the current game if it exists
                    BitmapImage bitmapImage;
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxFront.png");
                        bitmapImage.EndInit();
                        _gameInfoWindow.Image.Source = bitmapImage;
                    }

                    //Load the box back image for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_BoxBack.png");
                        bitmapImage.EndInit();
                        _gameInfoWindow.Image1.Source = bitmapImage;
                    }

                    //Load the screenshot for the current game if it exists
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png"))
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Media\Games\" + CurrentConsole.ConsoleName + "\\" + game.Title + "_Screenshot.png");
                        bitmapImage.EndInit();
                        _gameInfoWindow.Image2.Source = bitmapImage;
                    }

                    //Load the ESRB logo for the curent rating
                    _gameInfoWindow.DisplayEsrb(game);
                }
            }

            //Display the game info
            _gameInfoWindow.Show();
        }

        #region Helper Methods

        /// <summary>
        /// Define the keys to hook
        /// </summary>
        private void InitilizeGhkHook()
        {
            KeyboardHook = new GlobalKeyboardHook();
            KeyboardHook.KeyDown += Gkh_KeyDown;
            KeyboardHook.KeyUp += Gkh_KeyUp;
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
        public void DisplayPayNotification(String s)
        {
            Label2.Content = s;
        }

        #endregion
    }
}

