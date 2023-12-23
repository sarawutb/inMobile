using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using Xamarin.Essentials;
using Android.Content;
using static Xamarin.Essentials.Permissions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.App;
using Innovation.Mobile.App.Repository.Interface.Helper;

[assembly: Dependency(typeof(PermissionHelper))]
public class PermissionHelper : IPermissionHelper
{
    private const int RequestCode = 1;
    private Context Context = Android.App.Application.Context;

    public bool CheckAndRequestPermission(string permission, int requestCode = RequestCode)
    {
        if (ContextCompat.CheckSelfPermission(Context, permission) == Permission.Granted)
        {
            return true;
        }
        else
        {
            //ActivityCompat.RequestPermissions(Platform.CurrentActivity, new string[] { permission }, requestCode);
            if (ContextCompat.CheckSelfPermission(Context, permission) == Permission.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public async Task<bool> CheckAndRequestPermissionAsync<TPermission>() where TPermission : BasePermission, new()
    {
        try
        {
            var Permissions = new TPermission();
            var CheckStatus = await Permissions.CheckStatusAsync();
            if (CheckStatus != PermissionStatus.Granted)
            {
                return await Permissions.RequestAsync() == PermissionStatus.Granted ? true : false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static void OnRequestPermissionsResult(Activity activity, int requestCode, string[] permissions, Permission[] grantResults)
    {
        if (requestCode == RequestCode)
        {
            if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            {
                Console.WriteLine("Install packages permission granted");
            }
            else
            {
                Console.WriteLine("Install packages permission denied");
            }
        }
    }

}


