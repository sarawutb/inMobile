using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface ILoggingService
    {
        void Initialize(Assembly assembly);
        void Error(string message);
    }
}
