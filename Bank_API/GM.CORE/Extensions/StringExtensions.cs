using GM.CORE.Constants;
using GM.CORE.Enums;
using System;
using System.Data;

namespace GM.CORE.Extensions
{
    public static class StringExtensions
    {
        public static double ToMathExpression(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            return Convert.ToDouble(new DataTable().Compute(value, null));
        }

        public static string GetMsg(this ResponseCode @this) => @this switch
        {
            _ => ""
        };

        public static string ToCurrencyVn(this decimal @this) => @this switch
        {
            0M => "",
            _ => @this.ToString(Currency.FormatCurrencyVN) + " VNĐ"
        };
       
        //public static string ToEmptyOrValue(this string @this) => !string.IsNullOrEmpty(@this) ? @this : string.Empty;
    }
}