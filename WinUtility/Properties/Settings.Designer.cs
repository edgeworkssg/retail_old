﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.8784
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinUtility.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.\\SQLEXPRESS;AttachDbFilename=\"C:\\Program Files\\Microsoft SQL Server\\" +
            "MSSQL.1\\MSSQL\\Data\\AHAVATMP.mdf\";Integrated Security=True;Connect Timeout=30;Use" +
            "r Instance=True")]
        public string AHAVATMPConnectionString {
            get {
                return ((string)(this["AHAVATMPConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("A3DCC69DBC095EE877D29CA8")]
        public string SensitiveSalt {
            get {
                return ((string)(this["SensitiveSalt"]));
            }
            set {
                this["SensitiveSalt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("123")]
        public string Test {
            get {
                return ((string)(this["Test"]));
            }
            set {
                this["Test"] = value;
            }
        }
    }
}
