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
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);


        }

        public void displayEsrb(String esrb)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(@esrb);
            b.EndInit();
            image3.Source = b;
            
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Escape) || (e.Key == Key.Back)|| (e.Key == Key.I))
            {
                Close();

            }

            if ((e.Key == Key.F) )
            {
                if (!enlarge3 && !enlarge1 && !enlarge2)
                {
                    if (!enlarge)
                    {
                        image.Width = 500;
                        image.Height = 500;
                        enlarge = true;
                    }
                    else
                    {
                        image.Width = 100;
                        image.Height = 100;
                        enlarge = false;
                    }
                }
            }

            if ((e.Key == Key.B))
            {
                if (!enlarge3 && !enlarge && !enlarge2)
                {
                    if (!enlarge1)
                    {
                        image1.Width = 500;
                        image1.Height = 500;
                        enlarge1 = true;
                    }
                    else
                    {
                        image1.Width = 100;
                        image1.Height = 100;
                        enlarge1 = false;
                    }
                }
            }

            if ((e.Key == Key.S))
            {
                if (!enlarge3 && !enlarge1 && !enlarge)
                {
                    if (!enlarge2)
                    {
                        image2.Width = 500;
                        image2.Height = 500;
                        enlarge2 = true;
                    }
                    else
                    {
                        image2.Width = 100;
                        image2.Height = 100;
                        enlarge2 = false;
                    }
                }
            }

            if ((e.Key == Key.E))
            {
                if (!enlarge && !enlarge1 && !enlarge2)
                {
                    if (!enlarge3)
                    {
                        image3.Width = 500;
                        image3.Height = 500;
                        enlarge = true;
                    }
                    else
                    {
                        image3.Width = 100;
                        image3.Height = 100;
                        enlarge = false;
                    }
                }
            }


        }
    }
}
