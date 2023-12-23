﻿using Innovation.Mobile.App.Constants;
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
	public partial class ExtReceivingCompDtlView : ContentPage
	{
		public ExtReceivingCompDtlView ()
		{
			InitializeComponent ();
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, MessagingConstants.ReExternal, true);
        }
    }
}