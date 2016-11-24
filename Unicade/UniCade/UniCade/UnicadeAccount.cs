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
            if ((textBox1.Text.Length < 1) || (textBox2.Text.Length < 1) || (textBox3.Text.Length < 1) || (textBox4.Text.Length < 1))
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
            if (!textBox2.Text.Contains("@")||textBox2.Text.Length<5)
            {
                MessageBox.Show("Invalid Email Length");
                return;
            }
            if (!textBox2.Text.Contains("@") || textBox3.Text.Length < 4)
            {
                MessageBox.Show("Password Must be at least 4 chars");
                return;
            }
            if (textBox1.Text.Length<3)
            {
                MessageBox.Show("Username must be at least 3 chars");
                return;
            }
            if (type == 0)
            {
                SQLclient.createUser(textBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, "Null", "NullProfPath");
            }
            else
            {
                
                User u = new User(textBox1.Text, textBox3.Text, 0, textBox2.Text, 0, textBox4.Text, "Mature", "null");
                foreach (User us in Program.dat.userList)
                {
                    if (us.getUsername().Equals(u.getUsername()))
                    {
                        MessageBox.Show("Error: Username already exists");
                        return;
                    }
                }
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
