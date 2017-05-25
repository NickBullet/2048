using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public partial class MainForm : Form
    {
        private int count_step = 0;
        private readonly Game game = new Game();
        private readonly Bitmap bitmap;

        public MainForm()
        {
            InitializeComponent();

            if (gameField.Height != gameField.Width)
            {
                throw new InvalidOperationException("Game field must be quadratic.");
            }
            for (int row = 0; row < Game.FieldSize; row++)
            {
                for (int column = 0; column < Game.FieldSize; column++)
                {
                    game.Old_s[row, column] = game.Field[row, column];
                    game.New_s[row, column] = game.Field[row, column];
                }
            }
            bitmap = new Bitmap(gameField.Height, gameField.Width);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            drawGameField();
        }

        private void gameField_Paint(object sender, PaintEventArgs e)
        {
            gameField.BackgroundImage = bitmap;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool isGameOver = false;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    isGameOver = game.makeTurn(Direction.Up);
                    count_step++;
                    break;
                case Keys.Down:
                    isGameOver = game.makeTurn(Direction.Down);
                    count_step++;
                    break;
                case Keys.Left:
                    isGameOver = game.makeTurn(Direction.Left);
                    count_step++;
                    break;
                case Keys.Right:
                    isGameOver = game.makeTurn(Direction.Right);
                    count_step++;
                    break;
            }

            lblCurrentScore.Text = game.currentScore.ToString();

            drawGameField();
            if (isGameOver)
            {
                lblBestScore.Text = game.bestScore.ToString();
                if (MessageBox.Show("Field will be cleaned.\n Your final score: "
                    + game.currentScore.ToString(), "Game over", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    game.restart();
                    drawGameField();
                }
            }
        }

        private void drawGameField()
        {
            for (int row = 0; row < Game.FieldSize; row++)
            {
                for (int column = 0; column < Game.FieldSize; column++)
                {
                    int cellValue = game.Field[row, column];
                    Point drawPosition = CellStyle.getDrawPosition(bitmap, row, column);
                    drawCell(game.Field[row, column], drawPosition);
                }
            }
            gameField.Refresh();
        }

        private void drawCell(int value, Point drawPosition)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            int cellSize = CellStyle.getCellSize(bitmap);
            Color color = CellStyle.getColor(value);
            Brush brush = new SolidBrush(color);

            graphics.FillRectangle(brush, drawPosition.X, drawPosition.Y, cellSize, cellSize);

            if (value != 0)
            {
                drawCellValue(graphics, value, drawPosition);
            }
        }

        private void drawCellValue(Graphics graphics, int value, Point drawPosition)
        {
            if (value == 0)
            {
                throw new UnsopportedCellValueException("Zero must not be displayed.");
            }

            Font font = CellStyle.getFont(value);
            Point margin = CellStyle.getValueMargin(value);
            Brush brush = new SolidBrush(Color.White);
            float x = drawPosition.X + margin.X;
            float y = drawPosition.Y + margin.Y;
            graphics.DrawString(value.ToString(), font, brush, x, y);
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (count_step >= 10)
            {
                for (int row = 0; row < Game.FieldSize; row++)
                {
                    for (int column = 0; column < Game.FieldSize; column++)
                    {
                        game.Field[row, column] = game.Old_s[row, column];
                        game.New_s[row, column] = game.Old_s[row, column];
                        int cellValue = game.Old_s[row, column];
                        Point drawPosition = CellStyle.getDrawPosition(bitmap, row, column);
                        drawCell(game.Old_s[row, column], drawPosition);
                    }
                }
                count_step = 0;
            }
            else
            {
                int func_act_after = 10;
                if (count_step != 10 && count_step != 0)
                    MessageBox.Show("Данная функция станет доступна через: " + (func_act_after - count_step) + " ходов");
            }
            gameField.Refresh();
        }

        private void оИгреToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help form2 = new Help();
            form2.Show();
        }
    }
}
    
