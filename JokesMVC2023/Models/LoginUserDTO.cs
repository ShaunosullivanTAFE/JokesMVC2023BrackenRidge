using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JokesMVC2023.Models
{
    public class LoginUserDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

    }

    public class RegisterUserDTO : LoginUserDTO
    {

    }
}
