using be_atoutmajeur.Models.DTOs.Auth;

namespace be_atoutmajeur.Models.Interfaces;

public interface IAuthService
{
    Task<IApiResponse> LoginAsync(LoginRequestDto request);
    Task<IApiResponse> RegisterAsync(RegisterRequestDto request);
}