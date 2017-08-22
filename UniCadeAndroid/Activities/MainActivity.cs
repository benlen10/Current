using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using UniCadeAndroid.Backend;
using System.Collections.Generic;
using System.Linq;
using UniCadeAndroid.Interfaces;

namespace UniCadeAndroid.Activities
{
    [Activity(Label = "UniCadeAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        #region Private Instance Variables 

        private Button _settingsButton;

        private Button _loginButton;

        private Button _infoButton;

        private CheckBox _showFavoritesCheckbox;

        private CheckBox _globalSearchCheckbox;

        private ListView _gameSelectionListView;

        private Spinner _consoleSelectionSpinner;

        private EditText _searchBarEditText;

        private ImageView _consoleImageView;

        private bool favoritesViewEnabled;

        #endregion

        #region Properties

        /// <summary>
        /// The currently selected IGame object
        /// </summary>
        public static IGame CurrentGame;

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Initalize the database, preform an initial scan and refresh the total game count
            Database.Initalize();

            //Validate the media directory and attempt to laod both the database.xml and preferences.xml files
            if (!FileOps.StartupScan())
            {
                return;
            }

            //Refresh the total came count across all consoles
            Database.RefreshTotalGameCount();

            SetContentView(Resource.Layout.MainView);

            FindElementsById();

            CreateHandlers();

            PopulateConsoleSpinner();
        }

        public void PopulateConsoleSpinner()
        {

            List<string> consoleList = Database.GetConsoleList().ToList();

            var consoleSpinnerAdapter =
                new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, consoleList);

            _consoleSelectionSpinner.Adapter = consoleSpinnerAdapter;
        }

        private void RefreshGameList()
        {
            var currentConsole = _consoleSelectionSpinner.SelectedItem.ToString();
            var gameList = new List<string>();
            if (favoritesViewEnabled)
            {
                 gameList = new List<string>(Database.GetConsole(currentConsole).GetFavoriteGameList());
            }
            else
            {
                gameList = new List<string>(Database.GetConsole(currentConsole).GetGameList());
            }
            var gameListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, gameList);
            _gameSelectionListView.Adapter = gameListAdapter;
        }

        private void SelectedGameChanged()
        {
            string consoleName = _consoleSelectionSpinner.SelectedItem.ToString();
            string gameName = _gameSelectionListView.SelectedItem.ToString();
            CurrentGame = Database.GetConsole(consoleName).GetGame(gameName);
        }

        private void FindElementsById()
        {
            _settingsButton = FindViewById<Button>(Resource.Id.SettingsButton);
            _loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            _infoButton = FindViewById<Button>(Resource.Id.InfoButton);
            _showFavoritesCheckbox = FindViewById<CheckBox>(Resource.Id.ShowFavoritesCheckbox);
            _globalSearchCheckbox = FindViewById<CheckBox>(Resource.Id.GlobalfavoritesCheckbox);
            _gameSelectionListView = FindViewById<ListView>(Resource.Id.GameSelectionListView);
            _consoleSelectionSpinner = FindViewById<Spinner>(Resource.Id.ConsoleTextView);
            _searchBarEditText = FindViewById<EditText>(Resource.Id.SearchBarEditTExt);
            _consoleImageView = FindViewById<ImageView>(Resource.Id.ConsoleImageView);
        }

        private void CreateHandlers()
        {
            _settingsButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
            };

            _loginButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };

            _infoButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GameInfoActivity));
                StartActivity(intent);
            };

            _consoleSelectionSpinner.ItemSelected += (sender, e) =>
            {
                RefreshGameList();
            };

            _gameSelectionListView.ItemSelected += (sender, e) =>
            {
                SelectedGameChanged();

            };

            _showFavoritesCheckbox.CheckedChange += (sender, e) =>
            {
                favoritesViewEnabled = _showFavoritesCheckbox.Checked;
            };

            _globalSearchCheckbox.CheckedChange += (sender, e) =>
            {

            };
        }
    }
}

