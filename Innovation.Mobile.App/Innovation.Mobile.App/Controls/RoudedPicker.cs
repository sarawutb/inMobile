﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Controls
{
    public class RoudedPicker : Picker
    {
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor),
                typeof(Color), typeof(RoudedPicker), Color.Gray);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int),
                typeof(RoudedPicker), Device.OnPlatform<int>(1, 2, 2));

        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius),
                typeof(double), typeof(RoudedPicker), Device.OnPlatform<double>(6, 7, 7));

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
            BindableProperty.Create(nameof(IsCurvedCornersEnabled),
                typeof(bool), typeof(RoudedPicker), true);

        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }
    }
}
