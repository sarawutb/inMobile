using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Repository;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Views.Widget.Interface;
using Innovation.Mobile.App.Views.Widget;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Innovation.Mobile.App.Contracts.Service.Genaral;

namespace Innovation.Mobile.App.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private IConnectionService _connectionService;
        private ISettingsService _settingsService;
        public GenericRepository(IConnectionService connectionService, ISettingsService settingsService)
        {
            _connectionService = connectionService;
            _settingsService = settingsService;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(_settingsService.BaseApiUrlBySite) };
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.ConnectionClose = true;
            if (!string.IsNullOrEmpty(_settingsService.TokenSetting))
            {
                httpClient.DefaultRequestHeaders.Add("InhouseAPIKey", _settingsService.TokenSetting);
            }
            //if (!string.IsNullOrEmpty(_settingsService.SiteIdSetting))
            //{
            //    httpClient.DefaultRequestHeaders.Add("SiteID", _settingsService.SiteIdSetting);
            //}

            return httpClient;
        }
        public async Task<T> GetAsync<T>(string URL)
        {
            if (_connectionService.IsConnected)
            {
                try
                {
                    var client = CreateHttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, URL);
                    var response = await client.SendAsync(request);
                    var JsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    client.Dispose();
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        return Response<T>(response.StatusCode, JsonData);
                    }
                    else
                    {
                        throw new WebException(response.ReasonPhrase);
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
            }
        }

        public async Task<TR> PostAsync<T, TR>(string URL, T obj)
        {
            if (_connectionService.IsConnected)
            {
                try
                {
                    var client = CreateHttpClient();
                    var Json = JsonConvert.SerializeObject(obj);

                    var content = new StringContent(Json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post, URL);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    var JsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    client.Dispose();
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        return Response<TR>(response.StatusCode, JsonData);
                    }
                    else
                    {
                        throw new WebException(response.ReasonPhrase);
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
            }
        }

        public async Task<TR> PutAsync<T, TR>(string URL, T obj)
        {
            if (_connectionService.IsConnected)
            {
                try
                {
                    var client = CreateHttpClient();
                    var Json = JsonConvert.SerializeObject(obj);

                    var content = new StringContent(Json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var request = new HttpRequestMessage(HttpMethod.Put, URL);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    var JsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    client.Dispose();
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        return Response<TR>(response.StatusCode, JsonData);
                    }
                    else
                    {
                        throw new WebException(response.ReasonPhrase);
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
            }
        }

        public async Task<bool> DeleteAsync(string URL)
        {
            if (_connectionService.IsConnected)
            {
                try
                {
                    var client = CreateHttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Delete, URL);
                    var response = await client.SendAsync(request);
                    var JsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    client.Dispose();
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        return Response<bool>(response.StatusCode, JsonData);
                    }
                    else
                    {
                        throw new WebException(response.ReasonPhrase);
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
            }
        }

        public async Task<bool> DeleteAsync<T>(string URL, T obj)
        {
            if (_connectionService.IsConnected)
            {
                try
                {
                    var client = CreateHttpClient();
                    var Json = JsonConvert.SerializeObject(obj);

                    var content = new StringContent(Json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var request = new HttpRequestMessage(HttpMethod.Delete, URL);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    var JsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    client.Dispose();
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        return Response<bool>(response.StatusCode, JsonData);
                    }
                    else
                    {
                        throw new WebException(response.ReasonPhrase);
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
            }
        }

        protected T Response<T>(HttpStatusCode statusCode, string json)
        {
            try
            {
                var responseData = JsonConvert.DeserializeObject<ResponseApi<T>>(json);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        return responseData != null ? responseData.Data : default;
                    case HttpStatusCode.NotFound:
                        throw new WebException(responseData.Messenger);
                    case HttpStatusCode.Forbidden:
                        throw new WebException(responseData.Messenger);
                    case HttpStatusCode.Unauthorized:
                        throw new WebException(responseData.Messenger);
                    default:
                        throw new WebException(responseData.Error);
                }
            }
            catch
            {
                var responseData = JsonConvert.DeserializeObject<ResponseApi<object>>(json);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        return responseData != null ? (T)responseData.Data : default;
                    case HttpStatusCode.NotFound:
                        throw new WebException(responseData.Messenger);
                    case HttpStatusCode.Forbidden:
                        throw new WebException(responseData.Messenger);
                    case HttpStatusCode.Unauthorized:
                        throw new WebException(responseData.Messenger);
                    default:
                        throw new WebException(responseData.Error);
                }
            }
        }
    }
}
