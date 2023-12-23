using Innovation.Mobile.App.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InternalReceivingView : ContentPage
	{
		public InternalReceivingView ()
		{
			InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, MessagingConstants.ReExternal, true);
            return false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, MessagingConstants.ReExternal, true);
        } 
    }
}