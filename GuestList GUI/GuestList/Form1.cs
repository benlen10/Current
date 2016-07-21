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
        const int maxUsers =100;
        System.Collections.ArrayList users;


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
            users.Add(new User("Ben", "Len", "6-20-1995" , 1);

            if (pass.Equals(realPass)){
                 resultTxt = string.Format("Password: {0} is VALID", pass);
            }
            else{
                 resultTxt = string.Format("Password: {0} is INVALID", pass);
                
            }
            //MessageBox.Show(resultTxt);
            while (!pass.Equals(realPass))    //Loop until correct pass is entered
            {
                passWindow.ShowDialog();
                MessageBox.Show(resultTxt);
            }


        }

        static public void setPass(string p)
        {
            pass = p;
        }

        public void refreshList()
        {
            string contents = "Guest\n";
            foreach (string s in users)
            {
                contents = contents + s;
            }
            richTextBox1.Text = contents;
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
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
