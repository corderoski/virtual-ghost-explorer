using System;
using System.Windows.Forms;

namespace VGExplorerTool
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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.FileCreator());
        }

        internal const String APP_NAME = "Virtual Ghost Explorer";

    }
}
