using System;
using eCPU.Intel8080;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace eCPU.Machine8080
{
    public class SpaceInvaders
    {

        private const byte START_VBLANK_OPCODE = 0xcf;
        private const byte END_VBLANK_OPCODE = 0xd7;

        // number of clock cycles per vblank interrupt.
        private int VBLANK_INTERRUPT = 16666;
        private byte _next_vblank_opcode = END_VBLANK_OPCODE;

        private i8080 _cpu;

        private Stopwatch _sw;
        private Timer _timer;

        private long _lastTimeValue = 0;
        private long _cycleCount = 0;
        private long _instructionCount = 0;
        private long _vblankCount = 0;

        public delegate void DrawDelegate();
        private DrawDelegate _draw;

        // Attach draw function from UI 
        public void AttachDrawDelegate( DrawDelegate drawFunction )
        {
            _draw = drawFunction;
        }

        public i8080 CPU
        {
            get { return _cpu; }
        }

        // Load the program code for execution
        public void LoadProgram()
        {
            _lastTimeValue = 0;
            _cycleCount = 0;
            _instructionCount = 0;
            _vblankCount = 0;
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

        // Main function to begin program execution
        public void Run()
        {
            _sw = new Stopwatch();
            _sw.Start();
            _timer = new Timer();
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
            if (_draw != null) _draw();
        }
        
        public byte[] GetVideoRAM()
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

                if (_draw != null) _draw();
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
