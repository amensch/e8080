using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eCPU.Intel8080;
using System.IO;
using System.Diagnostics;
using eCPU.Machine8080;

namespace eCPU
{
    public partial class MainForm : Form
    {
        private long _count;
        private long _cycles;

        private SpaceInvaders _invaders = new SpaceInvaders();

        public MainForm()
        {
            InitializeComponent();
        }

        private void TestLoad()
        {
            FileLoader load = new FileLoader();
            string dir = "C:\\Users\\adam\\Downloads\\invaders";
            //string dir = "C:\\Users\\menschas\\Downloads\\invaders";

            load.AddFile(Path.Combine(dir,"invaders.h"));
            load.AddFile(Path.Combine(dir,"invaders.g"));
            load.AddFile(Path.Combine(dir,"invaders.f"));
            load.AddFile(Path.Combine(dir,"invaders.e"));

            //load.AddFile("C:\\Users\\adam\\Downloads\\invaders\\cpudiag.bin");

            //Disassemble8080 dasm = new Disassemble8080();
            //string output = dasm.Disassemble(load);
            //File.WriteAllText("C:\\Users\\adam\\Downloads\\invaders\\dasmtest.txt", output);

            //_invaders.CPU = new i8080();
            //_invaders.CPU.LoadInstructions(load.LoadData(), 0x00);

            //do
            //{
            //    _invaders.CPU.ExecuteNext();
            //    if (_invaders.CPU.Registers.ProgramCounter > 0x2000)
            //    {
            //        count++;
            //    }
            //    count++;
            //} while (true);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            _count = 0;
            _cycles = 0;

            _invaders.LoadProgram(0x00);

            WriteScreen();
        }

        private void WriteScreen()
        {
            txtPC.Text = _invaders.CPU.Registers.ProgramCounter.Value.ToString("X4");
            txtSP.Text = _invaders.CPU.Registers.StackPointer.ToString("X4");
            txtPSW.Text = _invaders.CPU.Registers.RegPSW.ToString("X4");
            txtBC.Text = _invaders.CPU.Registers.RegBC.ToString("X4");
            txtDE.Text = _invaders.CPU.Registers.RegDE.ToString("X4");
            txtHL.Text = _invaders.CPU.Registers.RegHL.ToString("X4");

            string flags = "";


            if (_invaders.CPU.Registers.CondReg.CarryFlag)
                flags += "C";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.ParityFlag)
                flags += "P";
            else
                flags += ".";

            if (_invaders.CPU.Registers.CondReg.ZeroFlag)
                flags += "Z";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.SignFlag)
                flags += "S";
            else
                flags += ".";

            if (_invaders.CPU.Registers.CondReg.AuxCarryFlag)
                flags += "A";
            else
                flags += ".";

            txtFlags.Text = flags;
            txtCount.Text = _count.ToString("N0");
            txtCycles.Text = _cycles.ToString("N0");
            string next;
            UInt32 bytes = Disassemble8080.DisassembleNext(_invaders.CPU.Memory, _invaders.CPU.Registers.ProgramCounter.Value, 0x00, out next);
            txtNext.Text = next;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _count++;
            _cycles += _invaders.CPU.ExecuteNext();
            WriteScreen();
        }

        private string BoolToBit( bool value )
        {
            if (value)
                return "1";
            else 
                return "0";
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            _invaders.Run();
            //_opCount = new long[0x100];
            //_last5 = new byte[5];
            //byte op;

            //for (int i = 0; i <= 0xff; i++)
            //{
            //    _opCount[i] = 0;
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    _last5[i] = 0x00;
            //}

            //do
            //{
            //    op = _invaders.CPU.Memory[_invaders.CPU.Registers.ProgramCounter.Value];
            //    AddLastOp(op);
            //    _opCount[op]++;
            //    _count++;
            //    _cycles += _invaders.CPU.ExecuteNext();

            //} while (_invaders.CPU.Registers.ProgramCounter.Value != 0x031d);

            //WriteScreen();
        }

        //private void AddLastOp( byte op )
        //{
        //    for (int i = 0; i < _last5.Length-1 ; i++)
        //    {
        //        _last5[i] = _last5[i + 1];
        //    }
        //    _last5[_last5.Length - 1] = op;
        //}

        private void btnRunN_Click(object sender, EventArgs e)
        {
            int count = 0;
            if( int.TryParse(txtRun.Text, out count))
            {
                for( int i=0; i < count; i++ )
                {
                    _count++;
                    _cycles += _invaders.CPU.ExecuteNext();
                }
                WriteScreen();  
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            eCPU.Screens.GameWindow frm = new eCPU.Screens.GameWindow(_invaders);
            frm.Show();
        }

        private void btnInt1_Click(object sender, EventArgs e)
        {
            _invaders.CPU.AddInterrupt(1);
        }

        private void btnInt2_Click(object sender, EventArgs e)
        {
            _invaders.CPU.AddInterrupt(2);
        }
    }
}

