using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace WordMerger
{
    static class Program
    {
        private static AdvancedFrom MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.GetCommandLineArgs().Contains("-ChLn"))
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length == 3)
                {
                    Properties.Settings.Default.Language = args[2];
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm = new AdvancedFrom();
                SingleInstanceApplication.Run(MainForm, NewInstanceHandler);
            }
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            if (e.CommandLine.Count > 1)
            {
                MainForm.addEntries(e.CommandLine.ToArray(), 1);
            }
            e.BringToForeground = true;
        }
        
    }

    public class SingleInstanceApplication : WindowsFormsApplicationBase
    {
        private SingleInstanceApplication()
        {
            base.IsSingleInstance = true;
        }

        public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
        {
            SingleInstanceApplication app = new SingleInstanceApplication();
            app.MainForm = f;
            app.StartupNextInstance += startupHandler;
            app.Run(Environment.GetCommandLineArgs());
        }
    }
}
