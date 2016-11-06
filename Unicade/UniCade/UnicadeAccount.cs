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
    public partial class UnicadeAccount : Form
    {
        int type;
        public UnicadeAccount(int type)
        {
            this.type = type;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //Create account button
        {
            if ((textBox1.Text == null) || (textBox2.Text == null)|| (textBox3.Text == null)|| (textBox4.Text == null))
            {
                MessageBox.Show("Fields cannot be empty");
                return;
            }
            if ((textBox1.TextLength>30) || (textBox2.Text.Length>30) || (textBox3.Text.Length>30) || (textBox4.Text.Length>100))//verify lengh. Description can be up to 100chars
            {
                MessageBox.Show("Invalid Length");
                return;
            }
            if (!textBox2.Text.Contains("@"))
            {
                MessageBox.Show("Invalid Email");
                return;
            }
                if (type == 0)
            {
                SQLclient.createUser(textBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, "Null", "NullProfPath");
            }
            else
            {
                User u = new User(textBox1.Text, textBox3.Text, 0, textBox2.Text, 0, textBox4.Text, "Mature", "null");
                Program.dat.userList.Add(u);
                SettingsWindow.curUser = u;
            }
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
