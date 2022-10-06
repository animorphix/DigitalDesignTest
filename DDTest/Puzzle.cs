using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace DDTest
{
    public partial class Puzzle : Form
    {
        
        private int n;
        private PictureBox[,] Safe = null;
        private GroupBox SizedGB = null;

        // the restrictions are in place to limit the area on the screen
        //that is required to fit the puzzle. It could be increased to any 
        //number without massive alterations to the code.
        private string ErrorMessage = "Enter values between 2 and 7";

        public Puzzle()
        {
            InitializeComponent();
        }

        

        public void EnterButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                int size = Int32.Parse(textBox1.Text);
                if (size > 1 && size <= 7)
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
                n = 0;
                this.textBox1.Clear();
            }

           

            if (n > 1 && n <= 7)
            {
                Safe safepuzzle = new Safe();
                safepuzzle.Show();

                if (Safe != null)
                {
                    foreach (var pictureBox in Safe)
                        SizedGB.Controls.Remove(pictureBox);
                    safepuzzle.Controls.Remove(SizedGB);
                }

                Safe = new PictureBox[n, n];
                SizedGB = new GroupBox();
                SizedGB.Size = new Size(n * 100,n * 100);
                SizedGB.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                safepuzzle.Controls.Add(SizedGB);

                var randomize = new Random();
                var notRandomized = true;
                var allAlignedCondition = true;

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        Safe[i, j] = new PictureBox
                        {
                            Location = new System.Drawing.Point(20 + i * 100, 20 + j * 100),
                            Size = new System.Drawing.Size(80, 80),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = Properties.Resources.Handle,
                            Anchor = AnchorStyles.Top,
                            TabIndex = i,
                            Tag = 0
                        };
                        SizedGB.Controls.Add(Safe[i, j]);
                        int row = i;
                        int column = j;
                        Safe[i, j].Click += (x, y) => { SafeClick(row, column); };
                    }

                while (notRandomized)
                {
                    for (int i = 0; i < n * n; i++)
                        SafeClick(randomize.Next(n), randomize.Next(n), true);

                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            allAlignedCondition &= (int)Safe[i, j].Tag == (int)Safe[0, 0].Tag;

                    if (!allAlignedCondition) notRandomized = false;
                }
                //this randomizer works by emulating user input. 
                //As it's aligned before randomization, the puzzle is always solvable by repeating
                //the process in reverse order
                
            }
        }

        public void SafeClick(int row, int column, bool state = false)
        {
            bool win = !state;
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

    }
}
