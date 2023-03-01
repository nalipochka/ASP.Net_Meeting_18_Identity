using Microsoft.AspNetCore.Identity;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.RolesViewModels
{
    public class ChangeRolesViewModel
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public IList<string> UserRoles { get; set; } = default!;
        public IEnumerable<IdentityRole> AllRoles { get; set; } = default!;
    }
}
