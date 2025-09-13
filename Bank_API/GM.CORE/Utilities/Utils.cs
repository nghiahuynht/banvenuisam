using BarcodeLib;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace GM.CORE.Utilities
{
    public static class Utils
    {
        public static int RoundCost(int cost) => (int)(Math.Ceiling(cost / 1000.0m) * 1000);

        public static string GetPathFile()
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            return Path.GetFullPath(Path.Combine(dirPath, @"\Resources\Reports\NinjaVan\NJVReprot.xlsx"));
        }

        public static byte[] GenerateBarcode(string barText)
        {
            if (barText is null)
            {
                throw new ArgumentNullException(nameof(barText));
            }

            var barcode = new Barcode();
           
       
            Image img = barcode.Encode(BarcodeLib.TYPE.CODE128, barText, 500, 200);

            return ImgToBytes(img);
        }

        public static byte[] GenerateQrCode(string qrText)
        {
            if (qrText is null)
            {
                throw new ArgumentNullException(nameof(qrText));
            }

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return BitmapToBytes(qrCodeImage);
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private static byte[] ImgToBytes(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);

                return stream.ToArray();
            }
        }

        public static string ToImgBase64String(byte[] value) => $"data:image/png;base64,{Convert.ToBase64String(value)}";

        public static string ToInConditionQuery(List<string> value) => string.Join(",", value);

        public static string ToInSqlQuery(List<string> listValue)
        {
            return string.Format("'{0}'", string.Join("','", listValue));
        }
        public static string ReplaceLastFour(string s)
        {
            int length = s.Length;
            //Check whether or not the string contains at least four characters; if not, this method is useless
            if (length < 4) return "";
            return s.Substring(0, length - 4) + "****";
        }


        public static string TruncateLongString(string str, int maxLength)
        {
            str = str.Replace("\n", "<br>");
            if (string.IsNullOrEmpty(str)) return str;
            string newstring = str.Substring(0, Math.Min(str.Length, maxLength));
            if(str.Length > maxLength)
            {
                newstring = newstring + "...";
            }
            return newstring;


        }
    }
}