using be_atoutmajeur.Models.Entities.User;

namespace be_atoutmajeur.Models.Interfaces;

public interface ISecurityServices
{
    string GenerateJwtToken(UserEntity user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}