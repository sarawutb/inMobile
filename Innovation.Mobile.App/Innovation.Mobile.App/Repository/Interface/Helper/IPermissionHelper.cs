using System.Threading.Tasks;
using static Xamarin.Essentials.Permissions;

namespace Innovation.Mobile.App.Repository.Interface.Helper
{
    public interface IPermissionHelper
    {
        bool CheckAndRequestPermission(string permission, int requestCode);
        Task<bool> CheckAndRequestPermissionAsync<TPermission>() where TPermission : BasePermission, new();
    }
}