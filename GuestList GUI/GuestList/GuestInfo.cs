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
    public partial class GuestInfo : Form
    {
        public GuestInfo(User u)
        {
            InitializeComponent();
            textBox1.Text = u.getFullName();
            textBox2.Text = "ID Number";
            textBox3.Text = u.getBirthday();
            textBox4.Text = "Pending";
            textBox5.Text = u.getPriority().ToString();
                


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
