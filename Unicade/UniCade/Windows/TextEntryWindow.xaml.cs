using System.Windows;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for TextEntryWindow.xaml
    /// </summary>
    public partial class TextEntryWindow
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
