using Innovation.Mobile.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Repository
{
    public interface IGenericRepository
    {
        Task<T> GetAsync<T>(string URL);
        Task<TR> PostAsync<T, TR>(string URL, T obj);
        Task<TR> PutAsync<T, TR>(string URL, T obj);
        Task<bool> DeleteAsync(string URL);
        Task<bool> DeleteAsync<T>(string URL, T obj);

    }
}
