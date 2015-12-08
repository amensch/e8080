using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.Machine8080
{
    class ArcadePort1 : IInputDevice
    {
        private byte port;



        public byte Read()
        {
            return port;
        }
    }
}
