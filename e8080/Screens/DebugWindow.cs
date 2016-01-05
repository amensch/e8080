using System;
using System.Windows.Forms;
using System.Drawing;
using KDS.e8080;

namespace KDS.e8080
{
    public partial class DebugWindow : Form
    {
        private IArcadeGame _game;

        public DebugWindow(IArcadeGame game)
        {
            InitializeComponent();
            listOps.DrawColumnHeader += listView1_DrawColumnHeader;
            listOps.DrawItem += listView1_DrawItem;
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
            FillListView();
        }

        private void FillListView()
        {
            UInt32 pc = _game.CPU.Registers.ProgramCounter.Value;
            UInt32 this_pc = pc;
            string text;

            listOps.SuspendLayout();

            // fill the listbox with the next 20 instructions
            listOps.Items.Clear();

            for( int i=0; i<13; i++ )
            {
                pc += Disassemble8080.DisassembleNext(_game.CPU.Memory, pc, 0x00, out text);
                listOps.Items.Add(new ListViewItem(new string[] { "$" + this_pc.ToString("X4"), text.Substring(5).Replace("\t"," ") }));
                this_pc = pc;
            }
            listOps.ResumeLayout();
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
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

