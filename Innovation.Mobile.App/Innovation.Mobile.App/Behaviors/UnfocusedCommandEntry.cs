using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Behaviors
{
    public sealed class UnfocusedCommandEntry
    {
        public static readonly BindableProperty UnfocusedCommandProperty =
            BindableProperty.CreateAttached(
                "UnfocusedCommand",
                typeof(ICommand),
                typeof(UnfocusedCommandEntry),
                default(ICommand),
                BindingMode.OneWay,
                null,
                PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Entry entry)
            {
                entry.Unfocused -= EntryOnUnfocused;
                entry.Unfocused += EntryOnUnfocused;
            }
        }

        private static void EntryOnUnfocused(object sender, FocusEventArgs e)
        {
            var txt = sender as Entry;

            var command = GetUnfocusedCommand(txt);
            if (command != null)
            {
                command.Execute(e);
            }
        }

        public static ICommand GetUnfocusedCommand(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(UnfocusedCommandProperty);
        }
    }
}
