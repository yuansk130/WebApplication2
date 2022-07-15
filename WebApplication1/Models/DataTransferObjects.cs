using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public string? username { get; set; }
    }
}