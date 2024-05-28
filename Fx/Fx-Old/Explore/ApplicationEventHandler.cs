using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Explore
{
    /// <summary>
    /// ApplicationEventHandler: Handles events global to the application, not tied to a specific control.
    /// </summary>
    public static class ApplicationEventHandler
    {
        const string CONTROL_NAME = "ApplicationEventHandler";

        static bool _IsIntialized = false;

        // Create delegates and event variables for anything you want handled
        // at an application (key press) level.  Each event needs to be coupled
        // with a private method that raises the event.

        public delegate void ToggleF11EventHandler();
        public static event ToggleF11EventHandler ToggleF11Event;

        public delegate void ToggleF12EventHandler();
        public static event ToggleF12EventHandler ToggleF12Event;

        /// <summary>
        /// Initialize the Application Event Handlers.
        /// </summary>
        /// <remarks>Called by Form/Control that creates AEH.</remarks>
        public static void Init()
        {
#if TRACE_AE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            if(!_IsIntialized)
            {
                MyMessageFilter myMessageFilter = new MyMessageFilter();
                myMessageFilter.ToggleF12KeyboardEvent += ToggleF12Handler;
                myMessageFilter.ToggleF11KeyboardEvent += ToggleF11Handler;
                Application.AddMessageFilter(myMessageFilter);
                _IsIntialized = true;
            }
            else
            {
                ToggleF11Event = null;
                ToggleF12Event = null;
            }
        }

        /// <summary>
        /// Handles the events raised by the MessageFilter.
        /// Raises events to the application handler.
        /// </summary>
        private static void ToggleF12Handler()
        {
#if TRACE_AE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            //if (ToggleF12Event != null)
            //{
            //    ToggleF12Event();
            //}
            ToggleF12EventHandler temp = Interlocked.CompareExchange(ref ToggleF12Event, null, null);
            if(temp != null) temp();
        }

        /// <summary>
        /// Handles the events raised by the MessageFilter.
        /// Raises events to the application handler.
        /// </summary>
        private static void ToggleF11Handler()
        {
#if TRACE_AE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            //if (ToggleF11Event != null)
            //{
            //    ToggleF11Event();
            //}
            ToggleF11EventHandler temp = Interlocked.CompareExchange(ref ToggleF11Event, null, null);
            if(temp != null) temp();
        }
    }

    public class MyMessageFilter : IMessageFilter
    {
        public delegate void ToggleF12KeyboardEventEventHandler();
        public event ToggleF12KeyboardEventEventHandler ToggleF12KeyboardEvent;

        public delegate void ToggleF11KeyboardEventEventHandler();
        public event ToggleF11KeyboardEventEventHandler ToggleF11KeyboardEvent;

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            switch(m.Msg)
            {
                case WM_KEYDOWN:
                    //Trace.WriteLine("KD")
                    switch(m.WParam.ToInt32())
                    {
                        case (int)Keys.F12:
                            if(ToggleF12KeyboardEvent != null)
                            {
                                ToggleF12KeyboardEvent();
                                //return true;
                            }
                            //Trace.WriteLine("F12");
                            break;

                        case (int)Keys.F11:
                            if(ToggleF11KeyboardEvent != null)
                            {
                                ToggleF11KeyboardEvent();
                                //return true;
                            }
                            //Trace.WriteLine("F11");
                            break;
                    }

                    break;
                case WM_KEYUP:
                    break;
                //Trace.WriteLine("KU")

                case WM_SYSKEYDOWN:
                    break;
                //Trace.WriteLine("SKD")

                case WM_SYSKEYUP:
                    break;
                //Trace.WriteLine("SKU")

            }

            // Pass the message along to the application.
            return false;
        }
    }
}
