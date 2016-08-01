﻿using System;
using Greatbone.Core;

namespace Greatbone.Sample
{
    public class SiteSpaceHub : WebHub<Space>
    {
        public SiteSpaceHub(WebServiceContext wsc) : base(wsc)
        {
            AddSub<SiteSpaceMgtSub>("mgt", null);
        }


        public override void Default(WebContext wc, Space zone)
        {
            throw new System.NotImplementedException();
        }
    }
}