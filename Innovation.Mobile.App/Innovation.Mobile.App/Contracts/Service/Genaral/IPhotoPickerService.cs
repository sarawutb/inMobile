using System.IO;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
