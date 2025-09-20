namespace be_atoutmajeur.Models.Interfaces;

public interface IApiResponse
{
    bool Success { get; set; }
    string Message { get; set; }
    DateTime Timestamp { get; set; }

    int StatusCode  { get; set; }
}