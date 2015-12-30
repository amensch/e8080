using System;
using System.Windows.Forms;
using eCPU.Intel8080;

namespace eCPU
{
    public partial class DebugWindow : Form
    {
        private SpaceInvaders.SpaceInvaders _invaders;

        public DebugWindow(SpaceInvaders.SpaceInvaders invaders)
        {
            InitializeComponent();
            _invaders = invaders;
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


        private void btnRunN_Click(object sender, EventArgs e)
        {
            int count = 0;
            if( int.TryParse(txtRun.Text, out count))
            {
                _invaders.RunInstructions(count); 
            }
            WriteScreen();
        }

    }
}

