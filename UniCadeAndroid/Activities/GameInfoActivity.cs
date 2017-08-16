
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
    [Activity(Label = "GameInfoActivity")]
    public class GameInfoActivity : Activity
    {

		#region Private Instance Variables

        TextView titleTextView;

		TextView consoleTextView;

        TextView publisherTextView;

        TextView criticScoreTextView;

        TextView playersTextView;

        TextView esrbRatingTextView;

        TextView esrbDescriptorsTextView;

        TextView playersCountTextView;

        TextView releaseDateTextView;

        TextView descriptionTextView;

        Button rescrapeGameButton;

        Button rescrapeConsoleButton;

        Button saveInfoButton;

        Button closeInfoButton;

        Button refreshInfoButton;

        ImageView boxFrontImageView;

        ImageView boxBackImageView;

        ImageView screenshotImageView;

        ImageView esrbLogoImageView;

		#endregion

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			/// Set the view
			SetContentView(Resource.Layout.SettingsView);

			FindElementsById();

			LinkClickHandlers();
        }

        void FindElementsById()
        {
            titleTextView = FindViewById<TextView>(Resource.Id.TitleTextView);
            consoleTextView = FindViewById<TextView>(Resource.Id.ConsoleTextView);
            publisherTextView = FindViewById<TextView>(Resource.Id.PublisherTextView);
            criticScoreTextView = FindViewById<TextView>(Resource.Id.CriticScoreTextView);
            playersTextView = FindViewById<TextView>(Resource.Id.PlayersTextView);
            esrbRatingTextView = FindViewById<TextView>(Resource.Id.EsrbRatingTextView);
            esrbDescriptorsTextView = FindViewById<TextView>(Resource.Id.EsrbDescriptorsTextView);
            releaseDateTextView = FindViewById<TextView>(Resource.Id.ReleaseDateTextView);
            descriptionTextView = FindViewById<TextView>(Resource.Id.DescriptonTextView);
            rescrapeGameButton = FindViewById<Button>(Resource.Id.RescrapeGameButton);
            rescrapeConsoleButton = FindViewById<Button>(Resource.Id.RescrapeConsoleButton);
            saveInfoButton = FindViewById<Button>(Resource.Id.SaveButton);
            closeInfoButton = FindViewById<Button>(Resource.Id.CloseButton);
            refreshInfoButton = FindViewById<Button>(Resource.Id.RefreshButton);
            boxFrontImageView = FindViewById<ImageView>(Resource.Id.BoxFrontImageView);
            boxBackImageView = FindViewById<ImageView>(Resource.Id.BoxBackImageView);
            screenshotImageView = FindViewById<ImageView>(Resource.Id.ScreenshotImageView);
            esrbLogoImageView = FindViewById<ImageView>(Resource.Id.EsrbLogoImageView);
        }

        void LinkClickHandlers(){
            
        }
    }
}
