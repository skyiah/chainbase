﻿using System;

namespace Greatbone.Core
{
    ///
    /// An access check before the target construct is invoked.
    ///
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class CheckAttribute : Attribute
    {
        public WebScope Scope { get; internal set; }

        public virtual bool Check(WebActionContext ac) => true;
    }
}