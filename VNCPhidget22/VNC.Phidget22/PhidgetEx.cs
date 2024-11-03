using System;
using System.Runtime.CompilerServices;

using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22
{
    // NOTE(crhodes)
    // Not sure we need to inherit from Phidgets.Phidget as the AdvancedServo, InterfaceKit, and Stepper no longer exist
    public class PhidgetEx // : Phidgets.Phidget
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the PhidgetEx class.
        /// </summary>
        /// <param name="ipAddress">IP Address of Host</param>
        /// <param name="port">Port number of Host</param>
        public PhidgetEx(string ipAddress, int port, int serialNumber)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter {ipAddress},{port} {serialNumber}", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // 
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

        public Phidgets.Phidget PhysicalPhidget 
        { 
            get; 
            set; 
        }

        public bool LogPhidgetEvents { get; 
            set; }

        //public delegate void PhidgetDeviceAttachedType();
        //public event PhidgetDeviceAttachedType PhidgetDeviceAttached;

        //protected virtual void OnPhidgetDeviceAttached()
        //{
        //    PhidgetDeviceAttached?.Invoke();
        //}

        //public event EventHandler PhidgetDeviceAttached;

        //protected virtual void OnPhidgetDeviceAttached(EventArgs e)
        //{
        //    PhidgetDeviceAttached?.Invoke(this, e);
        //}

        protected virtual void PhidgetDeviceIsAttached()
        {
            Int32 a = 2;
        }

        #endregion

        #region Event Handlers

        // FIX(crhodes)
        // 
        //public void Phidget_ServerDisconnect(object sender, Phidget22.Events.ServerDisconnectEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Phidget22.Phidget device = (Phidget22.Phidget)e.Device;

        //            Log.EVENT_HANDLER($"Phidget_ServerDisconnect {device.Address}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //public void Phidget_ServerConnect(object sender, ServerConnectEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Phidget22.Phidget device = (Phidget22.Phidget)e.Device;

        //            Log.EVENT_HANDLER($"Phidget_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        internal virtual void Phidget_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // FIX(crhodes)
                    // 
                    //Phidgets.Phidget device = (Phidgets.Phidget)e.Device;
                    Log.EVENT_HANDLER($"Phidget_Attach:", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Attach {device.Address},{device.Port} S#:{device.SerialNumber} ID:{device.ID}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            if (PhysicalPhidget is null)
            {
                var s = sender;
                
                PhysicalPhidget = ((Phidgets.Phidget)sender).Parent;

                PhidgetDeviceIsAttached();
                //OnPhidgetDeviceAttached(new EventArgs());
            }
        }

        public void Phidget_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // FIX(crhodes)
                    // 
                    //Phidget22.Phidget device = (Phidget22.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_Detach:", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            //if (LogPhidgetEvents)
            //{
                try
                {
                    // FIX(crhodes)
                    // 
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_Error:", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            //}
        }

        internal void Phidget_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // TODO(crhodes)
                    // Figure out what to show here.  Phidget21.Phidget did not have this event.
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_DigitalInputStateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // TODO(crhodes)
                    // Figure out what to show here.  Phidget21.Phidget did not have this event.
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_DigitalInputStateChange: {e.State}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_VoltageInputSensorChange(object sender, PhidgetsEvents.VoltageInputSensorChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // TODO(crhodes)
                    // Figure out what to show here.  Phidget21.Phidget did not have this event.
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_VoltageRatioInputSensorChange(object sender, PhidgetsEvents.VoltageRatioInputSensorChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // TODO(crhodes)
                    // Figure out what to show here.  Phidget21.Phidget did not have this event.
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
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
