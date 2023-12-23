using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Extensions
{
    public static class SystemExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
