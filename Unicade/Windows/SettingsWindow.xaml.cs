using System;
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
using System.Windows.Shapes;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Properties

        public static Game _curGame;
        public static User _curUser;
        static Console _curConsole2;
        static Console _curConsole;
        public static string _defaultUser;
        public static int _showSplash;
        public static int _scanOnStartup;
        public static int _restrictESRB;
        public static int _requireLogin;
        public static int _cmdOrGui;
        public static int _showLoading;
        public static int _payPerPlay;
        public static int _coins;
        public static int _playtime;
        public static int _perLaunch;
        public static int _viewEsrb;
        public static int _passProtect;
        public static int _enforceExt;

        #endregion

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        #region Games Tab

        /// <summary>
        /// Download game button
        /// Download metadata for the selected game from UniCade Cloud
        /// </summary>
        private void GamesTab_DownloadGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Placeholder method
        /// </summary>
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Verify that a string contains only numeric chars
        /// </summary>
        bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Given the string value for an esrb rating, calculate and return the ESRB int value
        /// </summary>
        public static int CalcEsrb(String esrb)
        {
            int EsrbNum = 0;
            if (esrb.Equals("Everyone"))
                EsrbNum = 1;
            else if (esrb.Equals("Everyone 10+"))
                EsrbNum = 2;
            else if (esrb.Equals("Teen"))
                EsrbNum = 3;
            else if (esrb.Equals("Mature"))
                EsrbNum = 4;
            else if (esrb.Equals("Adults Only (AO)"))
                EsrbNum = 5;
            else if (esrb.Equals("None"))
                EsrbNum = 0;
            else
                EsrbNum = 0;
            return EsrbNum;
        }

        /// <summary>
        /// Refresh the current game info passed in as a Game object
        /// </summary>
        public void RefreshGameInfo(Game game)
        {
            /*if (game == null)
            {
                //If no game is currently selected, set all info fields to null
                textBox2.Text = null;
                textBox13.Text = null;
                textBox12.Text = null;
                textBox15.Text = null;
                textBox11.Text = null;
                textBox10.Text = null;
                textBox6.Text = null;
                textBox17.Text = null;
                textBox19.Text = null;
                textBox18.Text = null;
                return;
            }

            //If a valid game is selected, update all info fields
            textBox2.Text = game.Title;
            textBox13.Text = game.Console;
            textBox12.Text = game.ReleaseDate;
            textBox15.Text = game.CriticScore;
            textBox11.Text = game.Publisher;
            textBox10.Text = game.Developer;
            textBox6.Text = game.Esrb;
            textBox17.Text = game.Players;
            textBox19.Text = game.EsrbDescriptor;
            textBox18.Text = game.Description;

            //Set favorite checkbox
            if (game.Favorite == 1)
                checkBox3.Checked = true;
            else
                checkBox3.Checked = false;

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png"))
                pictureBox1.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxFront.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png"))
                pictureBox2.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_BoxBack.png");
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png"))
                pictureBox3.Load(Directory.GetCurrentDirectory() + @"\Media\Games\" + _curConsole2.Name + "\\" + game.Title + "_Screenshot.png");
    */   
    }

        /// <summary>
        /// Save the current game info to the database file
        /// Display an error popup if any of the inputs contain invalid data
        /// </summary>
        private void SaveGameInfo()
        {
            /*
            //Invalid input checks
            if (listBox3.Items.Count < 1)
                return;
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Must select a console");
                return;
            }
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    _curGame.ReleaseDate = textBox12.Text;
                else
                {
                    MessageBox.Show("Release Date Invalid");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Release Date score must be only digits");
                return;
            }
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength < 5)
                    _curGame.CriticScore = textBox15.Text;
                else
                    MessageBox.Show("Critic Score Invalid");
            }
            else
                MessageBox.Show("Critic Score must be only digits");
            if (IsAllDigits(textBox12.Text))
            {
                if (textBox12.TextLength > 2)
                    _curGame.Players = textBox17.Text;
                else
                    MessageBox.Show("Players Invalid");
            }
            else
                MessageBox.Show("Players must be only digits");
            if (textBox10.Text.Contains("|") || textBox11.Text.Contains("|") || textBox6.Text.Contains("|") || textBox18.Text.Contains("|") || textBox19.Text.Contains("|"))
                MessageBox.Show("Fields contain invalid character {|}\nNew data not saved.");
            else
            {
                if (textBox6.Text.Contains("Everyone") || textBox6.Text.Contains("Teen") || textBox6.Text.Contains("Mature") || textBox6.Text.Contains("Adults") || textBox6.TextLength < 1)
                    _curGame.Esrb = textBox6.Text;
                else
                    MessageBox.Show("Invalid ESRB Rating");
                if ((textBox10.Text.Length > 20) || (textBox11.Text.Length > 20) || (textBox18.Text.Length > 20) || (textBox19.Text.Length > 20))
                    MessageBox.Show("Invalid Length");
                else
                {
                    _curGame.Publisher = textBox11.Text;
                    _curGame.Developer = textBox10.Text;
                    _curGame.Description = textBox18.Text;
                    _curGame.EsrbDescriptor = textBox19.Text;
                }
            }

            //If all input fields are valid, save the database
            FileOps.saveDatabase(Program._databasePath);
            */
        }

        /// <summary>
        /// Refresh the ESRB rating icon to the current ESRB rating
        /// </summary>
        public void RefreshEsrbIcon(Game g)
        {
            /*
            pictureBox4.Image = null;
            if (g.Esrb.Equals("Everyone"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png");
            else if (g.Esrb.Equals("Everyone (KA)"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png");
            else if (g.Esrb.Equals("Everyone 10+"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png");
            else if (g.Esrb.Equals("Teen"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png");
            else if (g.Esrb.Equals("Mature"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png");
            if (g.Esrb.Equals("Adults Only (AO)"))
                pictureBox4.Load(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png");
            */
        }

        /// <summary>
        /// Refresh global favorites across all consoles and users
        /// </summary>
        public void RefreshGlobalFavs()
        {
            /*
            listBox6.Items.Clear();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.GameCount > 0)
                {
                    foreach (Game g in c.GameList)
                    {
                        if (g.Favorite > 0)
                        {
                            listBox6.Items.Add(g.Title + " (" + g.Console + ")");
                        }
                    }
                }
            }
            */
        }

        #endregion
    }
}
