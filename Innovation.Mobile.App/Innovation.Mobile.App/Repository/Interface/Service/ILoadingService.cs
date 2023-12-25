using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Repository.Interface.Service
{
    public interface ILoadingService
    {
        Task DialogDownload();
        Task Loading(Action loadingAction, bool IsCloseLoading = false, int TimeoutLoading = 1000);
        Task<T> Loading<T>(Task<T> loadingAction, bool IsCloseLoading = false, int TimeoutLoading = 1000);
    }
}
