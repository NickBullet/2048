using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    [Serializable]
    class Game : ICloneable
    {
        private readonly R_step_back step = new R_step_back();//rus
        private static readonly int fieldSize = 4;

        private int[,] field;
        private int[,] new_s;
        private int[,] old_s;

        public int currentScore { get; set; }
        public int bestScore { get; set; }
        private bool isContinueAfter2048;

        public Game()
        {
            initField();
            new_s = new int[fieldSize, fieldSize];
            old_s = new int[fieldSize, fieldSize];
            isContinueAfter2048 = false;
        }

        public void restart()
        {
            initField();
            currentScore = 0;
            isContinueAfter2048 = false;
        }

        public bool makeTurn(Direction direction)
        {
            bool fieldChanged = move(direction);
            if (fieldChanged)
            {
                addNewCell();
            }
            return isGameOver();
        }

        private void initField()
        {
            field = new int[fieldSize, fieldSize];
            addNewCell();
            addNewCell();
        }

        private void addNewCell()
        {
            Random random = new Random();
            int row;
            int column;

            do
            {
                row = random.Next(0, fieldSize);
                column = random.Next(0, fieldSize);
            } while (!isEmpty(row, column));


            if (random.Next(1, 101) >= 90)
            {
                field[row, column] = 4;
            }
            else
            {
                field[row, column] = 2;
            }
        }

        private bool move(Direction direction)
        {
            step.new_step(old_s, new_s);
            bool isUpdated = false;
            bool isAlongRow = direction == Direction.Left || direction == Direction.Right;
            bool isIncreasingDirection = direction == Direction.Left || direction == Direction.Up;

            // The inner loop boundaries.
            int innerStart = isIncreasingDirection ? 0 : fieldSize - 1;
            int innerEnd = isIncreasingDirection ? fieldSize - 1 : 0;
            Func<int, bool> inInnerBoundaries =
                index => Math.Min(innerStart, innerEnd) <= index && index <= Math.Max(innerStart, innerEnd);

            Func<int, int> prev = isIncreasingDirection
                ? new Func<int, int>(innerIndex => innerIndex - 1)
                : new Func<int, int>(innerIndex => innerIndex + 1);

            Func<int, int> next = isIncreasingDirection
                ? new Func<int, int>(innerIndex => innerIndex + 1)
                : new Func<int, int>(innerIndex => innerIndex - 1);

            Func<int[,], int, int, int> getValue = isAlongRow
                ? new Func<int[,], int, int, int>((x, i, j) => x[i, j])
                : new Func<int[,], int, int, int>((x, i, j) => x[j, i]);

            Action<int[,], int, int, int> setValue = isAlongRow
                ? new Action<int[,], int, int, int>((x, i, j, v) => x[i, j] = v)
                : new Action<int[,], int, int, int>((x, i, j, v) => x[j, i] = v);

            for (int i = 0; i < fieldSize; i++)
            {
                step.new_step(old_s, new_s);
                for (int j = innerStart; inInnerBoundaries(j); j = next(j))
                {
                    if (getValue(field, i, j) != 0)
                    {
                        int newJ = j;
                        do
                        {
                            newJ = prev(newJ);
                        } while (inInnerBoundaries(newJ) && getValue(field, i, newJ) == 0);

                        if (inInnerBoundaries(newJ) && getValue(field, i, newJ) == getValue(field, i, j))
                        {
                            int newValue = getValue(field, i, newJ) * 2;
                            setValue(field, i, newJ, newValue);
                            setValue(field, i, j, 0);
                            isUpdated = true;

                            if (newValue >= 2048 && !isContinueAfter2048)
                            {
                                DialogResult result;
                                result = MessageBox.Show("You achieved 2048. Do you want to continue the game?",
                                    "Congratulations!", MessageBoxButtons.YesNo);
                                if (result == System.Windows.Forms.DialogResult.No)
                                { restart(); }
                                else { isContinueAfter2048 = true; }
                            }

                            currentScore += newValue;
                        }
                        else
                        {
                            newJ = next(newJ);
                            if (newJ != j)
                            {
                                int value = getValue(field, i, j);
                                setValue(field, i, j, 0);
                                setValue(field, i, newJ, value);
                                isUpdated = true;
                            }
                        }
                    }
                }
            }
            step.new_step(New_s, field);

            return isUpdated;
        }

        private bool isGameOver()
        {
            Game clone = (Game)Clone();
            var directions = Enum.GetValues(typeof(Direction));
            foreach (Direction direction in directions)
            {
                bool fieldChanged = clone.move(direction);
                if (fieldChanged)
                {
                    return false;
                }
            }
            if (currentScore > bestScore)
            { bestScore = currentScore; }

            return true;
        }

        private bool isEmpty(int row, int column)
        {
            return field[row, column] == 0;
        }

        public static int FieldSize
        {
            get
            {
                return fieldSize;
            }
        }

        public int[,] Field
        {
            get
            {
                return field;
            }

        }

        public int[,] New_s
        {
            get
            {
                return new_s;
            }

        }

        public int[,] Old_s
        {
            get
            {
                return old_s;
            }

        }

        public object Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);
                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }
    }
}
