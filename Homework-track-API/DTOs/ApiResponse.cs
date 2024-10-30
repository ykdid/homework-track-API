namespace Homework_track_API.DTOs;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    
    public ApiResponse(int statusCode, T? data, string? errorMessage)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }
}