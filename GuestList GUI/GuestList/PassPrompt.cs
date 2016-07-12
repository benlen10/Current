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
    public partial class PassPrompt : Form
    {
        public PassPrompt()
        {
            InitializeComponent();
        }

        private void PassPrompt_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.setPass(textBox1.Text);
            Form1.user = textBox2.Text;
            Close();
            
        }
    }
}
