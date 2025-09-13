using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(string message)
           : base(message)
        {
        }

        public HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
