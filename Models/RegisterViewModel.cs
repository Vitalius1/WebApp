using System.ComponentModel.DataAnnotations;
 
namespace WebApp.Models
{
    public class RegisterViewModel : BaseEntity
    {
//------------------------------------------------------------------------------------
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least 2 letters.")]
        [RegularExpression("^([a-zA-Z]+)$", ErrorMessage = "Must be letters only.")]
        public string FirstName {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least 2 letters.")]
        [RegularExpression("^([a-zA-Z]+)$", ErrorMessage = "Must be letters only.")]
        public string LastName {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least 2 characters long.")]
        [MaxLength(20, ErrorMessage = "Must be no more than 20 characters long.")]
        public string Username {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters.")]
        public string Password {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters.")]
        public string ConfirmPassword {get; set;}
//------------------------------------------------------------------------------------
    }
}