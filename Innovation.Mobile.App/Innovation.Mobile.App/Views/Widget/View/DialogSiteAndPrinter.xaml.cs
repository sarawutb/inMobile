using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Views.Widget.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogSiteAndPrinter : ContentPage
    {
        protected Application CurrentApplication => Application.Current;
        private DialogSiteAndPrinterViewModel _viewModel;
        private ISettingsService _settingsService;
        public List<SiteProfile> LstSiteProfile;
        public DialogSiteAndPrinter()
        {
            InitializePage();
        }
        public void InitializePage()
        {
            InitializeComponent();
            _viewModel = AppContainer.Resolve<DialogSiteAndPrinterViewModel>();
            this.BindingContext = _viewModel;
            _settingsService = AppContainer.Resolve<ISettingsService>();
        }
        public async Task<SiteAndPrinter> Show(bool isRefresh)
        {
            return await _viewModel.Show(this, isRefresh);
        }
        protected override bool OnBackButtonPressed()
        {
            InitializePage();
            CurrentApplication.MainPage.Navigation.PopModalAsync(false);
            return true;
        }
    }
}