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
            _invaders.LoadProgram(0x00);
            WriteScreen();
        }

        private void WriteScreen()
        {
            txtPC.Text = _invaders.CPU.Registers.ProgramCounter.Value.ToString("X4");
            txtSP.Text = _invaders.CPU.Registers.StackPointer.ToString("X4");
            txtRegA.Text = _invaders.CPU.Registers.RegA.ToString("X2");
            txtBC.Text = _invaders.CPU.Registers.RegBC.ToString("X4");
            txtDE.Text = _invaders.CPU.Registers.RegDE.ToString("X4");
            txtHL.Text = _invaders.CPU.Registers.RegHL.ToString("X4");

            string flags = "";
            if (_invaders.CPU.Registers.CondReg.AuxCarryFlag)
                flags += "A";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.CarryFlag)
                flags += "C";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.ParityFlag)
                flags += "P";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.SignFlag)
                flags += "S";
            else
                flags += ".";
            if (_invaders.CPU.Registers.CondReg.ZeroFlag)
                flags += "Z";
            else
                flags += ".";
            txtFlags.Text = flags;

            txtCount.Text = _invaders.CountOfInstructions.ToString("N0");
            txtCycles.Text = _invaders.CountOfCycles.ToString("N0");
            string next;
            UInt32 bytes = Disassemble8080.DisassembleNext(_invaders.CPU.Memory, _invaders.CPU.Registers.ProgramCounter.Value, 0x00, out next);
            txtNext.Text = next;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _invaders.CPU.ExecuteNext();
            WriteScreen();
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            _invaders.Run();
        }

        private void btnRunN_Click(object sender, EventArgs e)
        {
            int count = 0;
            if( int.TryParse(txtRun.Text, out count))
            {
                _invaders.RunInstructions(count); 
            }
            WriteScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            eCPU.Screens.GameWindow frm = new eCPU.Screens.GameWindow(_invaders);
            frm.Show();
        }
    }
}

