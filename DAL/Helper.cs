using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using static DAL.Contanst;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApp.Exceptions;
using System.Text.RegularExpressions;
using System.Globalization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;

namespace DAL
{
    public class Helper
    {
        public static string FirtDayOfYear()
        {

            return string.Format("01/01/{0}", DateTime.Now.Year);
        }
        public static string FirtDayOfMonth()
        {
            int monthNow = DateTime.Now.Month;
            string month = monthNow.ToString().Length == 1 ? "0" + monthNow : monthNow.ToString();
            return string.Format("01/{0}/{1}", month, DateTime.Now.Year.ToString());
        }
        public static string _ChangeFormatDate(string oldformant)
        {
            if (!string.IsNullOrEmpty(oldformant) && oldformant.IndexOf("/") > 0)
            {
                string[] arr = oldformant.Split('/');
                string newformat = arr[2] + "-" + arr[1] + "-" + arr[0];
                return newformat;
            }
            else
            {
                return "";
            }

        }
        public static int CheckIntNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                //return -1;
                return 0;
            }
            else
            {
                return Convert.ToInt32(readerValue);
            }
        }
        public static decimal CheckDecimalNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(readerValue);
            }
        }
        /// <summary>
        /// Convert phone to VN (bắt đầu = 84)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string ConvertPhoneToFormatVN(string phone)
        {
            Regex regex = new Regex(@"^(0[2|3|5|7|8|9])\d{8}$");
            
            if (regex.IsMatch(phone))
            {
                //chuyển sang đầu 84
                var phoneVN = $"84{phone.Substring(1)}";
                return phoneVN;
            }
            return phone;
        }

        public static string GenMaDon(Int64 value)
        {
            int standerLen = 7;
            int refixLength = value.ToString().Length;
            string temp = "";
            for (int i = refixLength; i < standerLen; i++)
            {
                temp = temp + "0";
            }
            string madon = temp + value.ToString();
            return madon;
        }


        public static string GenPartnerCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Chuyển chuỗi thành dạng chuẩn hóa NFD (Normalization Form Decomposed)
            string normalizedString = input.Normalize(NormalizationForm.FormD);

            // Duyệt qua từng ký tự và chỉ giữ lại các ký tự không phải là dấu hoặc khoảng trắng
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                // Kiểm tra nếu ký tự không phải là dấu (MarkNonSpacing) và không phải khoảng trắng
                if (unicodeCategory != UnicodeCategory.NonSpacingMark && !char.IsWhiteSpace(c))
                {
                    stringBuilder.Append(c);
                }
            }
            Random random = new Random();

            // Tạo số ngẫu nhiên từ 1000 đến 9999
            int randomNumber = random.Next(1000, 10000);

            // Trả chuỗi về dạng bình thường sau khi loại bỏ dấu và khoảng trắng
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC) + randomNumber.ToString();
        }

        public static string GenQRValue(Int64 numValue)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }

        public static string SignDataVIB(string data, string privateKeyPath)
        {
            AsymmetricKeyParameter privateKey = LoadPrivateKey(privateKeyPath);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(dataBytes, 0, dataBytes.Length);

            byte[] signature = signer.GenerateSignature();
            return Convert.ToBase64String(signature);
        }

        public static AsymmetricKeyParameter LoadPrivateKey(string path)
        {
             var reader = File.OpenText(path);
            var pemReader = new PemReader(reader);
            return (AsymmetricKeyParameter)pemReader.ReadObject();
        }

    }
}
