using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Innovation.Mobile.App.Interfase.Service;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service;
using Innovation.Mobile.App.Views.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: Dependency(typeof(InitializeService))]
public class InitializeService : IInitializeService
{
    private Android.Views.View _nativeView;

    private Dialog _dialog;
    public void Start()
    {
       //new LoadingService(InitContentView(new LoadingView()));
       //new DialogService(InitContentView(new DialogView()));
    }
    public Dialog InitContentView(ContentView contentPage)
    {
        if (contentPage != null)
        {
            // build the loading page with native base
            contentPage.Parent = Xamarin.Forms.Application.Current.MainPage;

            contentPage.Layout(new Rectangle(0, 0,
                Xamarin.Forms.Application.Current.MainPage.Width,
                Xamarin.Forms.Application.Current.MainPage.Height));

            var renderer = contentPage.GetOrCreateRenderer();
            _nativeView = renderer.View;

            var Activity = CrossCurrentActivity.Current.Activity;
            _dialog = new Dialog(Activity);
            _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            _dialog.SetCancelable(false);
            _dialog.SetContentView(_nativeView);
            Window window = _dialog.Window;
            window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            window.ClearFlags(WindowManagerFlags.DimBehind);
            window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            return _dialog;
        }
        return null;
    }
}

internal static class PlatformExtension
{
    public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
    {
        var renderer = XFPlatform.GetRenderer(bindable);
        if (renderer == null)
        {
            renderer = XFPlatform.CreateRendererWithContext(bindable, CrossCurrentActivity.Current.Activity);
            XFPlatform.SetRenderer(bindable, renderer);
        }
        return renderer;
    }
}