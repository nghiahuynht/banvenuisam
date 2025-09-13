using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.CORE.Commons
{
    public class Commons
    {
        public string subjectEmail { get; set; }
        public string bodyEmail { get; set; }
        public DateTime createDate { get; set; }
        public DateTime finalDate { get; set; }


        public string createOTP()
        {
            string num = "0123456789";
            int len = num.Length;
            string otp = "";
            int otpDiget = 6;
            string finalDiget;
            int getIndex;
            for (int i = 0; i < otpDiget; i++)
            {
                do
                {
                    getIndex = new Random().Next(0, len);
                    finalDiget = num.ToArray()[getIndex].ToString();
                } while (otp.IndexOf(finalDiget) != -1);
                otp += finalDiget;
            }
            return otp;
        }
    }
}
