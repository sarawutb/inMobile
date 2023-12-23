using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync(Type viewModelType);
        Task ClearBackStack();
        Task NavigateToAsync(Type viewModelType, object parameter);
        Task NavigateBackAsync();
        Task PopModalAsync();
        Task NavigateModalAsync(Page page, bool animated);
        Task RemoveLastFromBackStackAsync();
        Task PopToRootAsync();
        Task ReCheckCache();
    }
}
