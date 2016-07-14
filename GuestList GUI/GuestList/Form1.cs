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
            users.Add("User 1\n");
            users.Add("User 2\n");
            if (pass.Equals(realPass)){
                 resultTxt = string.Format("Password: {0} is VALID", pass);
            }
            else{
                 resultTxt = string.Format("Password: {0} is INVALID", pass);
                
            }
            //MessageBox.Show(resultTxt);
            while (!pass.Equals(realPass))
            {
                passWindow.ShowDialog();
                MessageBox.Show(resultTxt);
            }
            string contents = "Guest\n";
            foreach (string s in users)
            {
                contents = contents + s;
            }
                richTextBox1.Text = contents;
            
            

        }

        static public void setPass(string p)
        {
            pass = p;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
