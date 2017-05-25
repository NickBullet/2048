using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    class UnsopportedCellValueException : Exception
    {
        public UnsopportedCellValueException(string message) : base(message)
        {
        }
    }
}
