using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.Utilities
{
    public interface ICoreHttpClient
    {
        /// <summary>
        /// call api bình thường
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string clientName, string uri, object reqObj) where T : class;
        /// <summary>
        /// Call api có authen Bearer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string clientName, string uri, Dictionary<string, string> headers, object reqObj);
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
        Task<T> PostAsyncFormUrlEncoded<T>(string clientName, string uri, Dictionary<string, string> headers, IEnumerable<KeyValuePair<string, string>> reqObj);
        /// <summary>
        /// Call api thông qua HTTPClient Delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientName"></param>
        /// <param name="uri"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        Task<T> PostAsyncHttp<T>(string clientName, string uri, object reqObj) where T : class;
    }
}
