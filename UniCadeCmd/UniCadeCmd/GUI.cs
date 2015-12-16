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
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Load(@"C: \UniCade\Media\SNES.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

        }

        void GUI_KeyPress(object sender, KeyPressEventArgs e)
        {

                MessageBox.Show("Form.KeyPress: '" +
                    e.KeyChar.ToString() + "' pressed.");

        }
    }
}
