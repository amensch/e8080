using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCPU.Intel8080;

namespace eCPU.Machine8080
{
    class SoundDevice : IOutputDevice
    {

        /*
            * Port 3: (discrete sounds)
            * bit 0=UFO (repeats)        SX0 0.raw
            * bit 1=Shot                 SX1 1.raw
            * bit 2=Flash (player die)   SX2 2.raw
            * bit 3=Invader die          SX3 3.raw
            * bit 4=Extended play        SX4
            * bit 5= AMP enable          SX5
            * bit 6= NC (not wired)
            * bit 7= NC (not wired)
            * Port 4: (discrete sounds)
            * bit 0-7 shift data (LSB on 1st write, MSB on 2nd)

            Port 5:
            * bit 0=Fleet movement 1     SX6 4.raw
            * bit 1=Fleet movement 2     SX7 5.raw
            * bit 2=Fleet movement 3     SX8 6.raw
            * bit 3=Fleet movement 4     SX9 7.raw
            * bit 4=UFO Hit              SX10 8.raw
            * bit 5= NC (Cocktail mode control ... to flip screen)
            * bit 6= NC (not wired)
            * bit 7= NC (not wired)
        */

        private delegate void SoundAction();
        private SoundAction[] _actions = new SoundAction[8];

        public SoundDevice()
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
