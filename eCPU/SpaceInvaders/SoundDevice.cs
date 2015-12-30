using eCPU.Intel8080;
using System.Diagnostics;
using System.Media;

namespace eCPU.SpaceInvaders
{
    class SoundDevice : IOutputDevice
    {

        /*
            * Port 3: (discrete sounds)
            * bit 0=UFO (repeats)        SX0 0.raw                  //ufo.wav
            * bit 1=Shot                 SX1 1.raw                  //shoot.wav
            * bit 2=Flash (player die)   SX2 2.raw                  //explosion.wav
            * bit 3=Invader die          SX3 3.raw                  //invaderkilled.wav
            * bit 4=Extended play        SX4
            * bit 5= AMP enable          SX5
            * bit 6= NC (not wired)
            * bit 7= NC (not wired)

            Port 5:
            * bit 0=Fleet movement 1     SX6 4.raw                  //fastinvader1.wav
            * bit 1=Fleet movement 2     SX7 5.raw                  //fastinvader2.wav
            * bit 2=Fleet movement 3     SX8 6.raw                  //fastinvader3.wav
            * bit 3=Fleet movement 4     SX9 7.raw                  //fastinvader4.wav
            * bit 4=UFO Hit              SX10 8.raw                 //UfoHit.wav
            * bit 5= NC (Cocktail mode control ... to flip screen)
            * bit 6= NC (not wired)
            * bit 7= NC (not wired)
        */

        private delegate void SoundAction();
        private SoundAction[] _actions = new SoundAction[8];

        private int _port;
        private byte _lastdata;

        public SoundDevice(int port)
        {
            _port = port;
            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i] = () => { };
            }

            if(_port == 3)
            {
                _actions[0] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\ufo.wav");
                    player.Play();
                };
                _actions[1] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\shoot.wav");
                    player.Play();
                };
                _actions[2] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\explosion.wav");
                    player.Play();
                };
                _actions[3] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\invaderkilled.wav");
                    player.Play();
                };
            }
            if (_port == 5)
            {
                _actions[0] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\fastinvader1.wav");
                    player.Play();
                };
                _actions[1] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\fastinvader2.wav");
                    player.Play();
                };
                _actions[2] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\fastinvader3.wav");
                    player.Play();
                };
                _actions[3] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\fastinvader4.wav");
                    player.Play();
                };
                _actions[4] = () =>
                {
                    SoundPlayer player = new SoundPlayer("SpaceInvaders\\Sounds\\UfoHit.wav");
                    player.Play();
                };
            }
        }

        public void Write(byte data)
        {
            if (data != _lastdata)
            {
                Debug.WriteLine("Received " + data.ToString("X2") + " on port " + _port.ToString());

                byte test = 0x01;
                for( int bitnumber = 0; bitnumber < 8; bitnumber++ )
                {
                    // only play the sound on the bit transition from low to high
                    // if the bit is already high do not play the sound again
                    if( (byte)(data & test) == test && (byte)(_lastdata & test ) != test )
                    {
                        _actions[bitnumber]();
                    }
                    test = (byte)(test << 1);
                }
                _lastdata = data;
            }
        }

    }
}
