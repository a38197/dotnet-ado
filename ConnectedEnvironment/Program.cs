using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BidSoftware.Shared;

namespace BidSoftware.ConnectedEnvironment
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
            ConnectedService service = new ConnectedService();
            Application.Run(new Shared.UserInterface(service));
        }
    }
}
