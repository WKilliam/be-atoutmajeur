namespace be_atoutmajeur.Core;

public class CookieHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _environment;

    public CookieHandler(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
    {
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
    }

    public void SetAuthToken(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // ✅ Sécurité : pas accessible en JavaScript
            Secure = !_environment.IsDevelopment(), // ✅ HTTPS seulement en production
            SameSite = _environment.IsDevelopment() 
                ? SameSiteMode.Lax  // ✅ Permissif en développement
                : SameSiteMode.Strict, // ✅ Strict en production
            Expires = DateTime.UtcNow.AddDays(7), // ✅ 7 jours d'expiration
            Path = "/" // ✅ Disponible sur tout le site
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("authToken", token, cookieOptions);
    }

    public string? GetAuthToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];
    }

    public void RemoveAuthToken()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !_environment.IsDevelopment(),
            SameSite = _environment.IsDevelopment() 
                ? SameSiteMode.Lax 
                : SameSiteMode.Strict,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(-1) // ✅ Date passée pour supprimer
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("authToken", "", cookieOptions);
    }

    public bool HasAuthToken()
    {
        return !string.IsNullOrEmpty(GetAuthToken());
    }
}