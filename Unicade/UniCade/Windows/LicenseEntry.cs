using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UniCade
{
    public partial class LicenseEntry : Form
    {
        public LicenseEntry()
        {
            InitializeComponent();
            textBox1.Text = Program._userLicenseName;
            textBox2.Text = Program._userLicenseKey;
        }

        /// <summary>
        /// Enter button.
        /// This function will validate the license key and if valid, will save the key to the preferences file
        /// and close the current window
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            //Bad input checks
            if ((textBox1.Text == null) || (textBox2.Text == null))
            {
                System.Windows.MessageBox.Show("Missing Required Fields");
                return;
            }
            
            //Save the entered values to the currently active global variables
            Program._userLicenseName = textBox1.Text;
            Program._userLicenseKey = textBox2.Text;

            //If the key is valid, save the key text to the preferences file and close the current window
            if (Program.ValidateSHA256(Program._userLicenseName + Database.HashKey, Program._userLicenseKey))
            {
                MessageBox.Show(this,"License is VALID");
                Program._validLicense = true;
                FileOps.savePreferences(Program._prefPath);
                Close();
            }
            else
            {
                Program._validLicense = false;
                FileOps.savePreferences(Program._prefPath);
                MessageBox.Show(this,"License is INVALID");
                Close();
            }
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
