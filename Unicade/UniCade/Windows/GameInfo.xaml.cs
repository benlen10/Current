using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for GameInfo.xaml
    /// </summary>
    public partial class GameInfo : Window
    {

        #region Properties

        /// <summary>
        /// Set to True if image 1 is currently expanded
        /// </summary>
        bool IsImageExpanded1 = false;

        /// <summary>
        /// Set to True if image 2 is currently expanded
        /// </summary>
        bool IsImageExpanded2 = false;

        /// <summary>
        /// Set to True if image 3 is currently expanded
        /// </summary>
        bool IsImageExpanded3 = false;

        /// <summary>
        /// Set to True if image 4 is currently expanded
        /// </summary>
        bool IsImageExpanded4 = false;

        #endregion

        /// <summary>
        /// Public constructor for the GameInfo class
        /// </summary>
        public GameInfo()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        /// <summary>
        /// Display the icon for the current ESRB rating
        /// </summary>
        public void DisplayEsrb(String esrbRating)
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
            if (!IsImageExpanded4 && !IsImageExpanded2 && !IsImageExpanded3)
            {
                if (!IsImageExpanded1)
                {
                    image.Width = 700;
                    image.Height = 700;
                    textBlock.Visibility = Visibility.Hidden;
                    image1.Visibility = Visibility.Hidden;
                    image2.Visibility = Visibility.Hidden;
                    IsImageExpanded1 = true;

                }
                else
                {
                    image.Width = 180;
                    image.Height = 180;
                    textBlock.Visibility = Visibility.Visible;
                    image1.Visibility = Visibility.Visible;
                    image2.Visibility = Visibility.Visible;
                    IsImageExpanded1 = false;
                }
            }
        }

        /// <summary>
        /// Expand image 2
        /// </summary>
        public void ExpandImage2()
        {
            {
                if (!IsImageExpanded4 && !IsImageExpanded1 && !IsImageExpanded3)
                {
                    if (!IsImageExpanded2)
                    {
                        image1.Width = 700;
                        image1.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image2.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        IsImageExpanded2 = true;
                    }
                    else
                    {
                        image1.Width = 180;
                        image1.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image2.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        IsImageExpanded2 = false;
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
                if (!IsImageExpanded4 && !IsImageExpanded2 && !IsImageExpanded1)
                {
                    if (!IsImageExpanded3)
                    {
                        image2.Width = 700;
                        image2.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image1.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        IsImageExpanded3 = true;
                    }
                    else
                    {
                        image2.Width = 180;
                        image2.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        IsImageExpanded3 = false;
                    }
                }
            }
        }

        /// <summary>
        /// Expand image 4
        /// </summary>
        public void ExpandImage4()
        {

        }
    }
}
