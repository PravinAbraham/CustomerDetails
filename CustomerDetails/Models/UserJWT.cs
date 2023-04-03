using System.ComponentModel.DataAnnotations;

namespace CustomerDetails.Models
{
    public class UserJWT
    {
        [EmailAddress]
        public string Email { get; set; }
        public string password { get; set; }
    }
}
