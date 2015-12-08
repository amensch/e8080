using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.Machine8080
{
    class SoundPort : IOutputDevice
    {

        private delegate void SoundAction();
        private SoundAction[] _actions = new SoundAction[8];

        public SoundPort()
        {
            for( int i = 0; i < _actions.Length; i++ )
            {
                _actions[i] = () => { };
            }
        }

        public void Write(byte data)
        {
            for(byte i = 0; i < _actions.Length; i++ )
            {
                if( (byte)(data & (i+1)) == 1 )
                {
                    _actions[i]();
                }
            }
        }

    }
}
