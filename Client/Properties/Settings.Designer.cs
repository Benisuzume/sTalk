﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<RSAKeyValue><Modulus>7JOWqJNpEXahEQj5riBSplfIdREnO7kZQlBIo2aEXEdSk/lsu1Q2BfrJMT6/E7T4sPYE+jh72zkVsP75DhwSanfZHy1lnRk0LDC/KG1wVmWYUyRXCZB1hKhWKrwqFNWiXKysPNuhB1CzpSd1u66H26cSEu5g7vdeLHcKawTrZu3+IHF4vutOeivc0ibO4+Q9p9Q6oseWOfJ6xJkm3RdwaeOU0GwVH5H1NBbZuvQyPErrQYTb9l/gVSsASt4UEyE1sVtPeZTuwAI3nkdq6PJAmKnHsV2Zl2bsbGLK5tPpk6YIdKxJ5chWgNq/4m+n0Hq9iHY0Ofxuz66v8gWd9cRziw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>")]
        public string PublicKey {
            get {
                return ((string)(this["PublicKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("443")]
        public int ServerPort {
            get {
                return ((int)(this["ServerPort"]));
            }
        }
    }
}