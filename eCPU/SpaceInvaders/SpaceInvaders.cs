using System;
using eCPU.Intel8080;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace eCPU.SpaceInvaders
{
    public class SpaceInvaders
    {

        private const int IMAGE_WIDTH = 256;
        private const int IMAGE_HEIGHT = 224;

        private const byte START_VBLANK_OPCODE = 0xcf;
        private const byte END_VBLANK_OPCODE = 0xd7;

        // number of clock cycles per vblank interrupt.
        private int VBLANK_INTERRUPT = 16666;
        private byte _next_vblank_opcode = END_VBLANK_OPCODE;

        // CPU and I/O devices
        private i8080 _cpu;
        private ArcadePort1 _port1 = new ArcadePort1((int)Keys.C, (int)Keys.D1, (int)Keys.D2, (int)Keys.Up, (int)Keys.Left, (int)Keys.Right);
        private ArcadePort2 _port2 = new ArcadePort2((int)Keys.E, (int)Keys.S, (int)Keys.F);
        private SoundDevice _sound3 = new SoundDevice(3);
        private SoundDevice _sound5 = new SoundDevice(5);

        // Screen Image
        private Image _screen;

        // timers for proper CPU execution time
        private Stopwatch _sw;
        private System.Timers.Timer _timer;

        // statistics counters
        private long _lastTimeValue = 0;
        private long _cycleCount = 0;
        private long _instructionCount = 0;
        private long _vblankCount = 0;

        public i8080 CPU
        {
            get { return _cpu; }
        }

        // Load the program code for execution
        public void Load()
        {
            _lastTimeValue = 0;
            _cycleCount = 0;
            _instructionCount = 0;
            _vblankCount = 0;
            InitScreen();
            LoadProgram(0x00);
        }

        private void InitScreen()
        {
            using (Bitmap bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Black, 0, 0, IMAGE_WIDTH, IMAGE_HEIGHT);
                }
                _screen = (Image)bmp.Clone();
            }
        }

        public Image GetScreen()
        {
            return (Image)_screen.Clone();
        }

        private void DrawScreen()
        {
            byte[] vram = GetVideoRAM();
            GCHandle gc = GCHandle.Alloc(vram, GCHandleType.Pinned);

            Array.Reverse(vram);
            gc.Free();

            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(vram, 0);
            using (Bitmap bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, 32, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                _screen = (Image)bmp.Clone();
            }
        }

        private void LoadProgram(UInt16 startingAddress)
        {
            FileLoader load = new FileLoader();

            load.AddFile("SpaceInvaders\\ROM\\invaders.h");
            load.AddFile("SpaceInvaders\\ROM\\invaders.g");
            load.AddFile("SpaceInvaders\\ROM\\invaders.f");
            load.AddFile("SpaceInvaders\\ROM\\invaders.e");

            //load.AddFile("C:\\Users\\adam\\Downloads\\i8080-emulator-master\\i8080-emulator-master\\diag\\cpudiag.bin");

            //Disassemble8080 dasm = new Disassemble8080();
            //string output = dasm.Disassemble(load, 0x100);
            //File.WriteAllText("C:\\Users\\adam\\Downloads\\i8080-emulator-master\\i8080-emulator-master\\diag\\cpudiagdasm.txt", output);


            _cpu = new i8080();
            _cpu.LoadInstructions(load.LoadData(), startingAddress);

            ShiftOffsetDevice _soDevice = new ShiftOffsetDevice();
            ShiftDevice _device = new ShiftDevice(_soDevice);

            // input device port 1: First Arcade Port
            _cpu.AddInputDevice(_port1, 1);

            // input device port 2: Second Arcade Port
            _cpu.AddInputDevice(_port2, 2);

            // input device port 3: shift register output to cpu
            _cpu.AddInputDevice(_device, 3);

            // output device port 2: shift offset
            _cpu.AddOutputDevice(_soDevice, 2);

            // output device port 3: add sound port 
            _cpu.AddOutputDevice(_sound3, 3);

            // output device port 4: shift register input from cpu
            _cpu.AddOutputDevice(_device, 4);

            // output device port 5: add sound port 
            _cpu.AddOutputDevice(_sound5, 5);
        }

        public void KeyDown(Keys key)
        {
            switch(key)
            {
                case Keys.C:
                case Keys.D1:
                case Keys.D2:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    {
                        _port1.KeyDown((int)key);
                        break;
                    }
                case Keys.E:
                case Keys.S:
                case Keys.F:
                    {
                        _port2.KeyDown((int)key);
                        break;
                    }
            }
        }

        public void KeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.C:
                case Keys.D1:
                case Keys.D2:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    {
                        _port1.KeyUp((int)key);
                        break;
                    }
                case Keys.E:
                case Keys.S:
                case Keys.F:
                    {
                        _port2.KeyUp((int)key);
                        break;
                    }
            }
        }

        // Main function to begin program execution
        public void Run()
        {
            _sw = new Stopwatch();
            _sw.Start();
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            _timer.Interval = 1;
            _timer.Enabled = true;
        }

        // This function runs a specific number of instructions -- intended for debugging
        public void RunInstructions(int numOfInstructions)
        {
            for( int i = 0; i < numOfInstructions; i++ )
            {
                RunOneInstruction();
            }
            DrawScreen();
        }
        
        private byte[] GetVideoRAM()
        {
            byte[] vram = new byte[7168];
            Array.Copy(CPU.Memory, 0x2400, vram, 0, 7168);
            return vram;
        }

        // Called by the timer to run a specific number of clock cycles
        // so the CPU stays on proper timing.
        private void RunCPU()
        {
            _timer.Enabled = false;

            // Determine how much time has elapsed since the last cycle.
            // Then calculate how many CPU clock cycles should be run.
            long newTimeValue = _sw.ElapsedMilliseconds;
            long diffTime = newTimeValue - _lastTimeValue;

            // At 2 Mhz this equals 2000 cycles per millisecond
            long cpuCyclesToRun = 2000 * diffTime;
            while (cpuCyclesToRun > 0)
            {
                cpuCyclesToRun -= RunOneInstruction();
            }

            // Remember the elapsed time for the next interval
            _lastTimeValue = newTimeValue;

            _timer.Enabled = true;

        }

        // Call the CPU to run one instruction
        private int RunOneInstruction()
        {
            int cycles;

            cycles = _cpu.ExecuteNext();

            _cycleCount += cycles;
            _vblankCount += cycles;
            _instructionCount++;

            if (_vblankCount >= VBLANK_INTERRUPT)
            {
                _cpu.AddInterrupt(_next_vblank_opcode);
                _vblankCount = 0;

                if (_next_vblank_opcode == START_VBLANK_OPCODE)
                    _next_vblank_opcode = END_VBLANK_OPCODE;
                else
                    _next_vblank_opcode = START_VBLANK_OPCODE;

                DrawScreen();
            }
            return cycles;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunCPU();
        }

        public long CountOfCycles
        {
            get { return _cycleCount; }
        }

        public long CountOfInstructions
        {
            get { return _instructionCount; }
        }
    }
}
