﻿using System;
using System.Windows.Forms;

namespace UniCade
{
    public partial class PassWindow : Form
    {
        /// <summary>
        /// Public constructor for the password window
        /// </summary>
        public PassWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Enter button. Check if the entered pass is valid, otherwise display an error
        /// </summary>
        private void Button1_Click(object sender, EventArgs e)
        {
            Int32.TryParse(textBox1.Text, out int n);
            if (n > 0)
            {
                if (Int32.Parse(textBox1.Text) == SettingsWindow._passProtect)
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
        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
