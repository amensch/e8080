using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using eCPU.Machine8080;
using System.Runtime.InteropServices;

namespace eCPU.Screens
{
    public partial class GameWindow : Form
    {
        private const int IMAGE_WIDTH = 256;
        private const int IMAGE_HEIGHT = 224;
        private const UInt32 IMAGE_BUFFER_START = 0x2400;
        private const UInt32 IMAGE_BUFFER_END = 0x3fff;

        private object _lock = new object();

        SpaceInvaders _invaders;

        public GameWindow(SpaceInvaders inv)
        {
            InitializeComponent();

            _invaders = inv;
            _invaders.AttachDrawDelegate(DrawGame);

            InitWindow();

        }


        private void InitWindow()
        {
            Bitmap bmp = new Bitmap( IMAGE_WIDTH, IMAGE_HEIGHT );
            using( Graphics g = Graphics.FromImage( bmp ) )
            {
                g.FillRectangle( Brushes.Black, 0, 0, IMAGE_WIDTH, IMAGE_HEIGHT );
            }
            pbWindow.Image = bmp;
        }

        private void DrawGame()
        {
            byte[] vram = _invaders.GetVideoRAM();
            GCHandle gc = GCHandle.Alloc(vram, GCHandleType.Pinned);

            Array.Reverse(vram);
            gc.Free();

            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(vram, 0);
            Bitmap bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, 32, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr);

            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pbWindow.Image = bmp;
        }

    }
}
