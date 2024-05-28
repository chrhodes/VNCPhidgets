using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FxShow.User_Interface;
//using DebugHelp;

//using SQLInformation;

namespace FxShow
{
    ///<summary>
    ///Common items declared at the Class level.
    ///</summary>
    ///<remarks>
    ///Use this class for any thing you want globally available.
    ///Place only Static items in this class.  This Class cannot not be instantiated.
    ///</remarks>    
    public static class Common
    {
        public const string APP_NAME = "FxShow";

        const string TYPE_NAME = "Common";
        public static void Initialize()
        {
#if TRACE
            //Common.WriteToDebugWindow(string.Format("{0}:{1}()", TYPE_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            //SwapScreenBaseControl = null;
            //InitializedUserControls = null;

            // TODO: Add code to (re)Initialize anything that needs to start clear
            // when ucBase.Reload() is called.

        }

        #region Core

        // TODO: Add as many DebugLevels as needed.
        // Add accompanying checkboxes on frmDebugWindow

        public static bool DebugLevel1
        {
            get;
            set;
        }
        public static bool DebugLevel2
        {
            get;
            set;
        }
        public static bool DebugSQL
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the UI is running in DeveloperMode
        /// </summary>
        public static bool DeveloperMode
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the UI is running in DebugMode
        /// </summary>
        public static bool DebugMode
        {
            get;
            set;
        }

        //private static OutputWindow _outputWindow;

        //public static OutputWindow OutputWindow
        //{
        //    get
        //    {
        //        if(_outputWindow == null)
        //        {
        //            _outputWindow = new OutputWindow();
        //        }
        //        return _outputWindow;
        //    }
        //    set
        //    {
        //        _outputWindow = value;
        //    }
        //}

        //public static long WriteToDebugWindow(string message)
        //{
        //    return OutputWindow.Write(message);
        //}

        //public static long WriteToDebugWindow(string message, long startTime)
        //{
        //    return OutputWindow.Write(message, startTime);
        //}

        #endregion

        //private static Data.ApplicationDataSet _ApplicationDataSet;
        //public static Data.ApplicationDataSet ApplicationDataSet
        //{
        //    get
        //    {
        //        if (_ApplicationDataSet == null)
        //        {
        //            _ApplicationDataSet = new Data.ApplicationDataSet();

        //            // TODO: Add any other initialization of things related to the ApplicationDS
        //        }

        //        return _ApplicationDataSet;
        //    }
        //    set
        //    {
        //        _ApplicationDataSet = value;
        //    }
        //}

        // TODO(crhodes): Get rid of this (I think) and use the one from SQLInformation.  See if need anything else
        // in  a DataSet first.  May want a separate one for the App.
        //private static SQLInformation.Data.ApplicationDataSet _ApplicationDataSet;
        //public static SQLInformation.Data.ApplicationDataSet ApplicationDataSet
        //{
        //    get
        //    {
        //        if (_ApplicationDataSet == null)
        //        {
        //            //_ApplicationDataSet = new SQLInformation.Data.ApplicationDataSet();
        //            _ApplicationDataSet = SQLInformation.Common.ApplicationDataSet;

        //            // TODO: Add any other initialization of things related to the ApplicationDS
        //        }

        //        return _ApplicationDataSet;
        //    }
        //    set
        //    {
        //        _ApplicationDataSet = value;
        //    }
        //}

        private static Data.ApplicationDS _ApplicationDataSet;
        public static Data.ApplicationDS ApplicationDataSet
        {
            get
            {
                if (_ApplicationDataSet == null)
                {
                    //_ApplicationDataSet = new SQLInformation.Data.ApplicationDataSet();
                    _ApplicationDataSet = new FxShow.Data.ApplicationDS();

                    // TODO: Add any other initialization of things related to the ApplicationDS
                }

                return _ApplicationDataSet;
            }
            set
            {
                _ApplicationDataSet = value;
            }
        }
    }
}
