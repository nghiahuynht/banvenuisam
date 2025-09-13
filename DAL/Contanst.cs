using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Contanst
    {
        public static string InvoiceStatus_ConNo = "ConNo";
        public static string InvoiceStatus_DaThanhToan = "DaThanhToan";
        public static string InvoiceStatus_Huy = "Huy";

        public static string InvoiceType_SO = "SO";
        public static string InvoiceType_PO = "PO";

        public static string LoaiIn_VeLe = "Ve_Le";
        public static string LoaiIn_VeDoan = "Ve_Doan";
        #region thanh toán onpay

        public static string PaymentType_TM = "TM";
        public static string PaymentType_CK = "CK";
        public static string PaymentType_OnePay = "OnePay";
        public static decimal PaymentFee_OnePay = 0;
        public static decimal PaymentFee_TrucTiep = 0;

        public static string SECURE_SECRET = "6D0870CDE5F24F34F3915FB0045120DB";

        public enum ResultCode
        {

            Success = 0
        }
        #endregion
        public enum API_Method
        {
            GET = 1,
            POST = 2,
            PUT = 3
        }


        public static string CustForm_NguoiLon = "NguoiLon";
        public static string CustForm_TreEm = "TreEm";


        public static string OrderStatus_Completed = "Completed";
        public static string OrderStatus_Cancel = "Cancel";
    }
}
