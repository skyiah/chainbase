﻿using System;
using System.IO;
using System.Reflection;

namespace Greatbone.Core
{
    ///
    /// Represents a (sub)controller that consists of a group of action methods, and optionally a folder of static files.
    ///
    public abstract class WebSub : IMember
    {
        // actions declared by this controller
        readonly Set<WebAction> actions;

        // the default action
        readonly WebAction defaction;

        public WebServiceContext Context { get; internal set; }

        ///
        /// The key by which this sub-controller is added to its parent
        ///
        public string Key => Context.key;

        /// <summary>The service that this controller resides in.</summary>
        ///
        public WebService Service => Context.Service;

        public WebSub Parent => Context.Parent;

        public string StaticPath { get; internal set; }

        ///
        /// The corresponding static folder contents, can be null
        ///
        public Set<StaticContent> Statics { get; }

        /// <summary>The default static file in the corresponding folder, can be null</summary>
        ///
        public StaticContent DefaultStatic { get; }

        // the argument makes state-passing more convenient
        protected WebSub(WebServiceContext wsc)
        {
            Context = wsc;

            // initialize the context for the first time
            if (wsc.Service == null)
            {
                WebService svc = this as WebService;
                if (svc == null)
                {
                    throw new InvalidOperationException();
                }
                svc.Context = (WebServiceBuilder)wsc;
                wsc.Service = svc;
            }

            StaticPath = wsc.Parent == null ? Key : Path.Combine(Parent.StaticPath, Key);

            // load static files, if any
            if (StaticPath != null && Directory.Exists(StaticPath))
            {
                Statics = new Set<StaticContent>(256);
                foreach (string path in Directory.GetFiles(StaticPath))
                {
                    string file = Path.GetFileName(path);
                    string ext = Path.GetExtension(path);
                    string ctype;
                    if (StaticContent.TryGetType(ext, out ctype))
                    {
                        byte[] content = File.ReadAllBytes(path);
                        DateTime modified = File.GetLastWriteTime(path);
                        StaticContent sta = new StaticContent
                        {
                            Key = file.ToLower(),
                            Type = ctype,
                            Buffer = content,
                            LastModified = modified
                        };
                        Statics.Add(sta);
                        if (sta.Key.StartsWith("default."))
                        {
                            DefaultStatic = sta;
                        }
                    }
                }
            }

            actions = new Set<WebAction>(32);

            Type type = GetType();

            // introspect action methods
            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterInfo[] pis = mi.GetParameters();
                WebAction a = null;
                if (wsc.IsX)
                {
                    if (pis.Length == 2 && pis[0].ParameterType == typeof(WebContext) &&
                        pis[1].ParameterType == typeof(string))
                    {
                        a = new WebAction(this, mi, true);
                    }
                }
                else
                {
                    if (pis.Length == 1 && pis[0].ParameterType == typeof(WebContext))
                    {
                        a = new WebAction(this, mi, false);
                    }
                }
                if (a != null)
                {
                    if (a.Key.Equals("Default"))
                    {
                        defaction = a;
                    }
                    actions.Add(a);
                }
            }
        }

        public WebAction GetAction(String action)
        {
            if (string.IsNullOrEmpty(action))
            {
                return defaction;
            }
            return actions[action];
        }

        public virtual void Handle(string relative, WebContext wc, string x)
        {

        }
        
        public virtual void Handle(string relative, WebContext wc)
        {
            if (relative.IndexOf('.') != -1) // static handling
            {
                StaticContent sta;
                if (Statics != null && Statics.TryGet(relative, out sta))
                {
                    wc.Response.Content = sta;
                }
                else
                {
                    wc.Response.StatusCode = 404;
                }
            }
            else
            {
                // action handling
                WebAction a = relative.Length == 0 ? defaction : GetAction(relative);
                if (a == null)
                {
                    wc.Response.StatusCode = 404;
                }
                else
                {
                    a.Do(wc);
                }
            }
        }

        public virtual void Default(WebContext wc)
        {
            StaticContent sta = DefaultStatic;
            if (sta != null)
            {
                wc.Response.Content = sta;
            }
            else
            {
                // send not implemented
                wc.Response.StatusCode = 404;
            }
        }

        public virtual void Default(WebContext wc, string x)
        {
            StaticContent sta = DefaultStatic;
            if (sta != null)
            {
                wc.Response.Content = sta;
            }
            else
            {
                // send not implemented
                wc.Response.StatusCode = 404;
            }
        }
    }
}