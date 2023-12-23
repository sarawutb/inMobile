using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
