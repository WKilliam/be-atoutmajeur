namespace be_atoutmajeur.Core;

public class CookieHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetAuthToken(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("authToken", token, cookieOptions);
    }

    public string? GetAuthToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];
    }

    public void RemoveAuthToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("authToken");
    }

    public bool HasAuthToken()
    {
        return !string.IsNullOrEmpty(GetAuthToken());
    }
}