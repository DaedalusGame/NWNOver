using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWNOver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
#if !DEBUG
            try
            {
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new NWNOver());
#if !DEBUG
            }
            catch (Exception e)
            {
                using (var writer = File.CreateText("crashlog_" + DateTime.Now.ToShortDateString() + ".txt"))
                {
                    writer.WriteLine(e.Source);
                    writer.WriteLine(e.StackTrace);
                    if (e.Data != null)
                        foreach (var value in e.Data)
                        {
                            writer.WriteLine(value);
                        }
                }
            }
#endif
        }
    }
}
