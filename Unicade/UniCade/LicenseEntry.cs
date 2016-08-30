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
    public partial class LicenseEntry : Form
    {
        public LicenseEntry()
        {
            InitializeComponent();
            textBox1.Text = Program.userLicenseName;
            textBox2.Text = Program.userLicenseKey;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == null) || (textBox2.Text == null))
            {
                System.Windows.MessageBox.Show("Missing Required Fields");
                return;
            }
            Program.userLicenseName = textBox1.Text;
            Program.userLicenseKey = textBox2.Text;


            if (Program.ValidateSHA256(Program.userLicenseName + Database.getHashKey(), Program.userLicenseKey))
            {
                MessageBox.Show(this,"License is VALID");
                Program.validLicense = true;
                FileOps.savePreferences(Program.prefPath);
                Close();
            }
            else
            {
                Program.validLicense = false;
                FileOps.savePreferences(Program.prefPath);
                MessageBox.Show(this,"License is INVALID");
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
