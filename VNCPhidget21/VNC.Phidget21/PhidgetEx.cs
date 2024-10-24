using System;
using System.Threading.Tasks;
using System.Threading;
using Phidgets.Events;

using Prism.Events;

using VNC.Phidget.Players;

namespace VNC.Phidget
{
    public class PhidgetEx : Phidgets.Phidget
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="ipAddress">IP Address of Host</param>
        /// <param name="port">Port number of Host</param>
        public PhidgetEx(string ipAddress, int port, int serialNumber)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            Host = new Host { IPAddress = ipAddress, Port = port };
            SerialNumber = serialNumber;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Host Host { get; set; }
        public int SerialNumber { get; set; }

        public bool LogPhidgetEvents { get; set; }

        #endregion

        #region Event Handlers

        public void Phidget_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_ServerDisconnect {device.Address}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        public void Phidget_ServerConnect(object sender, ServerConnectEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal virtual void Phidget_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_Attach {device.Address},{device.Port} S#:{device.SerialNumber} ID:{device.ID}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        public void Phidget_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        public void Phidget_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;

                Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Commands (None)


        #endregion

        #region Public Methods (None)

        // NOTE(crhodes)
        // This did not work.  Had to go back to opening type'd Phidget.  See AdvancedServoEx, InterfaceKitEx, StepperEx
        ///// <summary>
        ///// Open Phidget and waitForAttachment
        ///// </summary>
        ///// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        //public void Open(Int32? timeOut = null)
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        open(SerialNumber, Host.IPAddress, Host.Port);

        //        if (timeOut is not null) { waitForAttachment((Int32)timeOut); }
        //        else { waitForAttachment(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public void Close()
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)



        #endregion
    }
}
