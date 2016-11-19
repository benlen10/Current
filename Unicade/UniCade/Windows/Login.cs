using System;
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

        /// <summary>
        /// Close button
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Login button
        /// Checks for bad user input and authenticates the user against the database
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            //Bad input checks
            if ((textBox1.Text == null) || (textBox2.Text == null))
            {
                MessageBox.Show("Fields cannot be blank");
                return;
            }

            //If the user is a SQL client, preform SQL user authentication 
            if (type == 0)
            {
                if (SQLclient.AuthiencateUser(textBox1.Text, textBox2.Text))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show(this, "Incorrect login details");
                }
            }

            //If the user is a local account, validate the info and close the window if sucuessful 
            else
            {
                foreach (User u in Database.UserList)
                {
                    if (u.Username.Equals(textBox1.Text))
                    {
                        if (u.Pass.Equals(textBox2.Text))
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
