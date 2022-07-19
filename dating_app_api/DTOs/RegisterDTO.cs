using System.ComponentModel.DataAnnotations;

namespace dating_app_api.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
