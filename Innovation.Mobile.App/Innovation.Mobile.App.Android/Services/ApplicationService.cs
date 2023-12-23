using Xamarin.Forms;
using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using System.Threading.Tasks;
using Innovation.Mobile.App.Views.Widget;
using System;
using System.IO;
using AndroidX.Core.Content;
using Java.Lang;
using static Android.Provider.DocumentsContract;
using Xamarin.Essentials;
using AndroidX.Core.App;
using Android;
using Android.Content.PM;
using static Xamarin.Essentials.Permissions;
using Innovation.Mobile.App.Droid;
using System.Linq;
using Innovation.Mobile.App.Interfase.Service;
using System.Diagnostics;
using Java.Security.Acl;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Helper;

[assembly: Dependency(typeof(ApplicationService))]
public class ApplicationService : IApplicationService
{
    private string ApplicationName = Android.App.Application.Context.ApplicationContext.PackageName;
    private string ApplicationBasePath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "ApplicationFile");
    private string ApplicationBaseFolder = "ApplicationFile";
    //private string ApplicationBasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Android.OS.Environment.DirectoryDownloads);
    private string ApkName = Android.App.Application.Context.ApplicationContext.PackageName + ".apk";
    private string ApkNameOld = "(OldVersion)" + Android.App.Application.Context.ApplicationContext.PackageName + ".apk";
    private long downloadId = 0;
    private string BaseUrlApk = "http://192.168.32.14:5432/test/com.companyname.Innovation.Mobile.App.apk";

    //public async void DownloadApk(DialogUpdateApplication dialogDownload)
    //{
    //    await Task.Run(async () =>
    //    {
    //        await CheckFileApk(ApplicationBasePath, ApkName);
    //    });

    //    Context context = Android.App.Application.Context;
    //    DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(BaseUrlApk));
    //    request.SetTitle("Downloading File");
    //    request.SetDescription("Please wait while the file is downloaded.");
    //    request.SetDestinationInExternalPublicDir(ApplicationBaseFolder, ApkName);

    //    DownloadManager manager = (DownloadManager)context.GetSystemService(Context.DownloadService);
    //    downloadId = manager.Enqueue(request);
    //    new System.Threading.Thread(() =>
    //    {
    //        while (dialogDownload.StatusDownload)
    //        {
    //            DownloadManager.Query q = new DownloadManager.Query();
    //            q.SetFilterById(downloadId);

    //            using (var cursor = manager.InvokeQuery(q))
    //            {
    //                if (cursor.MoveToFirst())
    //                {
    //                    double bytesDownloaded = cursor.GetDouble(cursor.GetColumnIndex(DownloadManager.ColumnBytesDownloadedSoFar));
    //                    double bytesTotal = cursor.GetDouble(cursor.GetColumnIndex(DownloadManager.ColumnTotalSizeBytes));

    //                    // Calculate the download progress percentage
    //                    double progress = System.Math.Round((double)((bytesDownloaded * 100.0) / bytesTotal), 2, MidpointRounding.AwayFromZero);

    //                    // Update the progress bar on the UI thread
    //                    Device.BeginInvokeOnMainThread(() =>
    //                    {
    //                        dialogDownload.PercenDownloadStatus = progress;
    //                        Console.WriteLine("download : " + progress);
    //                    });
                            
    //                    if (progress == 100)
    //                    {
    //                        dialogDownload.PercenDownloadStatus = 100;
    //                        dialogDownload.StatusDownload = false;
    //                    }
    //                }
    //            }
    //        }
    //    }).Start();
  
    //}
    //public VersionAndBuildAndroidModel GetVersionApp()
    //{
    //    var PackageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
    //    return new VersionAndBuildAndroidModel
    //    {
    //        PackageName = PackageInfo.PackageName,
    //        VersionCode = PackageInfo.VersionCode.ToString(),
    //        VersionName = PackageInfo.VersionName,
    //    };
    //}
    public async void InstallApk()
    {
        try
        {
            //var apkFilePath = Path.Combine(Android.OS.Environment.DirectoryDownloads, "Innovation.apk");
            //var intent = new Intent(Intent.ActionView);
            //intent.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(localPath)), "application/vnd.android.package-archive");
            //intent.SetFlags(ActivityFlags.NewTask);
            //Android.App.Application.Context.StartActivity(intent);
            var FullPathApk = System.IO.Path.Combine(ApplicationBasePath, ApkName);
            var file = new Java.IO.File(FullPathApk);


            //var apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(
            //    Android.App.Application.Context,
            //    "com.companyname.Innovation.Mobile.App.fileprovider",
            //    file);

            //Android.Net.Uri apkUri = Xamarin.Essentials.FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", new Java.IO.File(BasePathApk));

            var apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", file);

            //Intent installIntent = new Intent(Intent.ActionInstallPackage);
            //installIntent.SetData(apkUri);
            //installIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            //installIntent.AddFlags(ActivityFlags.NewTask);

            //Android.App.Application.Context.StartActivity(installIntent);

            Intent installIntent = new Intent(Intent.ActionView);
            installIntent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
            installIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            installIntent.AddFlags(ActivityFlags.NewTask);
            Forms.Context.StartActivity(installIntent);

        }
        catch (System.Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error installing APK: {ex.Message}");
        }
    }
    public static void InstallApk(Android.App.Activity activity, string apkFilePath)
    {
        try
        {
            // Create a content URI using FileProvider
            Android.Net.Uri apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(activity, activity.PackageName + ".fileprovider", new Java.IO.File(apkFilePath));
            // Create an Intent to install the APK
            Intent installIntent = new Intent(Intent.ActionInstallPackage);
            installIntent.SetData(apkUri);
            installIntent.AddFlags(ActivityFlags.NewTask);
            installIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            // Start the installation process
            activity.StartActivity(installIntent);
        }
        catch (System.Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error installing APK: {ex.Message}");
        }
    }
    public void UnInstallApk()
    {
        throw new System.NotImplementedException();
    }
    public void GoBackVersion()
    {
        try
        {
            var activity = Android.App.Application.Context;
            var PathOldVersion = System.IO.Path.Combine(ApplicationBasePath, ApkNameOld);
            // Create a content URI using FileProvider
            Android.Net.Uri apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(activity, activity.PackageName + ".fileprovider", new Java.IO.File(PathOldVersion));
            // Create an Intent to install the APK
            Intent installIntent = new Intent(Intent.ActionInstallPackage);
            installIntent.SetData(apkUri);
            installIntent.AddFlags(ActivityFlags.NewTask);
            installIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            // Start the installation process
            activity.StartActivity(installIntent);
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }
    private async Task CheckFileApk(string Path, string ApkName)
    {
        try
        {
            if (await DependencyService.Get<IPermissionHelper>().CheckAndRequestPermissionAsync<StorageRead>())
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                string FullPathApk = System.IO.Path.Combine(Path, ApkName);
                string FullPathApkOld = System.IO.Path.Combine(Path, ApkNameOld);


                if (File.Exists(FullPathApkOld))
                {
                    await Task.Run(() =>
                    {
                        File.Delete(FullPathApkOld);
                    });
                }

                if (File.Exists(FullPathApk))
                {
                    await Task.Run(() =>
                    {
                        File.Move(FullPathApk, FullPathApkOld);
                    });
                    await Task.Run(() =>
                    {
                        File.Delete(FullPathApk);
                    });
                }
            }
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }
    private async Task CheckFileApkAfterDownload()
    {
        // string fileName = System.IO.Path.Combine(System.Environment
        //.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "yourfile.jpg");
        //    var root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        //    var path1 = Android.OS.Environment.DirectoryDownloads;
        //    var path2 = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
        //var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        //if (status != PermissionStatus.Granted)
        //{
        //    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        //}

        //if (status == PermissionStatus.Granted)
        //{
        //    if (File.Exists(FullPathApk))
        //    {
        //        Console.WriteLine("OK");
        //        // File.Delete(BasePathApk);
        //    }
        //}
    }

}