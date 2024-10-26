using Homework_track_API.DTOs;

namespace Homework_track_API.Services.AuthService;

public interface IAuthService
{
    Task<AuthResponse> Register(UserRegisterRequest request);
    Task<AuthResponse> Login(UserLoginRequest request);
}