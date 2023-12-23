using Innovation.Mobile.App.Annotations;
using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Views.Widget.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected readonly IConnectionService _connectionService;
        protected readonly INavigationService _navigationService;
        protected readonly IDialogService _dialogService;
        protected readonly ISettingsService _settingsService;
        protected readonly ILoadPageAndroid _loadingService;

        public ViewModelBase(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService)
        {
            _connectionService = connectionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _settingsService = AppContainer.Resolve<ISettingsService>();
        }

        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public virtual Task InitializeAsync(object data)
        {
            return Task.FromResult(false);
        }
    }
}
