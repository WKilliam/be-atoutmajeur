using be_atoutmajeur.Models.Interfaces;

namespace be_atoutmajeur.Commons;

public class SuccessResponse<T> : IApiResponse
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public SuccessResponse(T data, string message = "Opération réussie")
    {
        Data = data;
        Message = message;
    }
}