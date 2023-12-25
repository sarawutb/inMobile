using Xamarin.Forms;
using Android.App;
using Innovation.Mobile.App.Views.Widget;
using Android.Service.Controls;
using System.Threading;
using System.Threading.Tasks;
using Innovation.Mobile.App.Interfase.Service;
using Innovation.Mobile.App.Service;
using Java.Lang;
using System;
using Innovation.Mobile.App.Repository.Interface.Service;
using static Java.Util.ResourceBundle;
using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.Constants;

[assembly: Dependency(typeof(LoadingService))]
namespace Innovation.Mobile.App.Service
{
    public class LoadingService : ILoadingService
    {
        public Task DialogDownload()
        {
            throw new NotImplementedException();
        }

        public async Task Loading(Action loadingAction, bool IsCloseLoading, int TimeoutLoading)
        {
            try
            {
                await DependencyService.Get<LoadingView>().Show(IsCloseLoading);
                loadingAction.Invoke();
                await Task.Delay(TimeoutLoading);
                await DependencyService.Get<LoadingView>().Hide();
            }
            catch (System.Exception ex)
            {
                await Task.Delay(TimeoutLoading);
                await DependencyService.Get<LoadingView>().Hide();
                await AppContainer.Resolve<IDialogService>().DialogError(ex.Message);
            }
        }

        //public async Task<T> Loading<T>(Task<T> Result, bool IsCloseLoading, int TimeoutLoading)
        public async Task<T> Loading<T>(Task<T> loadingAction, bool IsCloseLoading, int TimeoutLoading)
        {
            var Result = default(T);
            try
            {
                await DependencyService.Get<LoadingView>().Show(IsCloseLoading);
                //await Task.Run(() =>
                //{
                //    Func<T> loadingAction = delegate ()
                //    {
                //        return Result;
                //    };
                //    Result = loadingAction.Invoke();
                //});
                Result =  loadingAction.Result;
                await Task.Delay(TimeoutLoading);
                await DependencyService.Get<LoadingView>().Hide();
               
                return Result;
            }
            catch (System.Exception ex)
            {
                await Task.Delay(TimeoutLoading);
                await DependencyService.Get<LoadingView>().Hide();
                await AppContainer.Resolve<IDialogService>().DialogError(ex.Message);
                return Result;
            }
        }
    }
}
