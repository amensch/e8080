using System;
using eCPU.Intel8080;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace eCPU.Machine8080
{
    public class SpaceInvaders
    {

        private i8080 _cpu;

        private Stopwatch _sw;
        private Timer _timer;
        private long _lastTimeValue;
        private long _lastInterruptTime;
        private byte _lastInterruptNumber;
        private bool _exit;
        private long _cycleCount;

        public SpaceInvaders()
        {
            _exit = false;
        }



        public i8080 CPU
        {
            get { return _cpu; }
        }

        public void Run()
        {
            _exit = false;
            _cycleCount = 0;
            _lastTimeValue = 0;
            _lastInterruptTime = 0;
            _lastInterruptNumber = 1;
            _sw = new Stopwatch();
            _sw.Start();
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            _timer.Interval = 1;
            _timer.Enabled = true;
        }

        public void LoadProgram()
        {
            LoadProgram(0x00);
        }

        public void LoadProgram(UInt16 startingAddress)
        {
            FileLoader load = new FileLoader();
            string dir = "C:\\Users\\adam\\Downloads\\invaders";

            load.AddFile(Path.Combine(dir, "invaders.h"));
            load.AddFile(Path.Combine(dir, "invaders.g"));
            load.AddFile(Path.Combine(dir, "invaders.f"));
            load.AddFile(Path.Combine(dir, "invaders.e"));

            //load.AddFile("C:\\Users\\adam\\Downloads\\i8080-emulator-master\\i8080-emulator-master\\diag\\cpudiag.bin");

            //Disassemble8080 dasm = new Disassemble8080();
            //string output = dasm.Disassemble(load, 0x100);
            //File.WriteAllText("C:\\Users\\adam\\Downloads\\i8080-emulator-master\\i8080-emulator-master\\diag\\cpudiagdasm.txt", output);


            _cpu = new i8080();
            _cpu.LoadInstructions(load.LoadData(), startingAddress);

            ShiftOffsetDevice _soDevice = new ShiftOffsetDevice();
            ShiftDevice _device = new ShiftDevice(_soDevice);
            
            // shift register output to cpu
            _cpu.AddInputDevice(_device, 3);

            // shift offset
            _cpu.AddOutputDevice(_soDevice, 2);

            // shift register input from cpu
            _cpu.AddOutputDevice(_device, 4);

            // add sound port 3
            _cpu.AddOutputDevice(new SoundDevice(), 3);

            // add sound port 5
            _cpu.AddOutputDevice(new SoundDevice(), 5);

        }
        
        public byte[] GetVideoRAM()
        {
            byte[] vram = new byte[7168];
            Array.Copy(CPU.Memory, 0x2400, vram, 0, 7168);
            return vram;
        }



        private void RunCycle()
        {

            _timer.Enabled = false;

            // Determine how much time has elapsed since the last cycle.
            // Then calculate how many CPU clock cycles should be run.
            long newTimeValue = _sw.ElapsedMilliseconds;
            long diffTime = newTimeValue - _lastTimeValue;

            //Debug.WriteLine("diffTime=" + diffTime.ToString());

            // The CPU interrupts twice for screen drawing at 60hz
            // or 16 2/3 ms.  Determine if enough time has elapsed to
            // create the interrupt.
            if( (newTimeValue - _lastInterruptTime) > 16 )
            {
                _cpu.AddInterrupt(_lastInterruptNumber++);
                _lastInterruptTime = newTimeValue;

                if( _lastInterruptNumber > 2 )
                {
                    _lastInterruptNumber = 1;
                }
            }

            // At 2 Mhz this equals 2000 cycles per millisecond
            long cpuCyclesToRun = 2000 * diffTime;
           // Debug.WriteLine("cpuCyclesToRun=" + cpuCyclesToRun.ToString());
            while (cpuCyclesToRun > 0)
            {
                cpuCyclesToRun -= _cpu.ExecuteNext();
            }

            // Remember the elapsed time for the next interval
            _lastTimeValue = newTimeValue;

            _cycleCount++;
            //if( _cycleCount == 100 )
            //{
            //    _exit = true;
            //}

            //if( _exit )
            //{
            //    _timer.Enabled = false;
            //}

            _timer.Enabled = true;

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunCycle();
        }
    }
}
