using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels;
using System;
using System.Threading.Tasks;

public interface IDialogService
{
    //Task DialogUpdateApp(StateUpdateApp state, OptionDialogModel option = null);
    Task<SiteAndPrinter> DialogSiteAndPrinter(bool isRefresh = false);
    Task DialogYesOrNo(string TitleMsg, string ContentMsg, Action action);
    Task DialogError(string Msg);
    Task DialogOK(string Title = MessagingConstants.NotifySystem, IconDialog iconDialog = IconDialog.Info, string Content = null, Action action = null, bool IsIcon = true);
}

public enum IconDialog
{
    Success,
    Warning,
    Info,
    Error
}

public enum StateUpdateApp
{
    UpdateVersion,
    GoBackVersion
}
