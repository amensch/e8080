using System;
using System.Windows.Forms;

namespace KDS.e8080
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            Application.Run(new GameWindow());

        }
    }
}
