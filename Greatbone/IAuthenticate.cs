﻿using System.Threading.Tasks;

namespace Greatbone
{
    /// <summary>
    /// To implement authentication behavior on a service class.
    /// </summary>
    public interface IAuthenticate
    {
        bool Authenticate(WebContext ac);
    }

    /// <summary>
    /// To implement asynchronous authentication behavior on a service class.
    /// </summary>
    public interface IAuthenticateAsync
    {
        Task<bool> AuthenticateAsync(WebContext wc);
    }
}