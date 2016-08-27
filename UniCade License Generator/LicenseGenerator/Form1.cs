using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicenseGenerator
{
    public partial class Form1 : Form
    {
        private static string UniCadeKey = "JI3vgsD6Nc6VSMrNw0b4wvuJmDw6Lrld";
        private static string GuestListKey = "bjx58tQvp2Y1U5xO71PIsvFL7FGyyI08";

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("UniCade Interface");
            comboBox1.Items.Add("Guest List Software");
            comboBox2.Items.Add("SHA1");
            comboBox2.Items.Add("SHA256");
        }

        private void button1_Click(object sender, EventArgs e)  //Generate Button
        {
            if ((textBox1.Text.Length < 1) || (textBox3.Text.Length < 1)  || (comboBox2.SelectedItem==null))
            {
                MessageBox.Show("Required Fields Empty");
                return;
            }
            if (comboBox2.SelectedItem.ToString().Equals("SHA1"))
            {
                textBox2.Text = SHA1Hash(textBox1.Text + textBox3.Text);
            }
            else if (comboBox2.SelectedItem.ToString().Equals("SHA256"))
            {
                textBox2.Text = SHA256Hash(textBox1.Text + textBox3.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private string SHA1Hash(string data)
        {


            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }
        private string SHA256Hash(string data)
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        private bool ValidateSHA1(string input, string storedHashData)
        {
            string getHashInputData = SHA1Hash(input);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidateSHA256(string input, string storedHashData)
        {
            string getHashInputData = SHA256Hash(input);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button5_Click(object sender, EventArgs e)  //Validate button
        {
            if ((textBox1.Text.Length < 1) || (textBox3.Text.Length < 1) || (textBox2.Text.Length < 1) || (comboBox2.SelectedItem == null))
            {
                MessageBox.Show("Required Fields Empty");
                return;
            }
            if (comboBox2.SelectedItem.ToString().Equals("SHA1"))
            {
                if (ValidateSHA1((textBox1.Text + textBox3.Text), textBox2.Text))  //Name + key
                {
                    MessageBox.Show("Key is VALID");
                }
                else
                {
                    MessageBox.Show("Key is INVALID");
                }
            }
            else if (comboBox2.SelectedItem.ToString().Equals("SHA256"))
            {
                if (ValidateSHA256((textBox1.Text + textBox3.Text), textBox2.Text))
                {
                    MessageBox.Show("Key is VALID");
                }
                else
                {
                    MessageBox.Show("Key is INVALID");
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  //Product menu changed
        {

                if(comboBox1.SelectedIndex == 0){
                    textBox3.Text = UniCadeKey;
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    textBox3.Text = GuestListKey;
                }
                else
                {
                    textBox3.Text = null;
                }

        }
    }
}
