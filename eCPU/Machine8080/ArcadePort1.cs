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
        [Flags]
        private enum Port1Input
        {
            CreditButton = 0x01,
            Player2Start = 0x02,
            Player1Start = 0x04,
            // 0x08 unused
            Player1Shot = 0x10,
            Player1Left = 0x20,
            Player1Right = 0x40
            // 0x80 unused
        };

        private Dictionary<int, Port1Input> _map = new Dictionary<int, Port1Input>();
        private byte _port = 0;
        private object _lock = new object();

        public ArcadePort1(int CreditButtonKey, int Player1StartKey, int Player2StartKey, 
                            int Player1ShotKey, int Player1LeftKey, int Player1RightKey)
        {
            _map.Add(CreditButtonKey, Port1Input.CreditButton);
            _map.Add(Player1StartKey, Port1Input.Player1Start);
            _map.Add(Player2StartKey, Port1Input.Player2Start);
            _map.Add(Player1ShotKey, Port1Input.Player1Shot);
            _map.Add(Player1LeftKey, Port1Input.Player1Left);
            _map.Add(Player1RightKey, Port1Input.Player1Right);
        }

        public void KeyDown(int key)
        {
            Port1Input value;
            if (_map.TryGetValue(key, out value))
            {
                lock (_lock)
                    _port = (byte)(_port | (byte)value);
            }
        }

        public void KeyUp(int key)
        {
            Port1Input value;
            if (_map.TryGetValue(key, out value))
            {
                lock (_lock)
                    _port = (byte)(_port & ~(byte)value);
            }
        }

        public byte Read()
        {
            lock (_lock)
                return _port;
        }
    }
}
