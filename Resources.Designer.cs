﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwitchTV {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TwitchTV.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;weapons&gt;
        ///  &lt;weapon&gt;
        ///    &lt;name&gt;bare fists&lt;/name&gt;
        ///    &lt;keyword&gt;none&lt;/keyword&gt;
        ///    &lt;cost&gt;0&lt;/cost&gt;
        ///    &lt;damage&gt;1&lt;/damage&gt;
        ///  &lt;/weapon&gt;
        ///  &lt;weapon&gt;
        ///    &lt;name&gt;club&lt;/name&gt;
        ///    &lt;keyword&gt;club&lt;/keyword&gt;
        ///    &lt;cost&gt;50&lt;/cost&gt;
        ///    &lt;damage&gt;3&lt;/damage&gt;
        ///  &lt;/weapon&gt;
        ///  &lt;weapon&gt;
        ///    &lt;name&gt;dagger&lt;/name&gt;
        ///    &lt;keyword&gt;dagger&lt;/keyword&gt;
        ///    &lt;cost&gt;60&lt;/cost&gt;
        ///    &lt;damage&gt;4&lt;/damage&gt;
        ///  &lt;/weapon&gt;
        ///  &lt;weapon&gt;
        ///    &lt;name&gt;short sword&lt;/name&gt;
        ///    &lt;keyword&gt;shortSword&lt;/keyword&gt;
        ///    &lt;cos [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string weapons {
            get {
                return ResourceManager.GetString("weapons", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;zones&gt;
        ///  &lt;zone&gt;
        ///    &lt;name&gt;Moe&apos;s Tavern&lt;/name&gt;
        ///    &lt;keyword&gt;tavern&lt;/keyword&gt;
        ///    &lt;command&gt;drink&lt;/command&gt;
        ///    &lt;command&gt;info&lt;/command&gt;
        ///  &lt;/zone&gt;
        ///  &lt;zone&gt;
        ///    &lt;name&gt;Blacksmith&lt;/name&gt;
        ///    &lt;keyword&gt;blacksmith&lt;/keyword&gt;
        ///    &lt;keyword&gt;smith&lt;/keyword&gt;
        ///    &lt;command&gt;shop&lt;/command&gt;
        ///    &lt;command hidden=&quot;true&quot;&gt;buy&lt;/command&gt;
        ///  &lt;/zone&gt;
        ///  &lt;zone&gt;
        ///    &lt;name&gt;Spooky Forest&lt;/name&gt;
        ///    &lt;keyword&gt;forest&lt;/keyword&gt;
        ///    &lt;command&gt;adventure&lt;/command&gt;
        ///  &lt;/zone&gt;
        ///&lt;/zones&gt;.
        /// </summary>
        internal static string zones {
            get {
                return ResourceManager.GetString("zones", resourceCulture);
            }
        }
    }
}
