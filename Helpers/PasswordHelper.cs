using BCrypt.Net;
namespace Dashboard.Helpers
{
    public class PasswordHelper
    {
        // Méthode pour hacher le mot de passe
    public static string HashPassword(string password)
        {
            // Bcrypt crée un "sel" unique et hache le mot de passe
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Méthode pour vérifier un mot de passe
        public static bool VerifyPassword(string EnteredPassword, string storedHash)
        {
            // Vérifie si le mot de passe entré correspond au mot de passe haché
            return BCrypt.Net.BCrypt.Verify(EnteredPassword, storedHash);
        }

    }
}
