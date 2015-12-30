using System;
using System.Windows.Forms;

namespace eCPU.Screens
{
    public partial class GameWindow : Form
    {

        private SpaceInvaders.SpaceInvaders _invaders;

        public GameWindow()
        {
            InitializeComponent();
            _invaders = new SpaceInvaders.SpaceInvaders();
            _invaders.Load();

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbWindow.Image = _invaders.GetScreen();
        }

        private void runSpaceInvadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _invaders.Run();
        }

        private void debugSpaceInvadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugWindow frm = new DebugWindow(_invaders);
            frm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            _invaders.KeyDown(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _invaders.KeyUp(e.KeyCode);
        }

    }
}
