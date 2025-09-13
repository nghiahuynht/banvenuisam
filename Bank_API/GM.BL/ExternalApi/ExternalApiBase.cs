using GM.CORE;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GM.BL.ExternalApi
{
    internal class ExternalApiBase
    {
        private readonly HttpClient _httpClient;

        protected ExternalApiBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T> PostAsync<T>(string url, Dictionary<string, string> urlParams = null, dynamic dataBody = null)
        {
            MapParamsUrl(ref url, urlParams);

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (dataBody != null)
            {
                var jsonDataBody = JsonConvert.SerializeObject(dataBody);
                request.Content = new StringContent(jsonDataBody, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var baseData = data.ToModelFromJsonCamel<T>();

                return baseData;
            }

            return default(T);
        }

        protected async Task<T> GetAsync<T>(string url, Dictionary<string, string> urlParams = null)
        {
            MapParamsUrl(ref url, urlParams);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var baseData = data.ToModelFromJsonCamel<T>();//JsonConvert.DeserializeObject<T>(data); ;

                return baseData;
            }
            return default(T);
        }
        protected async Task<byte[]> GetFileAsync<T>(string uri,string url,string token, Dictionary<string, string> urlParams = null)
        {
            MapParamsUrl(ref url, urlParams);

            var request = new HttpRequestMessage(HttpMethod.Get,url);
            request.Headers.Add("token", token);
            if(uri != null)
            {
                _httpClient.BaseAddress = new Uri(uri);
            }
            
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Byte[] byteArray = response.Content.ReadAsByteArrayAsync().Result;

                
                return byteArray;
            }
            return null;
        }

        protected async Task<T> PostAsyncToken<T>(string uri,string url,string token, dynamic dataBody = null)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (dataBody != null)
            {
                var jsonDataBody = JsonConvert.SerializeObject(dataBody);
                request.Content = new StringContent(jsonDataBody, Encoding.UTF8, "application/json");
            }

            if (uri != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uri);
                // add token
                httpClient.DefaultRequestHeaders.Add("token", token);

                try
                {
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        var baseData = data.ToModelFromJsonCamel<T>();

                        return baseData;
                    }
                    httpClient.Dispose();
                    return default(T);

                }
                catch (HttpRequestException ex)
                {

                    httpClient.Dispose();
                    throw;
                }
              


            }

            else
            {
                // add token
                _httpClient.DefaultRequestHeaders.Add("token", token);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var baseData = data.ToModelFromJsonCamel<T>();

                    return baseData;

                }
                return default(T);
            }

          
           
          



        }


        private string MapParamsUrl(ref string fullUrl, Dictionary<string, string> urlParams)
        {
            if (urlParams != null && urlParams.Count > 0)
            {
                fullUrl += "?";
                //fullUrl += "/14"; //+ urlParams.Values;
                foreach (var par in urlParams)
                {
                    if (!string.IsNullOrEmpty(par.Key))
                    {
                        fullUrl += $"{par.Key}={par.Value}&";
                    }
                }
                fullUrl = fullUrl.Remove(fullUrl.Length - 1, 1);
            }
            return fullUrl;
        }

        protected async Task<T> GetAsyncToken<T>(string uri,string url, string token, Dictionary<string, string> urlParams = null)
        {
            MapParamsUrl(ref url, urlParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            T baseData = default(T);    
            if (uri != null)
            {
                // In case we need to change uri which is defferent with base uri => define new httpClient

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uri);
                // add token
                httpClient.DefaultRequestHeaders.Add("token", token);
                baseData = await SendAsync<T>(httpClient, request);          
            }
            else
            {
                // In case we use base uri => use base httpClient

                //add header token
                _httpClient.DefaultRequestHeaders.Add("token", token);
                var response = await _httpClient.SendAsync(request);
                baseData = await SendAsync<T>(_httpClient, request);
            }      
            return baseData;
        }

        private async Task<T> SendAsync<T>(HttpClient httpClient, HttpRequestMessage request )
        {
            try
            {
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var baseData = data.ToModelFromJsonCamel<T>();

                    return baseData;
                }
                httpClient.Dispose();
                return default(T);

            }
            catch (HttpRequestException ex)
            {

                httpClient.Dispose();
                throw;
            }
        }
    }
}