using Autofac;
using Innovation.Mobile.App.Contracts.Repository;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Data;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels;
using Innovation.Mobile.App.Views.Widget;
using System;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Bootstrap
{
    public class AppContainer
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<MenuViewModel>();
            builder.RegisterType<HomeViewModel>().SingleInstance(); ;
            builder.RegisterType<PickingListViewModel>().SingleInstance();
            builder.RegisterType<ReceivingListViewModel>().SingleInstance();
            builder.RegisterType<InternalReceivingViewModel>();
            builder.RegisterType<InternalReceivingBarcodeDetailViewModel>();
            builder.RegisterType<ExternalReceivingViewModel>();
            builder.RegisterType<ExtReceivingDtlViewModel>();
            builder.RegisterType<ExtReceivingCompDtlViewModel>();
            builder.RegisterType<QualityCheckListViewModel>().SingleInstance();
            builder.RegisterType<QualityCheckListDtlViewModel>();
            builder.RegisterType<QualityCheckListDtlBarcodeViewModel>();
            builder.RegisterType<ExtReceivingAddEditViewModel>();
            builder.RegisterType<PickingListDtlViewModel>();
            builder.RegisterType<PickingFIFOViewModel>().SingleInstance();
            builder.RegisterType<DialogSiteAndPrinterViewModel>();

            //services - data
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
            builder.RegisterType<MaterialAutoReceivingService>().As<IMaterialAutoReceivingService>().SingleInstance();

            //services - general
            builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            //builder.RegisterType<PhoneService>().As<IPhoneService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
            builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();

            //General
            builder.RegisterType<GenericRepository>().As<IGenericRepository>().SingleInstance();
            //builder.RegisterType<GenericRepositoryDemo>().As<IGenericRepositoryDemo>().SingleInstance();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
