using Acr.UserDialogs;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.ViewModels;
using Innovation.Mobile.App.Views.Widget;
using Innovation.Mobile.App.Views.Widget.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Service.Genaral
{
    public class DialogService : IDialogService
    {
        //public async Task DialogUpdateApp(StateUpdateApp state, OptionDialogModel option)
        //{
        //    await new DialogUpdateApplication().InitStartDownload(state, option);
        //}

        public async Task<SiteAndPrinter> DialogSiteAndPrinter(bool isRefresh = false)
        {
           return await DependencyService.Get<DialogSiteAndPrinter>().Show(isRefresh);
        }
        public async Task DialogError(string Msg)
        {
            try
            {
                await DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            }
            catch
            {
                Console.WriteLine("Closed");
            };
            await DependencyService.Get<DialogError>().DialogErrorMsg(Msg);
        }

        public async Task DialogOK(string Title, IconDialog iconDialog, string Content, Action action, bool IsIcon = true)
        {
            try
            {
                await DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            }
            catch
            {
                Console.WriteLine("Closed");
            };
            await DependencyService.Get<DialogOk>().Show(Title, iconDialog, Content, action, IsIcon);
        }

        public async Task DialogYesOrNo(string TitleMsg, string ContentMsg, Action action)
        {
            try
            {
                await DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            }
            catch
            {
                Console.WriteLine("Closed");
            };
            await DependencyService.Get<DialogYesOrNoView>().Show(ContentMsg, TitleMsg, action);
        }
    }
    //class DialogService : IDialogService
    //{
    //    public Task ShowDialog(string message, string title, string buttonLabel)
    //    {
    //       return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
    //    }

    //    public Task<bool> ShowDialogConfirm(string message, string title, string okText, string cancelText)
    //    {
    //        return UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
    //    }

    //    public void ShowToast(string message)
    //    {
    //        UserDialogs.Instance.Toast(message);
    //    }
    // }
}
