using System.ComponentModel.DataAnnotations;

namespace VisualTimeTracking.Common.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
       
        public string Password { get; set; } = string.Empty;
    }
}
