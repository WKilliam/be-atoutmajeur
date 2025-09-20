using be_atoutmajeur.Commons;
using be_atoutmajeur.Core;
using be_atoutmajeur.Data;
using be_atoutmajeur.Models.DTOs.Auth;
using be_atoutmajeur.Models.Entities.User;
using be_atoutmajeur.Models.Enums.Roles;
using be_atoutmajeur.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace be_atoutmajeur.Services.Auth;

public class AuthServices: IAuthService
{
    private readonly AppDbContext _context;
    private readonly ISecurityServices _securityService;
    private readonly CookieHandler _cookieHandler;

    public AuthServices(AppDbContext context, ISecurityServices securityService, CookieHandler cookieHandler)
    {
        _context = context;
        _securityService = securityService;
        _cookieHandler = cookieHandler;
    }

    public async Task<IApiResponse> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !_securityService.VerifyPassword(request.Password, user.PasswordHash))
                return new ErrorResponse("Incorrect credentials", 401);

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _securityService.GenerateJwtToken(user);
            _cookieHandler.SetAuthToken(token);
            
            var responseData = new AuthResponseDto
            {
                Role = user.Role.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return new SuccessResponse<AuthResponseDto>(responseData, "Connection successful");
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error while connecting", 500, ex.Message);
        }
    }

    public async Task<IApiResponse> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return new ErrorResponse("Email already used", 400);

            var user = new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = _securityService.HashPassword(request.Password),
                Role = Roles.User,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _securityService.GenerateJwtToken(user);
            _cookieHandler.SetAuthToken(token);

            var responseData = new AuthResponseDto
            {
                Role = user.Role.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return new SuccessResponse<AuthResponseDto>(responseData, "Successful registration");
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error while connecting", 500, ex.Message);
        }
    }
}