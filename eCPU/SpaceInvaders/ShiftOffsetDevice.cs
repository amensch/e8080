using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.SpaceInvaders
{
    public class ShiftOffsetDevice : IOutputDevice
    {
        private object _lock = new object();
        private byte _shiftOffset = 0x00;

        public byte Read()
        {
            lock (_lock)
            {
                return _shiftOffset;
            }
        }

        public void Write(byte data)
        {
            lock(_lock)
            {
                _shiftOffset = (byte)(data & 0x07);
            }
        }
    }
}
