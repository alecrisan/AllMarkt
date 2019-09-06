using System.Collections.Generic;
using System.Security.Claims;

namespace AllMarkt.Tools
{
    public interface IClaimsGetter
    {
        int UserId(IEnumerable<Claim> claims);

        string UserRole(IEnumerable<Claim> claims);

        string DisplayName(IEnumerable<Claim> claims);

        string Email(IEnumerable<Claim> claims);
    }
}
