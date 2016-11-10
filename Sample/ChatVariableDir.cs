﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Greatbone.Core;

namespace Greatbone.Sample
{

    /// /123/Get
    /// /123/Put
    public class ChatVariableDir : WebDir, IVariable
    {
        private ConcurrentDictionary<string, List<Chat>> online;

        public ChatVariableDir(WebDirContext ctx) : base(ctx)
        {
        }

        ///<summary>Returns the administration UI.</summary>
        [CheckAdmin]
        public void @default(WebContext wc, string subscript)
        {
        }

        [CheckSelf]
        public void get(WebContext wc, string subscpt)
        {
            List<Chat> chats;
            if (online.TryGetValue(subscpt, out chats)) // put in session
            {
                // return cached msgs
            }
            else
            {
                // database operation
                using (var dc = Service.NewDbContext())
                {
                    dc.QueryA("SELECT * FROM chats WHERE to=@to", p => p.Put("@to", subscpt));
                    // load into memory
                }
            }

            // wc.Response.SetContentAsJson(chats)
        }

        public void put(WebContext wc, string subscpt)
        {
            IPrincipal tok = wc.Principal;
            string sender = tok.Key;
            string text = wc.ToString();

            List<Chat> chats;
            if (online.TryGetValue(subscpt, out chats)) // put in session
            {
                Chat chat = chats.First(c => c.partner.Equals(sender));
                chats[0].Put(text);
            }
            else // put in database
            {
                using (var sc = Service.NewDbContext())
                {
                    sc.Execute("INSERT INTO chats (from, to, ) VALUES () ON CONFLICT DO UPDATE", p => { });
                }
            }
        }

    }

}