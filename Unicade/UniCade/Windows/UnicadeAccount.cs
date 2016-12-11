using System;
using System.Windows.Forms;

namespace UniCade
{
    public partial class UnicadeAccount : Form
    {
        #region Properties

        int AccountType;

        #endregion

        public UnicadeAccount(int type)
        {
            this.AccountType = type;
            InitializeComponent();
        }

        /// <summary>
        /// Create account button
        /// </summary>
        private void button1_Click(object sender, EventArgs e) 
        {
            //Check for invalid input
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

            //Create a new SQL user if the account type is UniCade Cloud
                if (AccountType == 0)
                SQLclient.CreateUser(textBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, "Null", "NullProfPath");
            else
            {
                //Create a new local user if the account type standard Unicade
                User u = new User(textBox1.Text, textBox3.Text, 0, textBox2.Text, 0, textBox4.Text, "Mature", "null");
                Database.UserList.Add(u);
                SettingsWindow.curUser = u;
            }
            Close();
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UnicadeAccount_Load(object sender, EventArgs e)
        {
        }
    }
}
