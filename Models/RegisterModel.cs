using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebFinancialHelper.Models
{
    // This is the model that will communicate with the UsersApi
    // These properties will be recieved from the user, mapped into a json file and will be send to the UsersApi
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string RePassword { get; set; }
    }
}
