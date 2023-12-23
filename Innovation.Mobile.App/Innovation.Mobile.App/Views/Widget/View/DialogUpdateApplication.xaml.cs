//using DevExpress.XamarinForms.Popup;
//using Innovation.Mobile.App.Interfase.Service;
//using Innovation.Mobile.App.Models;
//using System;
//using System.Threading.Tasks;

//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

//namespace Innovation.Mobile.App.Views.Widget
//{
//    [XamlCompilation(XamlCompilationOptions.Compile)]
//    public partial class DialogUpdateApplication : ContentPage
//    {
//        public bool StatusDownload { get; set; } = true;

//        private bool _Dismiss;
//        public bool Dismiss
//        {
//            get => _Dismiss;
//            set
//            {
//                _Dismiss = value;
//            }
//        }

//        private double _PercenDownloadStatus;

//        public double PercenDownloadStatus
//        {
//            get
//            {
//                if (_PercenDownloadStatus >= 100)
//                {
//                    animationDowmload.OnFinishedAnimation += AnimationViewOnAnimationFinished;
//                }
//                return _PercenDownloadStatus;
//            }
//            set
//            {
//                try
//                {
//                    _PercenDownloadStatus = value;
//                    if (StatusDownload)
//                    {
//                        PercenDownload.Text = string.Format("กำลังดาวน์โหลด {0} %", value);
//                        animationDowmload.ResumeAnimation();
//                        if (PercenDownloadStatus <= 50)
//                        {
//                            animationDowmload.PauseAnimation();
//                        }

//                    }

//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                    StatusDownload = false;
//                }
//                OnPropertyChanged(nameof(PercenDownload));
//            }
//        }

//        private OptionDialogModel _OptionDialogModel = new OptionDialogModel()
//        {
//            IsOnBack = false,
//            TimeoutColse = null
//        };

//        public OptionDialogModel OptionDialogModel
//        {
//            get => _OptionDialogModel;
//            set
//            {
//                _OptionDialogModel = value;
//                Dismiss = _OptionDialogModel.IsOnBack;
//                if (_OptionDialogModel.TimeoutColse != null)
//                {
//                    Task.Run(async () =>
//                    {
//                        await Task.Delay((int)_OptionDialogModel.TimeoutColse);
//                        await Application.Current.MainPage.Navigation.PopModalAsync(false);
//                    });
//                }
//            }
//        }

//        public DialogUpdateApplication()
//        {
//            InitializeComponent();
//            Popup.Closed += PopupClosed;
//        }

//        //public async Task InitStartDownload(StateUpdateApp state, OptionDialogModel option = null)
//        //{
//        //    OptionDialogModel = option;
//        //    string action = await DisplayActionSheet("Action Sheet", "Cancel", null, "Option 1", "Option 2", "Option 3");
//        //    return;
//        //    await Application.Current.MainPage.Navigation.PushModalAsync(this, false);
//        //    Popup.IsOpen = true;
//        //    if (state == StateUpdateApp.UpdateVersion)
//        //    {
//        //        DependencyService.Get<IApplicationService>().DownloadApk(this);
//        //    }
//        //    else if (state == StateUpdateApp.UpdateVersion)
//        //    {
//        //        DependencyService.Get<IApplicationService>().GoBackVersion();
//        //    }
//        //}

//        private void PopupClosed(object sender, EventArgs e)
//        {
//            Popup.IsVisible = true;
//            Dismiss = true;
//            //var _Popup = (sender as DXPopup);
//        }
//        private void AnimationViewOnAnimationFinished(object sender, EventArgs e)
//        {
//            CloseDialog();
//            DependencyService.Get<IApplicationService>().InstallApk();
//        }

//        private async Task CloseDialog()
//        {
//            await Application.Current.MainPage.Navigation.PopAsync(false);
//        }

//        protected override bool OnBackButtonPressed()
//        {
//            if (OptionDialogModel.IsOnBack)
//            {
//                Device.BeginInvokeOnMainThread(async () =>
//                {
//                    StatusDownload = false;
//                    await CloseDialog();
//                });
//            }

//            if (Popup.IsVisible)
//            {
//                Popup.IsOpen = false;
//                return true;
//            }
//            return base.OnBackButtonPressed();
//        }

//        protected override void OnDisappearing()
//        {
//            animationDowmload?.PauseAnimation();
//            StatusDownload = false;
//            base.OnDisappearing();
//        }
//    }

//}