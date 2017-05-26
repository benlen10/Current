using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UniCade
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
        bool ExpandImage1 = false;
        bool ExpandImage2 = false;
        bool ExpandImage3 = false;
        bool ExpandImage4 = false;

#endregion

        /// <summary>
        /// Public constructor for the GameInfo class
        /// </summary>
        public GameInfo()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();


        }

        public void displayEsrb(String esrb)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(@esrb);
            b.EndInit();
            image3.Source = b;

        }



        public void expand()
        {
            if (!ExpandImage4 && !ExpandImage2 && !ExpandImage3)
            {
                if (!ExpandImage1)
                {
                    image.Width = 700;
                    image.Height = 700;
                    textBlock.Visibility = Visibility.Hidden;
                    image1.Visibility = Visibility.Hidden;
                    image2.Visibility = Visibility.Hidden;
                    ExpandImage1 = true;

                }
                else
                {
                    image.Width = 180;
                    image.Height = 180;
                    textBlock.Visibility = Visibility.Visible;
                    image1.Visibility = Visibility.Visible;
                    image2.Visibility = Visibility.Visible;
                    ExpandImage1 = false;
                }
            }
        }

        public void expand1()
        {
            {
                if (!ExpandImage4 && !ExpandImage1 && !ExpandImage3)
                {
                    if (!ExpandImage2)
                    {
                        image1.Width = 700;
                        image1.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image2.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        ExpandImage2 = true;
                    }
                    else
                    {
                        image1.Width = 180;
                        image1.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image2.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        ExpandImage2 = false;
                    }
                }
            }
        }

        public void expand2()
        {
            {
                if (!ExpandImage4 && !ExpandImage2 && !ExpandImage1)
                {
                    if (!ExpandImage3)
                    {
                        image2.Width = 700;
                        image2.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image1.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        ExpandImage3 = true;
                    }
                    else
                    {
                        image2.Width = 180;
                        image2.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        ExpandImage3 = false;
                    }
                }
            }
        }


        public void expand3()
        {
            {
                /* if (!enlarge && !enlarge1 && !enlarge2)
                 {
                     if (!enlarge3)
                     {
                         image3.Width = 700;
                         image3.Height = 700;
                         textBlock.Visibility = Visibility.Hidden;
                         enlarge3 = true;
                     }
                     else
                     {
                         image3.Width = 180;
                         image3.Height = 180;
                         textBlock.Visibility = Visibility.Visible;
                         enlarge3 = false;
                     }
                 }*/
            }


        }
    }
}
