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
    public partial class Login : Form
    {
        int type;

        public Login(int type)
        {
            this.type = type;
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
            if ((textBox1.Text == null) || (textBox2.Text == null))
            {
                MessageBox.Show("Fields cannot be blank");
                return;
            }

            if (type == 0)
            {
                if (SQLclient.authiencateUser(textBox1.Text, textBox2.Text))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show(this, "Incorrect login details");
                }
            }
            else
            {
                foreach (User u in Program.dat.userList)
                {
                    if (u.getUsername().Equals(textBox1.Text))
                    {
                        if (u.getPass().Equals(textBox2.Text))
                        {
                            SettingsWindow.curUser = u;
                            Close();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(this, "Incorrect Password");
                            return;
                        }
                        
                    }
                    
                }
                MessageBox.Show(this, "User does not exist");
                return;


            }
        }
    }
}
