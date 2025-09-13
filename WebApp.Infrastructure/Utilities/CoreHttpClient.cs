using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.IO;

namespace WebApp.Infrastructure.Utilities
{
    public class CoreHttpClient : ICoreHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;
        public CoreHttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string clientName, string uri, object reqObj) where T : class
        {
            var log = new StringBuilder();
            try
            {
                var jsonString = JsonConvert.SerializeObject(reqObj);
                var clientFactory = _clientFactory.CreateClient(clientName);
                //dùng tạm sau đổi lại
                clientFactory.BaseAddress = new Uri($"{clientName}{uri}");
                clientFactory.DefaultRequestHeaders.Accept.Clear();
                clientFactory.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{clientName}{uri}"));
                request.Content = new StringContent(jsonString,
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header
                ///
                log.AppendLine(string.Concat($"Before call api url: {Path.Combine(clientFactory.BaseAddress.AbsoluteUri, uri)}", Environment.NewLine, $"Input: {jsonString}"));

                request.Content.Headers.ContentType.CharSet = null;
                var client = await clientFactory.SendAsync(request);
                log.AppendLine($"RequestUri: {client.RequestMessage.RequestUri.OriginalString}");
                if (client.IsSuccessStatusCode)
                {
                    var result = await client.Content.ReadFromJsonAsync<T>();
                    log.AppendLine($"ResponseCode: {client.StatusCode}");
                    log.AppendLine($"Response: {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                else
                {
                    var result = await client.Content.ReadAsStringAsync();
                    log.AppendLine($"ResponseCode: {client.StatusCode}");
                    log.AppendLine($"Response: {result}");

                    throw new HttpException("error_http_status_code");
                    //return default(T);
                }
            }
            catch (Exception ex)
            {
                log.AppendLine($"Exception: {ex}");
                throw new HttpException(ex.ToString());
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }
        }
        /// <summary>
        /// Gọi api có authorization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string clientName, string uri, Dictionary<string, string> headers, object reqObj)
        {
            var log = new StringBuilder();
            JObject data = null;
            try
            {
                var jsonString = JsonConvert.SerializeObject(reqObj);
                var clientFactory = _clientFactory.CreateClient(clientName);
                clientFactory.BaseAddress = new Uri($"{clientName}{uri}");
                log.AppendLine(string.Concat($"Before call api url: {Path.Combine(clientFactory.BaseAddress.AbsoluteUri, uri)}", Environment.NewLine, $"Input: {jsonString}"));
                using (var client = new System.Net.Http.HttpClient())
                {
                    var baseAddress = clientFactory.BaseAddress.AbsoluteUri;
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{clientName}{uri}"));
                    var inputData = JsonConvert.SerializeObject(reqObj);
                    request.Content = new StringContent(inputData,
                                                        Encoding.UTF8,
                                                        "application/json");//CONTENT-TYPE header
                    request.Content.Headers.ContentType.CharSet = null;
                    //hearder
                    //authoriaztion
                    if (headers != null)
                    {
                        foreach (var hd in headers)
                        {
                            client.DefaultRequestHeaders.Add(hd.Key.Trim(), hd.Value);
                            log.AppendLine($"{hd.Key.Trim()}: {hd.Value}");
                        }
                    }
                    var response = await client.SendAsync(request);
                    log.AppendLine($"RequestUri: {response.RequestMessage.RequestUri.OriginalString}");

                    log.AppendLine($"response: {response}");
                    var statusCode = response.StatusCode.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        var rs = JObject.Parse(content);
                        log.AppendLine($"ResponseCode: {response.StatusCode}");
                        log.AppendLine($"Response: {JsonConvert.SerializeObject(rs)}");
                        return rs != null ? rs.ToObject<T>() : default(T);
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                log.AppendLine($"Exception: {ex}");
                throw new HttpException(ex.ToString());
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }
        }
        /// <summary>
        /// Gọi api có authorization
        /// ContentType application/x-www-form-urlencoded
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        public async Task<T> PostAsyncFormUrlEncoded<T>(string baseAddress,string uri, Dictionary<string, string> headers, IEnumerable<KeyValuePair<string, string>> reqObj)
        {
            var log = new StringBuilder();
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress); // <-- fix ở đây

                    var request = new HttpRequestMessage(HttpMethod.Post, uri)
                    {
                        Content = new FormUrlEncodedContent(reqObj)
                    };

                    // Add headers
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            if (header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                            {
                                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", header.Value.Replace("Basic ", ""));
                            }
                            else
                            {
                                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                            }
                            log.AppendLine($"Header: {header.Key} = {header.Value}");
                        }
                    }

                    log.AppendLine($"Request to: {client.BaseAddress}{uri}");

                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    log.AppendLine($"Status: {response.StatusCode}");
                    log.AppendLine($"Response: {content}");

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(content);
                    }

                    throw new Exception($"API error {response.StatusCode}: {content}");
                }
            }
            catch (Exception ex)
            {
                log.AppendLine($"Exception: {ex}");
                throw;
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }
        }


        /// <summary>
        /// Call api thông qua HTTPClient Delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        public async Task<T> PostAsyncHttp<T>(string clientName, string uri, object reqObj) where T : class
        {
            var log = new StringBuilder();
            try
            {
                var jsonString = JsonConvert.SerializeObject(reqObj);
                var clientFactory = _clientFactory.CreateClient(clientName);
                log.AppendLine(string.Concat($"Before call api url: {Path.Combine(clientFactory.BaseAddress.AbsoluteUri, uri)}", Environment.NewLine, $"Input: {jsonString}"));
                var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var client = await clientFactory.PostAsync(uri, data);
                log.AppendLine($"RequestUri: {client.RequestMessage.RequestUri.OriginalString}");
                if (client.IsSuccessStatusCode)
                {
                    var result = await client.Content.ReadFromJsonAsync<T>();
                    log.AppendLine($"ResponseCode: {client.StatusCode}");
                    log.AppendLine($"Response: {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                else
                {
                    var result = await client.Content.ReadAsStringAsync();
                    log.AppendLine($"ResponseCode: {client.StatusCode}");
                    log.AppendLine($"Response: {result}");

                    throw new HttpException("error_http_status_code");
                }
            }
            catch (Exception ex)
            {
                log.AppendLine($"Exception: {ex}");
                throw new HttpException(ex.ToString());
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }
        }
    }
}
