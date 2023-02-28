using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP.Net_Meeting_18_Identity.Models.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;
    }
}
