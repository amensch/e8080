using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace eCPU.Screens
{
    public partial class GameWindow : Form
    {
        private const int IMAGE_WIDTH = 256;
        private const int IMAGE_HEIGHT = 224;

        private Image _screen;
        private SpaceInvaders.SpaceInvaders _invaders;

        public GameWindow()
        {
            InitializeComponent();
            InitWindow();

            _invaders = new SpaceInvaders.SpaceInvaders();
            _invaders.AttachDrawDelegate(DrawGame);

            _invaders.Load();

            timer1.Enabled = true;
        }

        private void InitWindow()
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

        private void DrawGame()
        {
            byte[] vram = _invaders.GetVideoRAM();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_screen != null)
                pbWindow.Image = _screen;
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
