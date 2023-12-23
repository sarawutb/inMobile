using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Innovation.Mobile.App.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListMenuItemTemplate : ContentView
	{
		public ListMenuItemTemplate ()
		{
			InitializeComponent ();
			this.BindingContext = AppContainer.Resolve<MenuViewModel>();
		}
	}
}