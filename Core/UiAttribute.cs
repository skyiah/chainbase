using System;

namespace Greatbone.Core
{
    /// 
    /// To specify user interaction related attributes and behaviors.
    ///
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class UiAttribute : Attribute
    {
        public UiAttribute() { }

        public UiAttribute(string label, string icon = null, int modal = 0)
        {
            Label = label;
            Icon = icon;
            Modal = modal;
        }

        public string Label { get; set; }

        public string Icon { get; set; }

        public int Modal { get; set; }
    }
}