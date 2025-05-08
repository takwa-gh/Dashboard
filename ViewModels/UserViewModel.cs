using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class UserViewModel
    {
       
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Rôle")]
        public string Role { get; set; }
    }

}
