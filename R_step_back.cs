using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    [Serializable]
    class R_step_back
    {

        public void new_step(int[,] new_s, int[,] field)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (field[i, j] != 0)
                        new_s[i, j] = field[i, j];
                    else
                        new_s[i, j] = 0;
                }
            }
        }
        public void delete(int[,] new_s)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    new_s[i, j] = 0;
                }
            }
        }
    }
}
