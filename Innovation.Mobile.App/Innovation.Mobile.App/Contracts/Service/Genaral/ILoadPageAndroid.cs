using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface ILoadPageAndroid
    {
        void InitLoadingPage(ContentPage loadingIndicatorPage);

        Task ShowLoadingPageAsync();

        Task HideLoadingPage();
    }
}
