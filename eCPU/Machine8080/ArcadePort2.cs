using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.Machine8080
{
    class ArcadePort2 : IInputDevice
    {
        [Flags]
        public enum Port2Input
        {
            ShipFlag1 = 0x01,       // DIP SWITCH   # ships
            ShipFlag2 = 0x02,       // DIP SWITCH   00 = 3, 01 = 4, 10 = 5, 11 = 6
            TiltFlag = 0x04,
            ExtraShipFlag = 0x08,   // DIP SWITCH   0 = extra at 1500, 1 = extra at 1000
            Player2Shot = 0x10,
            Player2Left = 0x20,
            Player2Right = 0x40,
            CoinInfoFlag = 0x80     // DIP SWITCH   0 = ON display coin info in demo screen
        };

        private Dictionary<int, Port2Input> _map = new Dictionary<int, Port2Input>();
        private byte _port = 0;
        private object _lock = new object();

        public ArcadePort2(int Player2ShotKey, int Player2LeftKey, int Player2RightKey)
        {
            _map.Add(Player2ShotKey, Port2Input.Player2Shot );
            _map.Add(Player2LeftKey, Port2Input.Player2Left );
            _map.Add(Player2RightKey, Port2Input.Player2Right );
        }

        public void KeyDown(int key)
        {
            Port2Input value;
            if( _map.TryGetValue( key, out value ) )
            {
                lock(_lock)
                    _port = (byte)(_port | (byte)value);
            }
        }

        public void KeyUp(int key)
        {
            Port2Input value;
            if ( _map.TryGetValue( key, out value ) )
            {
                lock(_lock)
                    _port = (byte)(_port & ~(byte)value);
            }
        }

        public byte Read()
        {
            lock(_lock)
                return _port;
        }
    }
}
