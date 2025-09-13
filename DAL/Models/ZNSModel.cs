using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ResZNSModel
    {
        /// <summary>
        /// CodeVerifier
        /// </summary>
        public string CodeVerifier { get; set; }
        /// <summary>
        /// CodeChallenge
        /// </summary>
        public string CodeChallenge { get; set; }
    }
}
