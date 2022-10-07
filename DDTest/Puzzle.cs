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
        // the restrictions on the size of the puzzle
        // are in place to limit the area on the screen
        //that is required to fit it. It could be increased to any 
        //number without massive alterations to the code.

        public Puzzle()
        {
            InitializeComponent();
        }

        private void DisplayError(string text)
        {
            MessageBox.Show(text);
            _puzzleSize = 0;
            this.textBox1.Clear();
        }

        public void EnterButton_Click(object sender, EventArgs e)
        {
            try
            {
                int size = Int32.Parse(textBox1.Text);
                if (size > 1 && size <= 7)
                {
                    _puzzleSize = size;
                    this.textBox1.Clear();
                }
                else
                {
                    DisplayError(MessageConstants.ErrorText);
                }

            }
            catch 
            { 
                DisplayError(MessageConstants.ErrorText); 
            }
      

            if (_puzzleSize > 1 && _puzzleSize <= 7)
            {
                
                _safepuzzle.Show();

                if (_safe != null)
                {
                    foreach (var pictureBox in _safe)
                        _sizedGB.Controls.Remove(pictureBox);
                    _safepuzzle.Controls.Remove(_sizedGB);
                }

                _safe = new PictureBox[_puzzleSize, _puzzleSize];
                _sizedGB = new GroupBox();
                _sizedGB.Size = new Size(_puzzleSize * 100,_puzzleSize * 100);
                _sizedGB.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                _safepuzzle.Controls.Add(_sizedGB);

                var randomize = new Random();
                var notRandomized = true;
                var allAlignedCondition = true;

                for (int i = 0; i < _puzzleSize; i++)
                    for (int j = 0; j < _puzzleSize; j++)
                    {
                        _safe[i, j] = new PictureBox
                        {
                            Location = new System.Drawing.Point(20 + i * 100, 20 + j * 100),
                            Size = new System.Drawing.Size(80, 80),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = Properties.Resources.Handle,
                            Anchor = AnchorStyles.Top,
                            TabIndex = i,
                            Tag = 0
                        };
                        _sizedGB.Controls.Add(_safe[i, j]);
                        int row = i;
                        int column = j;
                        _safe[i, j].Click += (x, y) => { SafeClick(row, column); };
                    }

                while (notRandomized)
                {
                    for (int i = 0; i < _puzzleSize * _puzzleSize; i++)
                        SafeClick(randomize.Next(_puzzleSize), randomize.Next(_puzzleSize), true);

                    for (int i = 0; i < _puzzleSize; i++)
                        for (int j = 0; j < _puzzleSize; j++)
                            allAlignedCondition &= (int)_safe[i, j].Tag == (int)_safe[0, 0].Tag;

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
            for (int i = 0; i < _puzzleSize; i++)
                for (int j = 0; j < _puzzleSize; j++)
                {
                    if (i == row || j == column)
                    {
                        Image image = _safe[i, j].Image;
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        _safe[i, j].Image = image;
                        _safe[i, j].Tag = ((int)_safe[i, j].Tag + 1) % 2;
                    }
                    win &= (int)_safe[i, j].Tag == (int)_safe[0, 0].Tag;
                }

            if (win) 
            { 
                MessageBox.Show(MessageConstants.PuzzleSolvedText);
                _safepuzzle.Hide();
            }
        }

        private int _puzzleSize;
        private PictureBox[,] _safe = null;
        private GroupBox _sizedGB = null;
        private Safe _safepuzzle = new Safe();
    }
}
