using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Enumerations
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string IconName { get; set; }
    }
    public static class MenuItemModel
    {
        public static MenuItem Home = new MenuItem { Name = "Home", IconName = "home.png" };
        public static MenuItem Receiving = new MenuItem { Name = "Receiving", IconName = "receiving.png" };
        public static MenuItem Picking = new MenuItem { Name = "Picking", IconName = "picking.png" };
        public static MenuItem QualityCheck = new MenuItem { Name = "QualityCheck", IconName = "qualityCheck.png" };
        public static MenuItem LogOut = new MenuItem { Name = "Logout", IconName = "logout.png" };
    }
    public enum MenuItemType
    {
        Home,
        Receiving,
        Picking,
        QualityCheck,
        Logout
    }
}
