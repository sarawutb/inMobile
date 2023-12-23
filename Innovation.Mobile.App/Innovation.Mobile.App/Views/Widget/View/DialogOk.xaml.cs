using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels;
using Innovation.Mobile.App.Views.Widget.Interface;
using Lottie.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Views.Widget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogOk : ContentPage
    {
        public event EventHandler<Action> EvenOnOK;
        protected Application CurrentApplication => Application.Current;
        private void SetIconDialog(IconDialog iconDialog)
        {
            switch (iconDialog)
            {
                case IconDialog.Success:
                    DialogIcon = "Success.json";
                    break;
                case IconDialog.Warning:
                    DialogIcon = "Warning.json";
                    break;
                case IconDialog.Info:
                    DialogIcon = "Info.json";
                    break;
                case IconDialog.Error:
                    DialogIcon = "Error.json";
                    break;
            }
        }
        public Action Action { get; set; } = null;
        private string _DialogIcon;
        public string DialogIcon
        {
            get => _DialogIcon;
            set
            {
                _DialogIcon = value;
                animationView.Animation = _DialogIcon;
            }
        }
        public string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                TitleMsg.Text = value;
            }
        }
        public string _Content;
        public string Content
        {
            get => _Content;
            set
            {
                ContentMsg.Text = value;
            }
        }
        public bool _IsIcon;
        public bool IsIcon
        {
            get => _IsIcon;
            set
            {
                _IsIcon = value;
            }
        }
        public DialogOk()
        {
            InitializeComponent();
        }
        public async Task Show(string title, IconDialog iconDialog, string content, Action action, bool isIcon)
        {
            Title = title;
            Content = content;
            SetIconDialog(iconDialog);
            if (this != App.Current.MainPage.Navigation.ModalStack.LastOrDefault() as DialogOk)
            {
                await CurrentApplication.MainPage.Navigation.PushModalAsync(this, false);
            }
            await GetOnEventHandlerAsync(action);
        }
        protected async Task Close()
        {
            await Navigation.PopModalAsync(false);
        }
        async void OnOK(object sender, EventArgs args)
        {
            await Close();
            EvenOnOK.Invoke(this, Action);
        }
        protected override bool OnBackButtonPressed()
        {
            Title = string.Empty;
            Content = string.Empty;
            Close();
            return true;
        }
        public async Task<Action> GetOnEventHandlerAsync(Action action)
        {
            if (action != null) Action = action;
            var tcs = new TaskCompletionSource<Action>();
            EventHandler<Action> handler = null;
            handler = async (sender, result) =>
            {
                EvenOnOK -= handler;
                if (Action != null)
                {
                    await Device.InvokeOnMainThreadAsync(Action);
                }
                tcs.SetResult(Action);
            };
            EvenOnOK += handler;
            return await tcs.Task;
        }
    }
}