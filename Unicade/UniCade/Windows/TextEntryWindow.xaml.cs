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

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for TextEntryWindow.xaml
    /// </summary>
    public partial class TextEntryWindow : Window
    {

        #region  Properties

        internal string EnteredText;

        #endregion


        public TextEntryWindow()
        {
            InitializeComponent();
        }

        #region  Private Methods

        private void TextEntryWindowButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextEntryWindowButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            EnteredText = TextEntryWindowTextbox.Text;
            Close();
        }

        #endregion
    }
}
