using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;


namespace FxShow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static int CLASS_BASE_ERRORNUMBER = ErrorNumbers.FXSHOW;
        private const string LOG_APPNAME = Common.APP_NAME;

        const string TYPE_NAME = "App";

        void ApplicationStartingUp(object sender, StartupEventArgs args)
        {
#if TRACE

#endif
            // Tell other assemblies where to write output messages.
            //SMOHelper.Common.OutputWindow = Common.OutputWindow;

            //User_Interface.Windows.MainWindow _window1 = new User_Interface.Windows.MainWindow();
            //User_Interface.Windows.MainWindowSkeleton _window1 = new User_Interface.Windows.MainWindowSkeleton();
            //User_Interface.Windows.MainRibbonWindow _window1 = new User_Interface.Windows.MainRibbonWindow();
            //User_Interface.Windows.Window1 _window1 = new User_Interface.Windows.Window1();
            //User_Interface.Windows.Window2 _window1 = new User_Interface.Windows.Window2();
            User_Interface.Windows.wndDX_MainWindow _window1 = new User_Interface.Windows.wndDX_MainWindow();

            String windowArgs = string.Empty;
            // Check for arguments; if there are some build the path to the package out of the args.
            if (args.Args.Length > 0 && args.Args[0] != null)
            {
                for (int i = 0; i < args.Args.Length; ++i)
                {
                    windowArgs = args.Args[i];
                    switch (i)
                    {
                        case 0: // Patient Id
                            //patientId = windowArgs;
                            break;
                    }
                }
            }

            try
            {
                _window1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.InnerException.ToString());
            }
#if TRACE

#endif
        }

        private void Application_SessionEnding_1(object sender, SessionEndingCancelEventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("Application_SessionEnding_1");
#endif
            //MessageBox.Show("Application_SessionEnding_1");
            //Common.OutputWindow.Close();
            //Common.OutputWindow = null;
        }

        private void Application_Exit_1(object sender, ExitEventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("Application_Exit_1");
#endif
            //MessageBox.Show("Application_Exit_1");
            //Common.OutputWindow.Close();
            //Common.OutputWindow = null;
        }

        private void Application_Deactivated_1(object sender, EventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("Application_Deactivated_1");
#endif
            //Common.OutputWindow.Close();
            //Common.OutputWindow = null;
        }

        private void Application_Activated_1(object sender, EventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("Application_Activated_1");
#endif

        }

    }
}
