﻿using System;
using System.IO;
using System.Reflection;

namespace Greatbone.Core
{
    ///
    /// <summary>
    /// A multiplexer controller that handles requests targeting variable-keys. 
    /// </summary>
    ///
    public abstract class WebMux : WebWork, IParent
    {
        // child controls
        private Roll<WebWork> children;

        protected WebMux(WebNodeContext wnc) : base(wnc) { }

        public Roll<WebWork> Children => children;

        public W AddChild<W>(string key, object state = null) where W : WebWork
        {
            if (children == null)
            {
                children = new Roll<WebWork>(16);
            }
            // create instance by reflection
            Type typ = typeof(W);
            ConstructorInfo ci = typ.GetConstructor(new[] { typeof(WebNodeContext) });
            if (ci == null) { throw new WebException(typ + ": the constructor with WebTie"); }
            WebNodeContext ctx = new WebNodeContext
            {
                key = key,
                State = state,
                Parent = this,
                HasVar = true,
                Folder = (Parent == null) ? key : Path.Combine(Parent.Folder, key),
                Service = Service
            };
            // call the initialization and add
            W child = (W)ci.Invoke(new object[] { ctx });
            children.Add(child);

            return child;
        }

        internal override void Handle(string relative, WebContext wc)
        {
            int slash = relative.IndexOf('/');
            if (slash == -1) // handle it locally
            {
                DoRsc(relative, wc);
            }
            else // dispatch to child control
            {
                string dir = relative.Substring(0, slash);
                WebWork child;
                if (children != null && children.TryGet(relative, out child))
                {
                    child.Handle(relative.Substring(slash), wc);
                }
            }
        }

    }

}