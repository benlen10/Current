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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)  //Close button
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  //Login button
        {
            if((textBox1.Text==null)|| (textBox2.Text == null))
            {
                MessageBox.Show("Fields cannot be blank");
                return;
            }
           
            if (SQLclient.authiencateUser(textBox1.Text, textBox2.Text))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect login details");
            }
        }
    }
}
