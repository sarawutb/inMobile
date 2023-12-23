using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class ApiData<T> : StateData
    {
        public int Site_ID { get; set; }
        public T MyData { get; set; }
    }
}
