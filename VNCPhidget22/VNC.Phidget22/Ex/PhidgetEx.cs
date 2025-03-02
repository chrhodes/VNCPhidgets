﻿using System;
using System.Runtime.CompilerServices;

using Phidget22.Events;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    // FIX(crhodes)
    // Not sure we need to inherit from Phidgets.Phidget as the AdvancedServo, InterfaceKit, and Stepper no longer exist
    public class PhidgetEx // : Phidgets.Phidget
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the PhidgetEx class.
        /// </summary>
        public PhidgetEx(int serialNumber)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // 
            //Host = new Host { IPAddress = ipAddress, Port = port };
            SerialNumber = serialNumber;

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // FIX(crhodes)
        // Not sure we need ipAddress and port anymore
        /// <summary>
        /// Initializes a new instance of the PhidgetEx class.
        /// </summary>
        /// <param name="ipAddress">IP Address of Host</param>
        /// <param name="port">Port number of Host</param>
        public PhidgetEx(string ipAddress, int port, int serialNumber)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter {ipAddress},{port} {serialNumber}", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // 
            Host = new Host { IPAddress = ipAddress, Port = port };
            SerialNumber = serialNumber;

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public Host Host { get; set; }
        public int SerialNumber { get; set; }

        public Phidgets.Phidget PhysicalPhidget
        {
            get;
            set;
        }

        public bool LogPhidgetEvents { get; set; }

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

        protected virtual void PhidgetDeviceAttached()
        {
            //Int32 a = 2;
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

        internal virtual void Phidget_Attach(object sender, AttachEventArgs e)
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

            // TODO(crhodes)
            // How will we use this?  Maybe configuration

            if (PhysicalPhidget is null)
            {
                var s = sender;

                PhysicalPhidget = ((Phidgets.Phidget)sender).Parent;

                PhidgetDeviceAttached();
                //OnPhidgetDeviceAttached(new EventArgs());
            }
        }

        public void Phidget_Detach(object sender, DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // FIX(crhodes)
                    // 
                    //Phidget22.Phidget device = (Phidget22.Phidget)e.Device;

                    Log.EVENT_HANDLER($"Phidget_Detach: sender:{sender}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_Error(object sender, ErrorEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // FIX(crhodes)
                    // 
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Channel_PropertyChange(object sender, PropertyChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    // TODO(crhodes)
                    // Figure out what to show here.  Phidget21.Phidget did not have this event.
                    //Phidget22.Phidget device = (Phidget22.Phidget)sender;
                    Log.EVENT_HANDLER($"Phidget_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Channel_DigitalInputStateChange(object sender, DigitalInputStateChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_DigitalInputStateChange: {e.State}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Channel_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_VoltageInputSensorChange: sender:{sender} {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Channel_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Channel_VoltageChange: sender:{sender} {e.Voltage}", Common.LOG_CATEGORY);
                    //Log.EVENT_HANDLER($"Phidget_Error {device.Address},{device.Attached} - type:{e.Type} code:{e.Code} description:{e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Channel_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Channel_VoltageRatioChange: sender:{sender} {e.VoltageRatio}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        internal void Phidget_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_VoltageRatioInputSensorChange: sender:{sender} {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        #endregion

        #region Commands (none)


        #endregion

        #region Public Methods (none)

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

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion
    }
}
