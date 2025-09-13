using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Enum
{
    public enum PaymentTypeEnum : int
    {
        TienMat = 0,
        Direct = 1,
        OnePay = 2
    }

    public enum PaymentStatus : int
    {
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        Paid = 1,
        /// <summary>
        /// Chưa thanh toán
        /// </summary>
        UnPaid = 0,
        /// <summary>
        /// Hủy thanh toán
        /// </summary>
        Cancel = 2
    }
}
