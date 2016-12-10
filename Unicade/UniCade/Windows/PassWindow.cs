using System;
using System.Windows.Forms;

namespace UniCade
{
    public partial class PassWindow : Form
    {
        public PassWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Enter button. Check if the entered pass is valid, otherwise display an error
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            Int32.TryParse(textBox1.Text, out n);
            if (n > 0)
            {
                if (Int32.Parse(textBox1.Text) == SettingsWindow.passProtect)
                {
                    DialogResult = DialogResult.OK;
                    MainWindow._validPAss = true;
                }

            }
            textBox1.Text = null;
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
