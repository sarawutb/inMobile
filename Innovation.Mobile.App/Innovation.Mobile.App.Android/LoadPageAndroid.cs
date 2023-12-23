﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Droid;
using Innovation.Mobile.App.Templates;
using Innovation.Mobile.App.Views.Widget;
using Innovation.Mobile.App.Views.Widget.Interface;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: Xamarin.Forms.Dependency(typeof(LoadPageAndroid))]
namespace Innovation.Mobile.App.Droid
{
    public class LoadPageAndroid : ILoadPageAndroid
    {
        private Android.Views.View _nativeView;

        private Dialog _dialog;

        private bool _isInitialized;

        public async Task HideLoadingPage()
        {

            await DependencyService.Get<ILoadingView>().Hide();
            //_isInitialized = false;
            ////_dialog.Hide();
            ////_dialog.Show();
            //_dialog.Dismiss();
        }

        public void InitLoadingPage(ContentPage loadingIndicatorPage)
        {
            if (loadingIndicatorPage != null)
            {
                // build the loading page with native base
                loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

                loadingIndicatorPage.Layout(new Rectangle(0, 0,
                    Xamarin.Forms.Application.Current.MainPage.Width,
                    Xamarin.Forms.Application.Current.MainPage.Height));

                var renderer = loadingIndicatorPage.GetOrCreateRenderer();

                _nativeView = renderer.View;

                _dialog = new Dialog(CrossCurrentActivity.Current.Activity);
                _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                _dialog.SetCancelable(false);
                _dialog.SetContentView(_nativeView);
                Window window = _dialog.Window;
                window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));

                _isInitialized = true;
            }
        }
        private void XamFormsPage_Appearing(object sender, EventArgs e)
        {
            var animation = new Animation(callback: d => ((ContentPage)sender).Content.Rotation = d,
                                          start: ((ContentPage)sender).Content.Rotation,
                                          end: ((ContentPage)sender).Content.Rotation + 360,
                                          easing: Easing.Linear);
            animation.Commit(((ContentPage)sender).Content, "RotationLoopAnimation", 16, 800, null, null, () => true);
        }

        public async Task ShowLoadingPageAsync()
        {
            await DependencyService.Get<ILoadingView>().Show(false);
            //if (!_isInitialized)
            //    InitLoadingPage(new LoadingPage()); // set the default page

            //// showing the native loading page
            //_dialog.Show();
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
}