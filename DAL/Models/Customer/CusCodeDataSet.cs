using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Customer
{
    public class CusCodeDataSet
    {
        /// <summary>
        /// Chiều dài tối đa chuỗi mã hóa
        /// </summary>
        public static readonly int len = 9;
        /// <summary>
        /// bao gồm số và phụ âm, nhỏ nhất là F, lớn nhất là 9
        /// </summary>
        public static readonly Dictionary<int, string> DataSet = new Dictionary<int, string>()
            {
                {0, "F"},
                {1, "B"},
                {2, "C"},
                {3, "D"},
                {4, "G"},
                {5, "H"},
                {6, "K"},
                {7, "L"},
                {8, "M"},
                {9, "N"},
                {10, "P"},
                {11, "Q"},
                {12, "R"},
                {13, "T"},
                {14, "V"},
                {15, "W"},
                {16, "X"},
                {17, "Z"},
                {18, "1"},
                {19, "2"},
                {20, "3"},
                {21, "4"},
                {22, "5"},
                {23, "6"},
                {24, "7"},
                {25, "8"},
                {26, "9"}
            };
    }
}
