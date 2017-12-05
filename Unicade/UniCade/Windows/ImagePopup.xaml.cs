using System;
using System.Windows.Media.Imaging;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for ImagePopup.xaml
    /// </summary>
    public partial class ImagePopup
    {
        public ImagePopup(string path)
        {
            InitializeComponent();
            MainImage.Source = new BitmapImage(new Uri(path));
        }
    }
}
