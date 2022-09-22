using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDTest
{
    public partial class Puzzle : Form
    {
        public Puzzle()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        int n;
        private PictureBox[,] Safe = null;

        private void EnterButton_Click(object sender, EventArgs e)
        {
            try
            {
                int size = Int32.Parse(textBox1.Text);
                if (size > 1 && size <= 8)
                    n = size;
                else
                {
                    MessageBox.Show("Enter values between 1 and 8");
                    n = 0;
                }
            }
            catch
            {
                MessageBox.Show("Enter values between 1 and 8");
            }


            if (Safe != null)
                foreach (var pictureBox in Safe)
                    Controls.Remove(pictureBox);

            Safe = new PictureBox[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Safe[i, j] = new PictureBox();
                    Safe[i, j].Location = new System.Drawing.Point(-58*n +474 + i * 100, 300 + j * 100);
                    Safe[i, j].Size = new System.Drawing.Size(80, 80);
                    Safe[i, j].TabIndex = i;
                    Safe[i, j].BackColor = Color.Black;
                    Safe[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    Safe[i, j].Image = Properties.Resources.Handle;
                    Safe[i, j].Tag = 0;
                    Safe[i, j].Anchor = AnchorStyles.Top;
                    Controls.Add(Safe[i, j]);
                    int row = i;
                    int column = j;
                    Safe[i, j].Click += (x, y) => { SafeClick(row, column); };
                }

            var rnd = new Random();
            for (int i = 0; i < 40; i++)
                SafeClick(rnd.Next(n), rnd.Next(n), true);
        }

        void SafeClick(int row, int column, bool init = false)
        {
            bool win = !init;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (i == row || j == column)
                    {
                        Image image = Safe[i, j].Image;
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        Safe[i, j].Image = image;

                        Safe[i, j].Tag = ((int)Safe[i, j].Tag + 1) % 2;
                    }

                    win &= (int)Safe[i, j].Tag == (int)Safe[0, 0].Tag;
                }

            if (win) MessageBox.Show("win");

        }
    }
}
