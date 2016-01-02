using System;
using System.Windows.Forms;

namespace KDS.e8080
{
    public partial class GameWindow : Form
    {

        private IArcadeGame _game;

        public GameWindow()
        {
            InitializeComponent();
            _game = new SpaceInvaders.SpaceInvaders();
            _game.Load();

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbWindow.Image = _game.GetScreen();
        }

        private void runSpaceInvadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _game.Run();
        }

        private void debugSpaceInvadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugWindow frm = new DebugWindow(_game);
            frm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            _game.KeyDown(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _game.KeyUp(e.KeyCode);
        }

    }
}
