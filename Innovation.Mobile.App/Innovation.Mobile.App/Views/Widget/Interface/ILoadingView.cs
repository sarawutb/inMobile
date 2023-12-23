

using System.Threading.Tasks;

namespace Innovation.Mobile.App.Views.Widget.Interface
{
    public interface ILoadingView
    {
        Task Hide();
        Task Show(bool IsClose);
    }
}