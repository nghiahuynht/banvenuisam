﻿namespace GM.DAL.Respository
{
    public static class StoreProcedure
    {
        // User
        public const string LOGIN = "[dbo].[sp_Login]";

        // Payment
        public const string GET_ORDER_INFOR = "[dbo].[sp_GetOrderForQRCode]";
        public const string UPDATE_PAYMENT_ORDER = "[dbo].[sp_UpdatePaymentStatusOrder]";



    }
}

