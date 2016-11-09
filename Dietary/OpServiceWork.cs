﻿using Greatbone.Core;

namespace Ministry.Dietary
{

    ///
    /// <summary>
    /// The operation service controller.
    /// </summary>
    ///
    public class OpService : WebServiceWork
    {
        public OpService(WebConfig cfg) : base(cfg)
        {
            SetVar<ShopVarWork>();
        }

        ///
        /// <summary>
        /// Get all fame categories.
        /// </summary>
        /// <code>
        /// GET /cats
        /// </code>
        ///
        public void cats(WebContext wc, string subscpt)
        {

        }

        public void search(WebContext wc, string subscpt)
        {
        }

        /// Create a new shop
        ///
        public void @new(WebContext wc, string subscpt)
        {
        }

        //
        // MESSAGES
        // 

        public void USER_UPD(MsgContext mc)
        {


        }

        public void RPT_OK(MsgContext mc)
        {

        }


        protected override IPrincipal GetPrincipal(bool token, string idstr)
        {
            if (token) // token
            {
                string plain = StrUtility.Decrypt(idstr, 0x4a78be76, 0x1f0335e2); // plain token
                JsonParse par = new JsonParse(plain);
                try
                {
                    Obj jo = (Obj)par.Parse();
                    // return jo.ToObj<Token>();
                }
                catch { }
            }
            else // username
            {
                // if (logins != null)
                // {
                //     for (int i = 0; i < logins.Length; i++)
                //     {
                //         Login lgn = logins[i];
                //         if (lgn.id.Equals(idstr)) return lgn;
                //     }
                // }
            }
            return null;
        }

        [CheckAdmin]
        public virtual void mgmt(WebContext wc, string subscpt)
        {
            if (Children != null)
            {
                wc.SendMajorLayout(200, "模块管理", a =>
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        WebWork child = Children[i];

                    }
                },
                true);
            }
        }


    }
}