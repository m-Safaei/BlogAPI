using System.Security.Claims;

namespace BlogAPI.Core.Utilities
{
    public static class UserUtilities
    {
        public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (data != null)
                {
                    return Guid.Parse(data.Value);
                }

                return default;
            }

            return default;
        }
    }
}
