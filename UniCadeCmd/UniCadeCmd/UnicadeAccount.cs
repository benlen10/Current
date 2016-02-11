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
    public partial class UnicadeAccount : Form
    {
        public UnicadeAccount()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //Create account button
        {
            SQLclient.connectSQL();
            SQLclient.createUser(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, "Null", "NullProfPath");
            MessageBox.Show("User created");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
