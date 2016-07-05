using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniCadeCmd
{
    public partial class LicenseEntry : Form
    {
        public LicenseEntry()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)  //Validate button
        {

                if ((textBox1.Text == null) || (textBox2.Text == null))
                {
                    MessageBox.Show("Missing Required Fields");
                    return;
                }
                Program.userLicenseName = textBox1.Text;
                Program.userLicenseKey = textBox2.Text;
                

                if (Program.ValidateSHA256(Program.userLicenseName + Database.getHashKey() , Program.userLicenseKey))
                {
                MessageBox.Show("License is VALID");
                Program.validLicense = true;
                FileOps.savePreferences(Program.prefPath);
                Close();
                }
            else
            {
                MessageBox.Show("License is INVALID");
            }


            
        }

        private void button1_Click(object sender, EventArgs e)  //Close button
        {
            Close();
        }
    }
}
