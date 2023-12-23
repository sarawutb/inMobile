using Akavache;
using Innovation.Mobile.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Innovation.Mobile.App.Service.Data
{
    public class BaseService
    {
        protected IBlobCache Cache;

        public BaseService(IBlobCache cache)
        {
            Cache = cache ?? BlobCache.LocalMachine;
        }

        public T GetFromCache<T>(string cacheName)
        {
            try
            {
                T t = Cache.GetObject<T>(cacheName).First();
                return t;
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }

        public void InvalidateCache()
        {
            Cache.InvalidateAllObjects<ApplicationUser>();
        }
    }
}
