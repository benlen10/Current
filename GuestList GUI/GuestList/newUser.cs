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
    public partial class newUser : Form
    {
        public newUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if ((textBox1.Text.Length < 1) || (textBox2.Text.Length < 1) || (textBox4.Text.Length < 1))
            {
                MessageBox.Show("Invalid Input");
            }
            else
            {
                Form1.users.Add(new User(textBox1.Text, textBox2.Text, maskedTextBox1.Text, Int32.Parse(textBox4.Text),0," ", " "));
                Program.f1.refreshList();
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
