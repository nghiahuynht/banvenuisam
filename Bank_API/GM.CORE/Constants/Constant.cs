using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace GM.CORE.Constants
{

    public static class ApplicationContentType
    {
        public const string EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string PDF = "application/pdf";
        public const string JSON = "application/json; charset=utf-8";
        public const string IMG_PNG = "image/png";
        public const string ZIP = "application/zip";
    }

    public static class DirectoryPath
    {
        public const string NJV_FOLDER = @"Resources\Reports\NJVAN";
        public const string TEMPLATE_FOLDER = @"Resources\Templates";
        public const string ASSETS_FOLDER = @"Resources\Assets";
    }

    public static class Number
    {
        public const string Zero = "0";
    }

    public class Currency
    {
        public static string FormatCurrencyVN =
            string.Format("#,###", CultureInfo.GetCultureInfo("vi-VN").NumberFormat.CurrencyGroupSeparator);
    }
    public static class Common
    {
        public const string WalkingUserCode = "19006663";
    }

}