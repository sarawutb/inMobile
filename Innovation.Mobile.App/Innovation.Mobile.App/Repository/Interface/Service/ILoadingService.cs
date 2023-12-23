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
        Task Loading(Action loadingAction, bool IsCloseLoading, int TimeoutLoading);
        Task<T> Loading<T>(T Result, bool IsCloseLoading = false, int TimeoutLoading = 100);
    }
}
