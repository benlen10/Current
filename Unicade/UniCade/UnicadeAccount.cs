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
            if ((textBox1.Text == null) || (textBox2.Text == null)|| (textBox3.Text == null)|| (textBox4.Text == null))
            {
                MessageBox.Show("Fields cannot be empty");
                return;
            }
            SQLclient.createUser(textBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, "Null", "NullProfPath");
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UnicadeAccount_Load(object sender, EventArgs e)
        {

        }
    }
}
