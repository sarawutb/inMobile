using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Views.Widget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogYesOrNoView : ContentPage
    {
        public event EventHandler<Action> EvenOnOK;
        protected Application CurrentApplication => Application.Current;
        public string Title { get; set; }
        public string Content { get; set; }
        public Action ActionYesOrNo { get; set; }
        public DialogYesOrNoView()
        {
            InitializeComponent();
        }

        public async Task Show(string TitleMsg, string ContentMsg, Action action)
        {
            Title = TitleMsg;
            Content = ContentMsg;
            if (this != App.Current.MainPage.Navigation.ModalStack.LastOrDefault() as DialogYesOrNoView)
            {
                await CurrentApplication.MainPage.Navigation.PushModalAsync(this, false);
            }
            await GetOnEventHandlerAsync(action);
        }

        async void OnButtonYes(object sender, EventArgs args)
        {
            await CurrentApplication.MainPage.Navigation.PopModalAsync(false);
            Device.BeginInvokeOnMainThread(() =>
            {
                ActionYesOrNo.Invoke();
            });
        }

        async void OnButtonNo(object sender, EventArgs args)
        {
            await Close();
        }
        
        protected async Task Close()
        {
            await Navigation.PopModalAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            ActionYesOrNo = null;
            Close();
            return true;
        }

        public async Task<Action> GetOnEventHandlerAsync(Action action)
        {
            if (action != null) ActionYesOrNo = action;
            var tcs = new TaskCompletionSource<Action>();
            EventHandler<Action> handler = null;
            handler = async (sender, result) =>
            {
                EvenOnOK -= handler;
                if (ActionYesOrNo != null)
                {
                    await Device.InvokeOnMainThreadAsync(ActionYesOrNo);
                }
                tcs.SetResult(ActionYesOrNo);
            };
            EvenOnOK += handler;
            return await tcs.Task;
        }

    }
}