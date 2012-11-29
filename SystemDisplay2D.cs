using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace AccreteSharp
{
    public partial class SystemDisplay2D : Form
    {
        StarSystem _ss;

        public SystemDisplay2D(StarSystem ss)
        {
            InitializeComponent();
            _ss = ss;
        }

        private float scale(double r)
        {
            if (r <= 0.0) return 0;
            return (int)((Math.Log10(r+1)) * 400);
        }

        private void paintSS(Graphics g)
        {
            PictureBox s = pictureBox1;
            int maxW = s.Width;
            float plX, plR;

            Pen p = new Pen(Color.Black);
            Brush b = Brushes.Gray;

            #region draw scale and borders ---------------------------------------------------------
            g.DrawLine(p, 0, s.Height / 2, maxW, s.Height / 2);

            g.DrawString("0.1", new Font("Tahoma", 10), Brushes.Blue, scale(0.1), s.Height / 2);
            g.DrawLine(p, scale(0.1), s.Height / 2, scale(0.1), s.Height / 2 - 5);
            g.DrawString("1", new Font("Tahoma", 10), Brushes.Blue, scale(1), s.Height / 2);
            g.DrawString("5", new Font("Tahoma", 10), Brushes.Blue, scale(5), s.Height / 2);
            g.DrawString("10", new Font("Tahoma", 10), Brushes.Blue, scale(10), s.Height / 2);
            g.DrawString("50", new Font("Tahoma", 10), Brushes.Blue, scale(50), s.Height / 2);
            g.DrawString("100", new Font("Tahoma", 10), Brushes.Blue, scale(100), s.Height / 2);
            #endregion

            foreach (Planet pl in _ss.planetsList)
            {
                plX = scale(pl.a);
                plR = (float)pl.radius / 1000;

                g.DrawEllipse(p, plX, (s.Height / 2) - (plR / 2) + (float)pl.e * 50, plR / 2, plR / 2);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            paintSS(e.Graphics);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (new ArgumentParser().GetArgValue("disp") == "2d")
            {
                _ss = new StarSystem();

                Graphics g = pictureBox1.CreateGraphics();
                g.Clear(Color.FromKnownColor(KnownColor.ButtonFace));

                paintSS(g);
            }
        }

        private void SystemDisplay2D_Resize(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.FromKnownColor(KnownColor.ButtonFace));

            paintSS(g);
        }
    }
}