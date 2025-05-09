using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public int UserId { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Role { get; set; }

    // Champ optionnel pour changer le mot de passe
    public string? NewPassword { get; set; }
}
