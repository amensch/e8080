using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.Machine8080
{
    public class ShiftDevice : IInputDevice, IOutputDevice
    {
        private UInt16 _shiftReg;
        private ShiftOffsetDevice _shiftOffset;
        private object _lock;

        public ShiftDevice(ShiftOffsetDevice device)
        {
            _shiftReg = 0;
            _shiftOffset = device;
        }

        public byte Read()
        {
            lock (_lock)
            {
                return (byte)((_shiftReg >> (8 - _shiftOffset.Read())) & 0xff);
            }
        }

        public void Write(byte data)
        {
            lock(_lock)
            {
                byte lsb = (byte)((_shiftReg >> 8) & 0xff);
                _shiftReg = (UInt16)(((UInt32)data << 8 | (UInt32)lsb) & 0xffff);
            }
        }
    }
}
