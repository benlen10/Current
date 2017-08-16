using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using UniCadeAndroid.Backend;
using UniCadeAndroid.Activities;

namespace UniCadeAndroid
{
    [Activity(Label = "UniCadeAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        #region Private Instance Variables 

        Button settingsButton;

        Button loginButton;

        Button infoButton;

        CheckBox showFavoritesCheckbox;

        CheckBox globalSearchCheckbox;

        ListView gameSelectionListView;

        Spinner consoleSelectionSpinner;

        EditText SearchBarEditText;

        ImageView consoleImageView;

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            /*
            //Initalize the database, preform an initial scan and refresh the total game count
            Database.Initalize();

            //Validate the media directory and attempt to laod both the database.xml and preferences.xml files
            if (!FileOps.StartupScan())
            {
                return;
            }

            //Refresh the total came count across all consoles
            Database.RefreshTotalGameCount();
            */
 
            SetContentView (Resource.Layout.MainView);

            FindElementsById();

            LinkClickHandlers();
        }

         void FindElementsById(){
			settingsButton = FindViewById<Button>(Resource.Id.SettingsButton);
			loginButton = FindViewById<Button>(Resource.Id.LoginButton);
			infoButton = FindViewById<Button>(Resource.Id.InfoButton);
            showFavoritesCheckbox = FindViewById<CheckBox>(Resource.Id.ShowFavoritesCheckbox);
            globalSearchCheckbox = FindViewById<CheckBox>(Resource.Id.GlobalfavoritesCheckbox);
            gameSelectionListView = FindViewById<ListView>(Resource.Id.GameSelectionListView);
            consoleSelectionSpinner = FindViewById<Spinner>(Resource.Id.ConsoleTextView);
            SearchBarEditText = FindViewById<EditText>(Resource.Id.SearchBarEditTExt);
            consoleImageView = FindViewById<ImageView>(Resource.Id.ConsoleImageView);
        }

        void LinkClickHandlers(){
			settingsButton.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(SettingsActivity));
				StartActivity(intent);
			};

			loginButton.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(SettingsActivity));
				StartActivity(intent);
			};

			infoButton.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(SettingsActivity));
				StartActivity(intent);
			};
            
        }

    }
}

