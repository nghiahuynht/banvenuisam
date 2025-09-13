using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DAL
{
    public class PaymentHelper
    {
        Uri _address;
        SortedList<String, String> _requestFields = new SortedList<String, String>(new VPCStringComparer());
        String _rawResponse;
        SortedList<String, String> _responseFields = new SortedList<String, String>(new VPCStringComparer());
        String _secureSecret;
        /// <summary>
        /// url thanh toán onepay
        /// </summary>
        /// <param name="url"></param>
        public PaymentHelper(string url)
        {
            _address = new Uri(url);
        }
        public void SetSecureSecret(String secret)
        {
            _secureSecret = secret;
        }

        public void AddDigitalOrderField(String key, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestFields.Add(key, value);
            }
        }
        public String GetResultField(String key, String defValue)
        {
            String value;
            if (_responseFields.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return defValue;
            }
        }

        public String GetResultField(String key)
        {
            return GetResultField(key, "");
        }

        private String GetRequestRaw()
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _requestFields)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value) + "&");
                }
            }
            //remove trailing & from string
            if (data.Length > 0)
                data.Remove(data.Length - 1, 1);
            return data.ToString();
        }
        public String Create3PartyQueryString()
        {
            StringBuilder url = new StringBuilder();
            //Payment Server URL
            url.Append(_address);
            url.Append("?");
            //Create URL Encoded request string from request fields 
            url.Append(GetRequestRaw());
            //Hash the request fields
            url.Append("&vpc_SecureHash=");
            url.Append(CreateSHA256Signature(true));
            return url.ToString();
        }
        public string Process3PartyResponse(IQueryCollection queryParams)
        {
            foreach (var item in queryParams)
            {
                if (!item.Key.Equals("vpc_SecureHash") && !item.Key.Equals("vpc_SecureHashType"))
                {
                    _responseFields.Add(item.Key, item.Value);
                }

            }

            if (!queryParams["vpc_TxnResponseCode"].Equals("0") && !String.IsNullOrEmpty(queryParams["vpc_Message"]))
            {
                if (!String.IsNullOrEmpty(queryParams["vpc_SecureHash"]))
                {
                    if (!CreateSHA256Signature(false).Equals(queryParams["vpc_SecureHash"]))
                    {
                        return "INVALIDATED";
                    }
                    return "CORRECTED";
                }
                return "CORRECTED";
            }

            if (String.IsNullOrEmpty(queryParams["vpc_SecureHash"]))
            {
                return "INVALIDATED";//no sercurehash response
            }
            if (!CreateSHA256Signature(false).Equals(queryParams["vpc_SecureHash"]))
            {
                return "INVALIDATED";
            }
            return "CORRECTED";
        }
        /// <summary>
        ///  SHA256 Hash Code
        /// </summary>
        /// <param name="useRequest"></param>
        /// <returns></returns>
        public string CreateSHA256Signature(bool useRequest)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[_secureSecret.Length / 2];
            for (int i = 0; i < _secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            SortedList<String, String> list = (useRequest ? _requestFields : _responseFields);
            foreach (KeyValuePair<string, string> kvp in list)
            {
                if (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_"))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }
        private class VPCStringComparer : IComparer<String>
        {
            /*
             <summary>Customised Compare Class</summary>
             <remarks>
             <para>
             The Virtual Payment Client need to use an Ordinal comparison to Sort on 
             the field names to create the SHA256 Signature for validation of the message. 
             This class provides a Compare method that is used to allow the sorted list 
             to be ordered using an Ordinal comparison.
             </para>
             </remarks>
             */

            public int Compare(String a, String b)
            {
                /*
                 <summary>Compare method using Ordinal comparison</summary>
                 <param name="a">The first string in the comparison.</param>
                 <param name="b">The second string in the comparison.</param>
                 <returns>An int containing the result of the comparison.</returns>
                 */

                // Return if we are comparing the same object or one of the 
                // objects is null, since we don't need to go any further.
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                // Ensure we have string to compare
                string sa = a as string;
                string sb = b as string;

                // Get the CompareInfo object to use for comparing
                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    // Compare using an Ordinal Comparison.
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }
    }

    #region Cache
    /// <summary>
    /// Class Hỗ trợ Caching
    /// MemoryCache sẽ được xóa mỗi khi IIS app pool được làm mới (recycle)
    /// </summary>
    public class MemoryCacheHelper
    {
        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// Add a cache object with date expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absExpiration"></param>
        /// <returns></returns>
        public static bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            return MemoryCache.Default.Add(key, value, absExpiration);
        }

        /// <summary>
        /// Delete cache value from key
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }

        /// <summary>
        /// Delete cache value from key
        /// </summary>
        /// <param name="key"></param>
        public static object DeleteByKey(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                return memoryCache.Remove(key);
            }

            return null;
        }
    }
    #endregion
}