using Microsoft.AspNetCore.Identity;

namespace ASP.Net_Meeting_18_Identity.Data
{
    public class User: IdentityUser
    {
        public int YearOfBirth { get; set; }
    }
}
