using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views;
using Innovation.Mobile.App.Views.Widget.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Service.Genaral
{
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly Dictionary<Type, Type> _mappings;
        protected Application CurrentApplication => Application.Current;

        public NavigationService(IAuthenticationService authenticationService, ISettingsService settingsService,
            IDialogService dialogService)
        {
            _authenticationService = authenticationService;
            _settingsService = settingsService;
            _dialogService = dialogService;
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {

            if (_authenticationService.IsUserAuthenticated())
            {
                await NavigateToAsync<MainViewModel>();
            }
            else
            {
                await NavigateToAsync<LoginViewModel>();
            }
        }

        public async Task ClearBackStack()
        {
            if (CurrentApplication.MainPage != null)
                await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public async Task PopModalAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopModalAsync(false);
        }
        public async Task PopToRootAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                await mainPage.Detail.Navigation.PopToRootAsync();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);

            if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (page is LoginView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;

                if (mainPage.Detail is InnoNavigationPage navigationPage)
                {
                    var currentPage = navigationPage.CurrentPage;

                    if (currentPage.GetType() != page.GetType())
                    {
                        await navigationPage.PushAsync(page);
                    }
                }
                else
                {
                    navigationPage = new InnoNavigationPage(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as InnoNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new InnoNavigationPage(page);
                    await new InnoNavigationPage(CurrentApplication.MainPage).PushAsync(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(MenuViewModel), typeof(MenuView));
            _mappings.Add(typeof(HomeViewModel), typeof(HomeView));
            _mappings.Add(typeof(ReceivingListViewModel), typeof(ReceivingListView));
            _mappings.Add(typeof(PickingListViewModel), typeof(PickingListView));
            _mappings.Add(typeof(PickingListDtlViewModel), typeof(PickingListDtlView));
            _mappings.Add(typeof(InternalReceivingViewModel), typeof(InternalReceivingView));
            _mappings.Add(typeof(ExternalReceivingViewModel), typeof(ExternalReceivingView));
            _mappings.Add(typeof(ExtReceivingAddEditViewModel), typeof(ExtReceivingAddEditView));
            _mappings.Add(typeof(InternalReceivingBarcodeDetailViewModel), typeof(InternalReceivingBarcodeDetailView));
            _mappings.Add(typeof(ExtReceivingDtlViewModel), typeof(ExtReceivingDtlView));
            _mappings.Add(typeof(ExtReceivingCompDtlViewModel), typeof(ExtReceivingCompDtlView));
            _mappings.Add(typeof(QualityCheckListViewModel), typeof(QualityCheckListView));
            _mappings.Add(typeof(QualityCheckListDtlViewModel), typeof(QualityCheckListDtlView));
            _mappings.Add(typeof(QualityCheckListDtlBarcodeViewModel), typeof(QualityCheckListDtlBarcodeView));
            _mappings.Add(typeof(PickingFIFOViewModel), typeof(PickingFIFOView));
        }

        public async Task ReCheckCache()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                if (_settingsService.CurrentFormSetting != typeof(LoginViewModel).Name) // "LoginViewModel"
                {
                    await _dialogService.DialogOK("แจ้งเตือนระบบ", IconDialog.Warning, "เนื่องจากไม่ได้ทำรายการในเวลาที่กำหนด กรุณา Login อีกครั้ง \nไม่สามารถดำเนินการต่อได้",
                                    async () =>
                                    {
                                        await NavigateToAsync<LoginViewModel>();
                                    });
                }
            }
        }

        public async Task NavigateModalAsync(Page page, bool animated)
        {
            await CurrentApplication.MainPage.Navigation.PushModalAsync(page, animated);
        }
    }
}
