using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCPU.Intel8080
{
    public class i8080ProgramCounter
    {
        public UInt16 Value { get; set; }

        public i8080ProgramCounter()
        {
            Value = 0;
        }

        public void Increment(byte opCode)
        {
            Value += i8080Table.opCodes[opCode].size;
        }

    }
}
