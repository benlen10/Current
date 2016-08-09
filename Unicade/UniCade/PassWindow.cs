using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniCade
{
    public partial class PassWindow : Form
    {
        public PassWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            Int32.TryParse(textBox1.Text, out n);
            if (n > 0)
            {
                if (Int32.Parse(textBox1.Text) == SettingsWindow.passProtect)
                {
                    DialogResult = DialogResult.OK;
                    MainWindow.passValid = true;
                }

            }
            textBox1.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();

        }
    }

}
