﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VoltageRatio_Example.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.phidgets.com/docs/Calibrating_Load_Cells")]
        public string calibrationURL {
            get {
                return ((string)(this["calibrationURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Step 1 - Zero Load")]
        public string title0 {
            get {
                return ((string)(this["title0"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Step 2a - Rated Output")]
        public string title1 {
            get {
                return ((string)(this["title1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1. Remove all external weight from the system.\r\n\r\n2. Wait until the measurement i" +
            "s stable and press \"Continue\".")]
        public string state0 {
            get {
                return ((string)(this["state0"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1. If you have a calibration sheet for your load cell(s), enter the rated output " +
            "and the load cell capacity below.  Then press \"Calibrate\".\r\n\r\n2. If you do not h" +
            "ave a calibration sheet for your load cell(s), press \"Skip\".")]
        public string state1 {
            get {
                return ((string)(this["state1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1. Apply a known weight to the system.  This should be as close to the maximum ra" +
            "ting for the system as possible.\r\n\r\n2. Enter the weight into the input box below" +
            ".\r\n\r\n3. Wait until the measurement is stable and press \"Calibrate\".")]
        public string state2 {
            get {
                return ((string)(this["state2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Step 2b - Non-Zero Load")]
        public string title2 {
            get {
                return ((string)(this["title2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.phidgets.com/docs/Calibrating_Load_Cells#Step_2a:_Enter_the_Rated_Out" +
            "put")]
        public string ratedOutputURL {
            get {
                return ((string)(this["ratedOutputURL"]));
            }
        }
    }
}