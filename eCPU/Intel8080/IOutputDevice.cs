using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCPU.Intel8080
{
    public interface IOutputDevice
    {
        void Write(byte data);
    }
}
