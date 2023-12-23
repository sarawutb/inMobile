using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Droid.Dependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Xamarin.Forms;
[assembly: Dependency(typeof(AutoUpdateVersion))]
namespace Innovation.Mobile.App.Droid.Dependencies
{
    public class AutoUpdateVersion :Activity ,IAutoUpdateVersion
    {
        public string VersionName()
        {
           return Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
        }
        public string PackageNameApp()
        {
            return Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).PackageName;
        }

        public void GetVersion(string version , string package)
        {
            Device.OpenUri(new Uri(version));
            Uninstall(package);
        }
        public void Uninstall(string package)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionDelete);
                intent.SetData(Android.Net.Uri.Parse("package:"+ package));
                Forms.Context.StartActivity(intent);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}