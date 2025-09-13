using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApp.Infrastructure;

namespace DAL
{
    /// <summary>
    /// liên quan ZNS
    /// </summary>
    public class ZNSHelper
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string CodeVerifier;
        public static string CodeChallenge;

        public static void Init()
        {
            CodeVerifier = GetCodeVerifier();
            CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        }
        private static string GetCodeVerifier()
        {
            try
            {
                var random = new Random();
                var nonce = new char[43];
                for (int i = 0; i < nonce.Length; i++)
                {
                    nonce[i] = chars[random.Next(chars.Length)];
                }
                return new string(nonce);
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception][GetCodeVerifier]: {ex} ");
                return string.Empty;
            }
        }
        private static string GenerateCodeChallenge(string codeVerifier)
        {
            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }
    }
}
