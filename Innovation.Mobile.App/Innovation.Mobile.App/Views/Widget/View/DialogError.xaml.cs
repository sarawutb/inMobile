using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovation.Mobile.App.Views.Widget.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Views.Widget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogError : ContentPage, IDialogError
    {
        public event EventHandler<Action> EvenOnOK;
        protected Application CurrentApplication => Application.Current;
        public string _Error;
        public string Error
        {
            get => _Error;
            set
            {
                ErrorMsg.Text = value;
            }
        }
        public DialogError()
        {
            InitializeComponent();
        }
        public async Task DialogErrorMsg(string error)
        {
            Error = error;
            if (this != App.Current.MainPage.Navigation.ModalStack.LastOrDefault() as DialogError)
            {
                await CurrentApplication.MainPage.Navigation.PushModalAsync(this, false);
            }
            await GetOnEventHandlerAsync(() => { });
        }
        protected async Task Close()
        {
            await Navigation.PopModalAsync(false);
        }
        async void OnOK(object sender, EventArgs args)
        {
            await Close();
            EvenOnOK.Invoke(this, () => { });
        }
        protected override bool OnBackButtonPressed()
        {
            Close();
            return true;
        }
        public async Task<Action> GetOnEventHandlerAsync(Action action)
        {
            var tcs = new TaskCompletionSource<Action>();
            EventHandler<Action> handler = null;
            handler = async (sender, result) =>
            {
                EvenOnOK -= handler;
                if (action != null)
                {
                    await Device.InvokeOnMainThreadAsync(action);
                }
                tcs.SetResult(action);
            };
            EvenOnOK += handler;
            return await tcs.Task;
        }

    }
}