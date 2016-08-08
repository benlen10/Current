﻿using System;
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
    public partial class GuestInfo : Form
    {
        User u;
        public GuestInfo(User u)
        {
            this.u = u;
            InitializeComponent();
            textBox1.Text = u.getFirstName();
            textBox6.Text = u.getLastName();
            textBox2.Text = "ID Number";
            textBox3.Text = u.getBirthday();
            textBox4.Text = "Pending";
            textBox5.Text = u.getPriority().ToString();
            textBox7.Text = u.getStatus().ToString();
            richTextBox1.Text = u.getNotes();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)  //Close button
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  //Update Guest Info
        {
            if((textBox1.Text.Length<1)|| (textBox5.Text.Length < 1) || (textBox6.Text.Length < 1) || (textBox7.Text.Length < 1))
            {
                MessageBox.Show("Invalid Info");
            }
            u.SetFirstName(textBox1.Text);
            u.SetLastName(textBox6.Text);
            u.SetPriority(Int32.Parse(textBox5.Text));
            u.SetStatus(Int32.Parse(textBox7.Text));
            u.setNotes(richTextBox1.Text);
            Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = "";
            u.setBlacklist();
            if (u.blacklistStat())
            {
                text = "Guest is on Blacklist";
            }
            else
            {
                text = "Guest is clear";
            }
            MessageBox.Show(text);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}