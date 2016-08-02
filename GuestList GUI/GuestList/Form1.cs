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
    partial class Form1 : Form
    {
        protected static string pass;
        public static string user;
        private string realPass = "temp";
        const int maxUsers = 100;
        public static System.Collections.ArrayList users;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PassPrompt passWindow = new PassPrompt();
            passWindow.ShowDialog();
            string resultTxt;
            users = new System.Collections.ArrayList();


            if (pass.Equals(realPass))
            {
                resultTxt = string.Format("Password: {0} is VALID", pass);
            }
            else
            {
                resultTxt = string.Format("Password: {0} is INVALID", pass);

            }
            //MessageBox.Show(resultTxt);
            while (!pass.Equals(realPass))    //Loop until correct pass is entered
            {
                passWindow.ShowDialog();
                MessageBox.Show(resultTxt);
            }
            users.Add(new User("Ben", "Len", "6-20-1995", 1));
            refreshList();

        }
        // METHOD DECLARATION

        static public void setPass(string p)
        {
            pass = p;
        }

        public void refreshList()
        {
            string contents = "";
            string contents2 = "";

            foreach (User u in users)
            {
                if (u.getStatus() == 0)
                {
                    contents = contents + u.getFullName() + "\n";
                }
                else
                {
                    contents2 = contents2 + u.getFullName() + "\n";
                }
            }
            richTextBox1.Text = contents;
            richTextBox2.Text = contents2;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            newUser nu = new newUser();
            nu.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (User u in users)
            {
                if (u.getFullName().Equals(textBox1.Text))
                {
                    if (u.getStatus() == 0)
                    {
                        u.SetStatus(1);
                    }
                    else
                    {
                        u.SetStatus(0);
                    }
                }
            }
            refreshList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach (User u in users)
            {
                if (u.getFullName().Equals(textBox1.Text))
                {
                    GuestInfo gi = new GuestInfo(u);
                    found = true;
                    gi.ShowDialog();
                }
            }
            if (!found)
            {
                MessageBox.Show("User Not Found");
            }
        }
    }
}
