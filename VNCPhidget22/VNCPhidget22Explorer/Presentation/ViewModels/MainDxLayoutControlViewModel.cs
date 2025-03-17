﻿using System;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class MainDxLayoutControlViewModel : ViewModelBase, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public MainDxLayoutControlViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            Title = "VNCLogViewer - MainDxLayoutControl";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        //private string _title = "VNCLogViewer - MainDxLayoutControl";

        //public string Title
        //{
        //    get => _title;
        //    set
        //    {
        //        if (_title == value)
        //            return;
        //        _title = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Commands (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        #region IInstanceCount

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
