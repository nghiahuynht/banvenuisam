using Microsoft.IdentityModel.Tokens;
using System;

namespace GM.API.Provider
{
    public class TokenProviderOptions
    {
        public string Path { get; set; }
        public TimeSpan Expiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}