using System.ComponentModel.DataAnnotations;

namespace WebFinancialHelper.Models
{
    // This is the model that will communicate with the UsersApi
    // These properties will be recieved from the user, mapped into a json file and will be send to the UsersApi
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
