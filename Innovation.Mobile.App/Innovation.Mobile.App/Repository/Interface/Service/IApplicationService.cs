using Innovation.Mobile.App.Views.Widget;
using static Xamarin.Essentials.Permissions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using Innovation.Mobile.App.Models;

namespace Innovation.Mobile.App.Interfase.Service
{
    public interface IApplicationService
    {
        //void DownloadApk(DialogUpdateApplication dialogDownload);
        void InstallApk();
        void UnInstallApk();
        //VersionAndBuildAndroidModel GetVersionApp();
        void GoBackVersion();
    }
}
