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
        private int n;
        private PictureBox[,] Safe = null;
        private string ErrorMessage = "Enter values between 2 and 8";

        public Puzzle()
        {
            InitializeComponent();
        }

        public void EnterButton_Click(object sender, EventArgs e)
        {
            try
            {
                int size = Int32.Parse(textBox1.Text);
                if (size > 1 && size <= 8)
                {
                    n = size;
                    this.textBox1.Clear();
                }
                else
                {
                    MessageBox.Show(ErrorMessage);
                    n = 0;
                    this.textBox1.Clear();
                }
            }
            catch
            {
                MessageBox.Show(ErrorMessage);
            }

            if (Safe != null)
                foreach (var pictureBox in Safe)
                    groupBox1.Controls.Remove(pictureBox);

            Safe = new PictureBox[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Safe[i, j] = new PictureBox
                    {
                        Location = new System.Drawing.Point(800/(3/2*n) + i * 100, j * 100),
                        Size = new System.Drawing.Size(80, 80),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Image = Properties.Resources.Handle,
                        Anchor = AnchorStyles.Top,
                        TabIndex = i,
                        Tag = 0
                    };
                    groupBox1.Controls.Add(Safe[i, j]);
                    int row = i;
                    int column = j;
                    Safe[i, j].Click += (x, y) => { SafeClick(row, column); };
                }
            //this randomizer works by emulating user input. 
            //As it's aligned before randomization, the puzzle is always solvable by repeating
            //the process in reverse order

            var randomize = new Random();
            var notRandomized = true;
            var allAlignedCondition = true;
            while (notRandomized)
            {
                for (int i = 0; i < 50; i++)
                    SafeClick(randomize.Next(n), randomize.Next(n), true);

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        allAlignedCondition &= (int)Safe[i, j].Tag == (int)Safe[0, 0].Tag;

                if (!allAlignedCondition) notRandomized = false;
            }
        }

        public void SafeClick(int row, int column, bool init = false)
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

            if (win) MessageBox.Show("Puzzle solved!");
        }

        private void Puzzle_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
