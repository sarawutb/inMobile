using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.ViewModels;
using Innovation.Mobile.App.Views.Widget;
using Innovation.Mobile.App.Views.Widget.View;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Innovation.Mobile.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /*MainPage = new MainPage();*/
            InitializeWidget();
            InitializeApp();
            InitializeNavigation();
        }

        private void InitializeApp()
        {
            AppContainer.RegisterDependencies();

            //var qualityCheckListDtlViewModel = AppContainer.Resolve<QualityCheckListDtlViewModel>();
            //qualityCheckListDtlViewModel.InitializeMessenger();
        }
        private void InitializeWidget()
        {
            DependencyService.Register<LoadingView>();
            DependencyService.Register<DialogYesOrNoView>();
            DependencyService.Register<DialogError>();
            DependencyService.Register<DialogOk>();
            DependencyService.Register<DialogSiteAndPrinter>();
        }
        private async Task InitializeNavigation()
        {
            var navigationService = AppContainer.Resolve<INavigationService>();
            await navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send(this, MessagingConstants.Closeapp, true);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            ReCheckCache();
        }

        private async Task ReCheckCache()
        {
            var navigationService = AppContainer.Resolve<INavigationService>();
            await navigationService.ReCheckCache();
        }

        public void InitialzeNLog(Assembly assembly)
        {
            var logService = AppContainer.Resolve<ILoggingService>();
            logService.Initialize(assembly);
        }
    }
}
