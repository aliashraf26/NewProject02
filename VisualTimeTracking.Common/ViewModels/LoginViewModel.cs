using System.ComponentModel.DataAnnotations;

namespace VisualTimeTracking.Common.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = "";
       
        public string Password { get; set; } = "";
    }
}
