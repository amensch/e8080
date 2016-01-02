using System;
using System.Windows.Forms;
using KDS.e8080;

namespace KDS.e8080
{
    public partial class DebugWindow : Form
    {
        private IArcadeGame _game;

        public DebugWindow(IArcadeGame game)
        {
            InitializeComponent();
            _game = game;
        }

        private void WriteScreen()
        {
            txtPC.Text = _game.CPU.Registers.ProgramCounter.Value.ToString("X4");
            txtSP.Text = _game.CPU.Registers.StackPointer.ToString("X4");
            txtRegA.Text = _game.CPU.Registers.RegA.ToString("X2");
            txtBC.Text = _game.CPU.Registers.RegBC.ToString("X4");
            txtDE.Text = _game.CPU.Registers.RegDE.ToString("X4");
            txtHL.Text = _game.CPU.Registers.RegHL.ToString("X4");

            string flags = "";
            if (_game.CPU.Registers.CondReg.AuxCarryFlag)
                flags += "A";
            else
                flags += ".";
            if (_game.CPU.Registers.CondReg.CarryFlag)
                flags += "C";
            else
                flags += ".";
            if (_game.CPU.Registers.CondReg.ParityFlag)
                flags += "P";
            else
                flags += ".";
            if (_game.CPU.Registers.CondReg.SignFlag)
                flags += "S";
            else
                flags += ".";
            if (_game.CPU.Registers.CondReg.ZeroFlag)
                flags += "Z";
            else
                flags += ".";
            txtFlags.Text = flags;

            txtCount.Text = _game.CountOfInstructions.ToString("N0");
            txtCycles.Text = _game.CountOfCycles.ToString("N0");
            string next;
            UInt32 bytes = Disassemble8080.DisassembleNext(_game.CPU.Memory, _game.CPU.Registers.ProgramCounter.Value, 0x00, out next);
            txtNext.Text = next;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _game.RunInstructions(1);
            WriteScreen();
        }


        private void btnRunN_Click(object sender, EventArgs e)
        {
            int count = 0;
            if( int.TryParse(txtRun.Text, out count))
            {
                _game.RunInstructions(count); 
            }
            WriteScreen();
        }

    }
}

