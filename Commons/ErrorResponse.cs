using be_atoutmajeur.Models.Interfaces;

namespace be_atoutmajeur.Commons;

public class ErrorResponse : IApiResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ErrorResponse(
        string message, 
        int statusCode = 400, 
        string? details = null)
    {
        Message = message;
        StatusCode = statusCode;
        Details = details;
        StatusCode = statusCode;
        Timestamp = DateTime.UtcNow;
    }
}