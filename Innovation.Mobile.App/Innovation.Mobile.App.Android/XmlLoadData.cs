using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(XmlLoadData))]
namespace Innovation.Mobile.App.Droid
{
    public class XmlLoadData :IXmlLoadData
    {
        //public void LoadData(string data)
        //{
        //    var ma = MainActivity.Instance;
        //    XmlDocument xml = new XmlDocument();
        //    xml.Load(ma.Assets.Open("Programcode.xml"));
        //     data = xml.InnerText;
        //}

        string IXmlLoadData.LoadData()
        {
            var ma = MainActivity.Instance;
            XmlDocument xml = new XmlDocument();
            xml.Load(ma.Assets.Open("Setting.xml"));
            var data = xml.InnerText;
            return data;
        }

    }
}