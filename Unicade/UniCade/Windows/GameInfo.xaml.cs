using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for GameInfo.xaml
    /// </summary>
    public partial class GameInfo
    {

        #region Private Instance Variables

        /// <summary>
        /// Set to True if image 1 is currently expanded
        /// </summary>
        private bool _isImageExpanded1;

        /// <summary>
        /// Set to True if image 2 is currently expanded
        /// </summary>
        private bool _isImageExpanded2;

        /// <summary>
        /// Set to True if image 3 is currently expanded
        /// </summary>
        private bool _isImageExpanded3;

        #endregion

        /// <summary>
        /// Public constructor for the GameInfo class
        /// </summary>
        public GameInfo()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        /// <summary>
        /// Display the icon for the current ESRB rating
        /// </summary>
        public void DisplayEsrb(string esrbRating)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(esrbRating);
            bitmapImage.EndInit();
            Image3.Source = bitmapImage;
        }

        /// <summary>
        /// Expand image 1
        /// </summary>
        public void ExpandImage1()
        {
            if (!_isImageExpanded2 && !_isImageExpanded3)
            {
                if (!_isImageExpanded1)
                {
                    Image.Width = 700;
                    Image.Height = 700;
                    TextBlock.Visibility = Visibility.Hidden;
                    Image1.Visibility = Visibility.Hidden;
                    Image2.Visibility = Visibility.Hidden;
                    _isImageExpanded1 = true;

                }
                else
                {
                    Image.Width = 180;
                    Image.Height = 180;
                    TextBlock.Visibility = Visibility.Visible;
                    Image1.Visibility = Visibility.Visible;
                    Image2.Visibility = Visibility.Visible;
                    _isImageExpanded1 = false;
                }
            }
        }

        /// <summary>
        /// Expand image 2
        /// </summary>
        public void ExpandImage2()
        {
            {
                if (!_isImageExpanded1 && !_isImageExpanded3)
                {
                    if (!_isImageExpanded2)
                    {
                        Image1.Width = 700;
                        Image1.Height = 700;
                        TextBlock.Visibility = Visibility.Hidden;
                        Image.Visibility = Visibility.Hidden;
                        Image2.Visibility = Visibility.Hidden;
                        Image3.Visibility = Visibility.Hidden;
                        _isImageExpanded2 = true;
                    }
                    else
                    {
                        Image1.Width = 180;
                        Image1.Height = 180;
                        TextBlock.Visibility = Visibility.Visible;
                        Image.Visibility = Visibility.Visible;
                        Image2.Visibility = Visibility.Visible;
                        Image3.Visibility = Visibility.Visible;
                        _isImageExpanded2 = false;
                    }
                }
            }
        }

        /// <summary>
        /// Expand image 3
        /// </summary>
        public void ExpandImage3()
        {
            {
                if (!_isImageExpanded2 && !_isImageExpanded1)
                {
                    if (!_isImageExpanded3)
                    {
                        Image2.Width = 700;
                        Image2.Height = 700;
                        TextBlock.Visibility = Visibility.Hidden;
                        Image.Visibility = Visibility.Hidden;
                        Image1.Visibility = Visibility.Hidden;
                        Image3.Visibility = Visibility.Hidden;
                        _isImageExpanded3 = true;
                    }
                    else
                    {
                        Image2.Width = 180;
                        Image2.Height = 180;
                        TextBlock.Visibility = Visibility.Visible;
                        Image.Visibility = Visibility.Visible;
                        Image1.Visibility = Visibility.Visible;
                        Image3.Visibility = Visibility.Visible;
                        _isImageExpanded3 = false;
                    }
                }
            }
        }
    }
}
