using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniCadeCmd
{
    public partial class SettingsWindow : Form
    {
        Console curConsole;
        public SettingsWindow()
        {
            
            InitializeComponent();
            foreach(Console c in Program.dat.consoleList)
            {
                listBox1.Items.Add(c.getName());
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox1.SelectedItem.ToString();
            foreach(Console c in Program.dat.consoleList)
            {
                if (c.getName().Equals(curItem))
                {
                    curConsole = c;
                    textBox1.Text = c.getEmuPath();
                    textBox3.Text = c.getRomPath();
                    textBox4.Text = c.getRomExt();
                    textBox5.Text = c.getLaunchParam();
                    textBox7.Text = "Null";
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            curConsole.setEmuPath(textBox1.Text);
            curConsole.setRomPath(textBox3.Text);
            curConsole.setRomExt(textBox4.Text);
            curConsole.setLaunchParam(textBox5.Text);
            Program.saveDatabase(Program.databasePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
