using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Models
{
    public class MainMenuItem : BindableObject
    {
        private string _menuText;
        private string _menuIcon;
        private MenuItemType _menuItemType;
        private Type _viewModelToLoad;

        public MenuItemType MenuItemType
        {
            get
            {
                return _menuItemType;
            }
            set
            {
                _menuItemType = value;
                OnPropertyChanged();
            }
        }

        public string MenuText
        {
            get
            {
                return _menuText;
            }
            set
            {
                _menuText = value;
                OnPropertyChanged();
            }
        }
       
        public string MenuIcon
        {
            get
            {
                return _menuIcon;
            }
            set
            {
                _menuIcon = value;
                OnPropertyChanged();
            }
        }
        
        public Type ViewModelToLoad
        {
            get
            {
                return _viewModelToLoad;
            }
            set
            {
                _viewModelToLoad = value;
                OnPropertyChanged();
            }
        }
    }
}
