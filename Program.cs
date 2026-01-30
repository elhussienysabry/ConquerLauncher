using System;
using System.Windows.Forms;

namespace ConquerLauncher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ShowFatal(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                ShowFatal(ex ?? new Exception("Unhandled exception"));
            };

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                ShowFatal(ex);
            }
        }

        private static void ShowFatal(Exception ex)
        {
            try
            {
                MessageBox.Show($"Unhandled error:\n{ex.GetType()}: {ex.Message}\n\n{ex.StackTrace}", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }
    }
}
