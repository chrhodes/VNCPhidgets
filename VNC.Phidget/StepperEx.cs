using System;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget
{
    public class StepperEx : PhidgetEx //Stepper
    {
        #region Constructors, Initialization, and Load

        public readonly IEventAggregator EventAggregator;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public StepperEx(string ipAddress, int port, int serialNumber, IEventAggregator eventAggregator)
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            InitializePhidget();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            Stepper = new Phidgets.Stepper();

            this.Stepper.Attach += Phidget_Attach;
            this.Stepper.Detach += Phidget_Detach;
            this.Stepper.Error += Phidget_Error;
            this.Stepper.ServerConnect += Phidget_ServerConnect;
            this.Stepper.ServerDisconnect += Phidget_ServerDisconnect;

            //this.InputChange += Stepper_InputChange;
            //this.OutputChange += Stepper_OutputChange;
            //this.SensorChange += Stepper_SensorChange;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.Stepper Stepper = null;

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }

        #endregion

        #region Commands (None)

        #endregion

        #region Event Handlers

        //private void Stepper_SensorChange(object sender, SensorChangeEventArgs e)
        //{
        //    if (LogSensorChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    if (LogOutputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    if (LogInputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        public new void Open(Int32? timeOut = null)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                Stepper.open(SerialNumber, Host.IPAddress, Host.Port);

                if (timeOut is not null) { Stepper.waitForAttachment((Int32)timeOut); }
                else { Stepper.waitForAttachment(); }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                this.Stepper.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(StepperSequence stepperSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogPerformanceSequence) Log.Trace("Enter", Common.LOG_CATEGORY);

                if (stepperSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < stepperSequence.ActionLoops; actionLoop++)
                    {
                        Log.Trace($"Loop:{actionLoop + 1}", Common.LOG_CATEGORY);

                        if (stepperSequence.ExecuteActionsInParallel)
                        {
                            if (LogPerformanceSequence) Log.Trace($"Parallel Actions Loop:{actionLoop + 1}", Common.LOG_CATEGORY);

                            await PlaySequenceActionsInParallel(stepperSequence);
                        }
                        else
                        {
                            if (LogPerformanceSequence) Log.Trace($"Sequential Actions Loop:{actionLoop + 1}", Common.LOG_CATEGORY);

                            await PlaySequenceActionsInSequence(stepperSequence);
                        }
                    }
                }

                if (LogPerformanceSequence) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        private Task PlaySequenceActionsInParallel(StepperSequence stepperSequence)
        {
            throw new NotImplementedException();
        }

        private Task PlaySequenceActionsInSequence(StepperSequence stepperSequence)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
