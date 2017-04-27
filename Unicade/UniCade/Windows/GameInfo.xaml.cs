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

namespace UniCade
{
    /// <summary>
    /// Interaction logic for GameInfo.xaml
    /// </summary>
    public partial class GameInfo : Window
    {
        bool enlarge = false;
        bool enlarge1 = false;
        bool enlarge2 = false;
        bool enlarge3 = false;


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



            public void expand() { 
                if (!enlarge3 && !enlarge1 && !enlarge2)
                {
                    if (!enlarge)
                    {
                        image.Width = 700;
                        image.Height = 700;
                    textBlock.Visibility = Visibility.Hidden;
                    image1.Visibility = Visibility.Hidden;
                    image2.Visibility = Visibility.Hidden;
                    enlarge = true;

                }
                    else
                    {
                        image.Width = 180;
                        image.Height = 180;
                    textBlock.Visibility = Visibility.Visible;
                    image1.Visibility = Visibility.Visible;
                    image2.Visibility = Visibility.Visible;
                    enlarge = false;
                    }
                }
            }

        public void expand1()
        {
            {
                if (!enlarge3 && !enlarge && !enlarge2)
                {
                    if (!enlarge1)
                    {
                        image1.Width = 700;
                        image1.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image2.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        enlarge1 = true;
                    }
                    else
                    {
                        image1.Width = 180;
                        image1.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image2.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        enlarge1 = false;
                    }
                }
            }
        }

        public void expand2()
        {
            {
                if (!enlarge3 && !enlarge1 && !enlarge)
                {
                    if (!enlarge2)
                    {
                        image2.Width = 700;
                        image2.Height = 700;
                        textBlock.Visibility = Visibility.Hidden;
                        image.Visibility = Visibility.Hidden;
                        image1.Visibility = Visibility.Hidden;
                        image3.Visibility = Visibility.Hidden;
                        enlarge2 = true;
                    }
                    else
                    {
                        image2.Width = 180;
                        image2.Height = 180;
                        textBlock.Visibility = Visibility.Visible;
                        image.Visibility = Visibility.Visible;
                        image1.Visibility = Visibility.Visible;
                        image3.Visibility = Visibility.Visible;
                        enlarge2 = false;
                    }
                }
            }
        }
            

            public void expand3() { 
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
