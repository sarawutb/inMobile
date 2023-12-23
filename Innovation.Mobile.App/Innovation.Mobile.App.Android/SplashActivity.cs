﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Innovation.Mobile.App.Droid
{
    [Activity(Label = "Innovation.Mobile.App",
       Icon = "@drawable/Innovation_Logo_01",
       Theme = "@style/SplashTheme",
       MainLauncher = true,
       NoHistory = true,
       ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            CallMainActivity();
        }

        private void CallMainActivity()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}