
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UniCadeAndroid.Activities
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        #region Private Instance Variables

        Button loadDatabaseButton;

        Button loadBackupButton;

        Button saveDatabaseButton;

        Button backupDatabaseButton;

        CheckBox loadDatabaseOnStartupCheckbox;

        CheckBox rescanLocalLibrariesOnStartupCheckbox;

        CheckBox displayConsoleLogoCheckbox;

        CheckBox displayEsrbLogoCheckbox;

        Button deleteAllLocalImagesButton;

        Button unicadeCloudButton;

        Button webScraperSettingsButton;

        EditText PasswordEditText;

        Button enterLicenseButton;

        Button applyButton;

        Button closeSettingsButton;

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the view
            SetContentView(Resource.Layout.SettingsView);

            FindElementsById();

            LinkClickHandlers();

        }

		void FindElementsById()
		{
            loadDatabaseButton = FindViewById<Button>(Resource.Id.LoadDatabaseButton);
            loadBackupButton = FindViewById<Button>(Resource.Id.LoadBackupButton);
            saveDatabaseButton = FindViewById<Button>(Resource.Id.SaveDatabaseButton);
            backupDatabaseButton = FindViewById<Button>(Resource.Id.BackupDatabaseButton);
            loadDatabaseOnStartupCheckbox = FindViewById<CheckBox>(Resource.Id.LoadDatabaseOnStartupCheckbox);
            rescanLocalLibrariesOnStartupCheckbox = FindViewById<CheckBox>(Resource.Id.RescanLocalLibrariesOnStartupCheckbox);
            displayConsoleLogoCheckbox = FindViewById<CheckBox>(Resource.Id.DisplayConsoleLogoCheckbox);
            displayEsrbLogoCheckbox = FindViewById<CheckBox>(Resource.Id.DisplayEsrbLogoCheckbox);
            deleteAllLocalImagesButton = FindViewById<Button>(Resource.Id.DeleteAllLocalImagesButton);
            unicadeCloudButton = FindViewById<Button>(Resource.Id.UniCadeCloudButton);
            webScraperSettingsButton = FindViewById<Button>(Resource.Id.WebScraperSettingsButton);
            PasswordEditText = FindViewById<EditText>(Resource.Id.PasswordEditText);
            enterLicenseButton = FindViewById<Button>(Resource.Id.EnterLicenseKeyButton);
            applyButton = FindViewById<Button>(Resource.Id.ApplyButton);
            closeSettingsButton = FindViewById<Button>(Resource.Id.CloseButton);
		}

		void LinkClickHandlers()
		{
            webScraperSettingsButton.Click += (sender, e) =>
			{
                var intent = new Intent(this, typeof(ScraperSettingsActivity));
				StartActivity(intent);
			};

			unicadeCloudButton.Click += (sender, e) =>
			{
                var intent = new Intent(this, typeof(LoginActivity));
				StartActivity(intent);
			};
		}

	}
}
