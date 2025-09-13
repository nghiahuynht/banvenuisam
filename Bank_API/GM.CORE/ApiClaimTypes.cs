namespace GM.CORE
{
    public class ApiClaimTypes
    {
        public string ClaimType { get; private set; }

        private ApiClaimTypes(string claimType)
        {
            ClaimType = claimType;
        }

        public static ApiClaimTypes UserId => new ApiClaimTypes("UserId");
        public static ApiClaimTypes UserName => new ApiClaimTypes("UserName");
    }
}