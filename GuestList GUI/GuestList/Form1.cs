using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace GuestList
{
    partial class Form1 : Form
    {
        protected static string pass;
        public static string user;
        private string realPass = "temp";
        const int maxUsers = 100;
        public static System.Collections.ArrayList users;
        public string path = @"C:\Users\Ben\Desktop\Database.txt";


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
            //users.Add(new User("Ben", "Len", "6-20-1995", 1,1," ", " "));
            loadDatabase();
            refreshList();

        }
        // METHOD DECLARATION

        static public void setPass(string p)
        {
            pass = p;
        }

        public void refreshList()
        {
            label2.Text = "Checked In";
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

        private void button3_Click(object sender, EventArgs e)  //Blacklist 
        {
            if (!label2.Text.Equals("BLACKLIST"))
            {
                string contents = "";
                label2.Text = "BLACKLIST";

                foreach (User u in users)
                {
                    if (u.blacklistStat())
                    {
                        contents = contents + u.getFullName() + "\n";
                    }

                }
                richTextBox2.Text = contents;
            }
            else
            {
                label2.Text = "Checked In";
                refreshList();
            }
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
            if (users.Count > 0)
            {
                foreach (User u in users)
                {
                    if (u.getFullName().Equals(textBox1.Text))
                    {
                        GuestInfo gi = new GuestInfo(u);
                        found = true;
                        gi.ShowDialog();
                        break;
                    }
                }
                if (!found)
                {
                    MessageBox.Show("User Not Found");
                }
            }
            else
            {
                MessageBox.Show("Guest List is Empty");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void saveDatabase()
        {
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }

                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (User u in users)
                    {
                        string txt = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{7}| |", u.getFirstName(), u.getLastName(), u.getBirthday(), 0, u.getPriority(), u.getStatus(),u.getId(), u.getNotes());
                        sw.WriteLine(txt);

                    }

                }

            
        }
        public void loadDatabase()
        {
            
            string line;
            char[] sep = { '|' };
            string[] r = {" "};
            StreamReader file = new StreamReader(@"C:\Users\Ben\Desktop\Database.txt");
            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                //Console.WriteLine("Length: " + r.Length);
                if (r.Length >= 7)
                {
                    users.Add(new User(r[0], r[1], r[2], Int32.Parse(r[3]), Int32.Parse(r[4]), r[5], r[6]));
                }
            }
            file.Close();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AdvSettings av = new AdvSettings();
            av.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)  //Exit
        {
            saveDatabase();
            Close();
            Environment.Exit(0);
        }
    }
}
