using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Threading;

namespace UniCadeCmd
{
    public partial class Splash : Form
    {
        int splashTime = 2000;
        System.Windows.Forms.Timer formClose = new Timer();
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BringToFront();
            pictureBox1.Load(@"C:\UniCade\Media\Backgrounds\UniCade Logo.png");
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            formClose.Interval = 3000;
            formClose.Tick += new EventHandler(formClose_Tick);
            formClose.Start();

        }
        void formClose_Tick(object sender, EventArgs e)
        {
            formClose.Stop();
            formClose.Tick -= new EventHandler(formClose_Tick);
            this.Close();
        }
    }
}
