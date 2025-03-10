using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    public class IREx : Phidgets.IR, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IRConfiguration _IRConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new IREx and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="IRConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public IREx(int serialNumber, IRConfiguration IRConfiguration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _IRConfiguration = IRConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<IRSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures IREx using IRConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _IRConfiguration.Channel;
            IsRemote = true;

            Attach += IRExEx_Attach;
            Detach += IRExEx_Detach;
            Error += IRExEx_Error;
            PropertyChange += IRExEx_PropertyChange;

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        // NOTE(crhodes)
        // UI binds to these properties so need to use INPC
        // as UI is bound before Attach fires to update properties from Phidget

        #region Logging

        bool _logPhidgetEvents;
        public bool LogPhidgetEvents
        {
            get { return _logPhidgetEvents; }
            set { _logPhidgetEvents = value; OnPropertyChanged(); }
        }

        bool _logErrorEvents = true;    // probably always want to see Errors
        public bool LogErrorEvents
        {
            get { return _logErrorEvents; }
            set { _logErrorEvents = value; OnPropertyChanged(); }
        }

        bool _logPropertyChangeEvents;
        public bool LogPropertyChangeEvents
        {
            get { return _logPropertyChangeEvents; }
            set { _logPropertyChangeEvents = value; OnPropertyChanged(); }
        }

        bool _logPerformanceSequence;
        public bool LogPerformanceSequence
        {
            get { return _logPerformanceSequence; }
            set { _logPerformanceSequence = value; OnPropertyChanged(); }
        }

        bool _logSequenceAction;
        public bool LogSequenceAction
        {
            get { return _logSequenceAction; }
            set { _logSequenceAction = value; OnPropertyChanged(); }
        }

        bool _logActionVerification;
        public bool LogActionVerification
        {
            get { return _logActionVerification; }
            set { _logActionVerification = value; OnPropertyChanged(); }
        }

        #endregion

        private int _serialNumber;
        public int SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber == value)
                    return;
                _serialNumber = value;
                DeviceSerialNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _attached;
        public bool Attached
        {
            get => _attached;
            set
            {
                if (_attached == value)
                    return;
                _attached = value;
                OnPropertyChanged();
            }
        }

        #region IREx




        #endregion

        #endregion

        #region Event Handlers

        private void IRExEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.IR IR = sender as Phidgets.IR;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"IRExEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //Attached = dOutput.Attached;

            // Just set it so UI behaves well
            Attached = true;

            try
            {

            }
            catch (Phidgets.PhidgetException pex)
            {
                if (pex.ErrorCode != Phidgets.ErrorCode.Unsupported)
                {
                    throw pex;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Exit IRExEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void IRExEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"IRExEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            switch (e.PropertyName)
            {
                case "DataRate":
                    DataRate = base.DataRate;
                    break;

                case "DataInterval":
                    DataInterval = base.DataInterval;
                    break;

                default:
                    Log.EVENT_HANDLER($"IRExEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void IRExEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"IRExEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void IRExEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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

        #region Public Methods

        public new void Open()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Open();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(IRSequence IRSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" IRSequence:>{IRSequence.Name}<" +
                        $" startActionLoopSequences:>{IRSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{IRSequence.ActionLoops}<" +
                        $" actions:>{IRSequence.Actions.Count()}<" +
                        $" actionsDuration:>{IRSequence?.ActionsDuration}<" +
                        $" endActionLoopSequences:>{IRSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (IRSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < IRSequence.ActionLoops; actionLoop++)
                    {
                        if (IRSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PhidgetDeviceSequencePlayer player = PhidgetDeviceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PhidgetDeviceClassSequence sequence in IRSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePhidgetDeviceSequence(sequence);
                            }
                        }

                        if (IRSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(IRSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (IRAction action in IRSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (IRSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{IRSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }

                            Thread.Sleep((Int32)IRSequence.ActionsDuration);
                        }

                        if (IRSequence.EndActionLoopSequences is not null)
                        {
                            PhidgetDeviceSequencePlayer player = new PhidgetDeviceSequencePlayer(_eventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PhidgetDeviceClassSequence sequence in IRSequence.EndActionLoopSequences)
                            {
                                await player.ExecutePhidgetDeviceSequence(sequence);
                            }
                        }
                    }
                }

                if (LogSequenceAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        // FIX(crhodes)
        // 
        private async Task PerformAction(IRAction action)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogSequenceAction)
            {
                startTicks = Log.Trace($"Enter IREx:{Channel}", Common.LOG_CATEGORY);
                actionMessage.Append($"IREx:{Channel}");
            }

            try
            {
                 // NOTE(crhodes)
                 // First make any logging changes

                #region Logging

                if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean)action.LogPhidgetEvents;
                if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean)action.LogErrorEvents;
                if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean)action.LogPropertyChangeEvents;

                if (action.LogPerformanceSequence is not null) LogPerformanceSequence = (Boolean)action.LogPerformanceSequence;
                if (action.LogSequenceAction is not null) LogSequenceAction = (Boolean)action.LogSequenceAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion

                #region IREx Actions

                // TODO(crhodes)
                // Implement

                #endregion

                if (action.Duration > 0)
                {
                    if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        // This is the traditional approach - requires string name to be passed in

        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            long startTicks = 0;
#if LOGGING
            if (Common.VNCCoreLogging.INPC) startTicks = Log.VIEWMODEL_LOW($"Enter ({propertyName})", Common.LOG_CATEGORY);
#endif
            // This is the new CompilerServices attribute!

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#if LOGGING
            if (Common.VNCCoreLogging.INPC) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
#endif
        }

        #endregion
    }
}
