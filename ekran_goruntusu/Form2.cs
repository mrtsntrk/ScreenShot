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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ekran_goruntusu
{
    public partial class Form2 : Form
    {
        int selectX;
        int selectY;
        int selectWidth;
        int selectHeight;
        public Pen selectPen;

        bool start = false;
        private void SaveToClipboard()
        {
            if(selectWidth >0)
            {
                Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);
                Bitmap OrginalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                Bitmap _img = new Bitmap(selectWidth, selectHeight);
                Graphics g = Graphics.FromImage(_img);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(OrginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                Clipboard.SetImage(_img);
            }
            //End application
            //Application.Exit();
            Form1 frm1 = new Form1();
            this.Close();
            frm1.ShowDialog();

        }
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Hide();
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphic = Graphics.FromImage(printscreen as Image);
            graphic.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            using (MemoryStream s = new MemoryStream())
            {
                printscreen.Save(s, ImageFormat.Bmp);
                pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);
                pictureBox1.Image = Image.FromStream(s);

            }
            this.Show();
            Cursor = Cursors.Cross;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(pictureBox1.Image == null)
                return;
                 if (start)
            {
                pictureBox1.Refresh();
                selectWidth = e.X - selectX;
                selectHeight = e.Y - selectY;
                pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);

            }
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(!start)
            {
                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    selectX = e.X;
                    selectY = e.Y;
                    selectPen = new Pen(Color.Red, 1);
                    selectPen.DashStyle = DashStyle.DashDotDot;
                }
                pictureBox1.Refresh();
                start = true;
            }
            else
            {
                if (pictureBox1.Image == null)
                    return;
                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    selectWidth = e.X - selectX;
                    selectHeight = e.Y - selectY;
                    pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);

                }
                start = false;
                SaveToClipboard();
            }
        }
    }
}
