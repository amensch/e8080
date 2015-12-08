using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCPU
{
    interface ICodeDisassembler
    {
        string Disassemble(ICodeLoader loader);
    }
}
