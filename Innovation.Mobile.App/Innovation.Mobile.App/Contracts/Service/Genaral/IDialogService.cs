using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface IDialogService
    {
        Task ShowDialog(string message, string title, string buttonLabel);

        void ShowToast(string message);
        Task<bool> ShowDialogConfirm(string message, string title, string okText, string cancelText);
    }
}
