using DevExpress.XamarinForms.DataForm;
using Lottie.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using Innovation.Mobile.App.Views.Widget.Interface;

namespace Innovation.Mobile.App.Views.Widget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingView : ContentPage, ILoadingView
    {
        public bool IsCloseLoading { get; set; } = false;

        public LoadingView()
        {
            InitializeComponent();
            var animationView = FindByName("animationView") as AnimationView;
            animationView.OnFinishedAnimation += (s, e) =>
            {
                animationView.ResumeAnimation();
            };
            // Set the PopupDismiss property to false to prevent dismissal

        }

        public async Task Show(bool IsClose = false)
        {
            IsCloseLoading = IsClose;
            if (Application.Current.MainPage != null)
            {
                var Loading = Application.Current.MainPage.Navigation.ModalStack.FirstOrDefault(x => x == this);
                if (Loading == null)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(this, false);
                }
            }
        }

        public async Task Hide()
        {
            if (Application.Current.MainPage != null)
            {
                var Loading = Application.Current.MainPage.Navigation.ModalStack.FirstOrDefault(x => x == this);
                if (Loading != null)
                {
                    if (Loading.Navigation.ModalStack.Count > 0)
                        await Loading.Navigation.PopModalAsync(false);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return !IsCloseLoading;
        }
    }
}