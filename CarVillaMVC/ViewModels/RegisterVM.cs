using System.ComponentModel.DataAnnotations;

namespace CarVillaMVC.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(65)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [MaxLength(65)]
        [MinLength(3)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(65)]
        [MinLength(2)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
