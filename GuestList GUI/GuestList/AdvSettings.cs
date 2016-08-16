using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestList
{
    public partial class AdvSettings : Form
    {
        public AdvSettings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)  //Save Database
        {
            Program.f1.saveDatabase();
        }

        private void button9_Click(object sender, EventArgs e)  //Load Databse
        {
            Program.f1.loadDatabase();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed by Benjamin Lenington\nFree and open source");
        }

        private void button2_Click(object sender, EventArgs e)  //Change database path
        {
            TextEntry te = new TextEntry();
            te.ShowDialog();

        }
    }
}
