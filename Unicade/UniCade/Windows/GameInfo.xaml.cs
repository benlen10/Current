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
            image3.Source = bitmapImage;
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
                    image.Width = 700;
                    image.Height = 700;
                    textBlock.Visibility = Visibility.Hidden;
                    image1.Visibility = Visibility.Hidden;
                    image2.Visibility = Visibility.Hidden;
                    _isImageExpanded1 = true;

                }
                else
                {
                    image.Width = 180;
                    image.Height = 180;
                    textBlock.Visibility = Visibility.Visible;
                    image1.Visibility = Visibility.Visible;
                    image2.Visibility = Visibility.Visible;
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
                        image1.Width = 700;
                        image1.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image2.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        _isImageExpanded2 = true;
                    }
                    else
                    {
                        image1.Width = 180;
                        image1.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image2.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
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
                        image2.Width = 700;
                        image2.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image1.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        _isImageExpanded3 = true;
                    }
                    else
                    {
                        image2.Width = 180;
                        image2.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        _isImageExpanded3 = false;
                    }
                }
            }
        }
    }
}
