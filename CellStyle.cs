using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    public class CellStyle
    {
        private static readonly int cellMargin = 5;
        private static readonly IDictionary<int, Color> colors = new Dictionary<int, Color>()
        {
            { 0, Color.BurlyWood },
            { 2, Color.LightSalmon },
            { 4, Color.Peru },
            { 8, Color.Chocolate },
            { 16, Color.Gray },
            { 32, Color.DarkSeaGreen },
            { 64, Color.Gold },
            { 128, Color.HotPink },
            { 256, Color.DarkOrange },
            { 512, Color.LightPink },
            { 1024, Color.DarkRed },
            { 2048, Color.Red },
            { 4096, Color.Navy },
            { 8192, Color.Teal },
        };

        public static Point getDrawPosition(Bitmap bitmap, int row, int column)
        {
            int cellHeightWithMargin = bitmap.Height / Game.FieldSize;
            int cellWidthWithMargin = bitmap.Width / Game.FieldSize;
            int x = column * cellHeightWithMargin + cellMargin;
            int y = row * cellHeightWithMargin + cellMargin;
            return new Point(x, y);
        }

        public static int getCellSize(Bitmap bitmap)
        {
            return bitmap.Height / Game.FieldSize - 2 * cellMargin;
        }

        public static Color getColor(int value)
        {
            return colors[value];
        }

        public static Point getValueMargin(int value)
        {
            int digitCount = value.ToString().Length;
            int xMargin;
            int yMargin;

            switch (digitCount)
            {
                case 1:
                    xMargin = 22;
                    yMargin = 17;
                    break;
                case 2:
                    xMargin = 8;
                    yMargin = 17;
                    break;
                case 3:
                    xMargin = 0;
                    yMargin = 20;
                    break;
                case 4:
                    xMargin = -4;
                    yMargin = 23;
                    break;
                default:
                    throw new UnsopportedCellValueException("Invalid digit amount.");
            }

            return new Point(xMargin, yMargin);
        }

        public static Font getFont(int value)
    }
}
