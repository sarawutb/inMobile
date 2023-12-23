using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface IAutoUpdateVersion
    {
        String VersionName();
        void GetVersion(string version,string package);
        string PackageNameApp();
    }
}
