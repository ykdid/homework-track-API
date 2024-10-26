using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Homework_track_API.Repositories.StudentRepository;
using Homework_track_API.Repositories.TeacherRepository;
using Homework_track_API.Services.EncryptionService;
using Microsoft.IdentityModel.Tokens;

namespace Homework_track_API.Services.AuthService;

public class AuthService(IEncryptionService encryptionService,IConfiguration configuration, IStudentRepository studentRepository, ITeacherRepository teacherRepository):IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly ITeacherRepository _teacherRepository = teacherRepository;

    public async Task<AuthResponse> Register(UserRegisterRequest request)
    {
        if (request.Role == UserRole.Student)
        {
            var existingStudent = await _studentRepository.GetStudentByEmailAsync(request.Email);
            
            if (existingStudent != null)
            {
                return new AuthResponse { IsSuccess = false, ErrorMessage = "User already exists." };
            }

            var student = new Student
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = _encryptionService.Encrypt(request.Email),
                Password = _encryptionService.Hash(request.Password),
                ProfileImagePath = null
            };

            await _studentRepository.CreateStudentAsync(student);

            return new AuthResponse { IsSuccess = true, Id = student.Id };
        }
        else if (request.Role == UserRole.Teacher)
        {
            var existingTeacher = await _teacherRepository.GetTeacherByEmailAsync(request.Email);

            if (existingTeacher != null)
            {
                return new AuthResponse { IsSuccess = false, ErrorMessage = "User already exists." };
            }

            var teacher = new Teacher
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = _encryptionService.Encrypt(request.Email),
                Password = _encryptionService.Hash(request.Password),
                ProfileImagePath = null
            };
            
            return new AuthResponse { IsSuccess = true, Id = teacher.Id };
        }
        
        return new AuthResponse { IsSuccess = false, ErrorMessage = "Invalid role." };
    }

    public async Task<AuthResponse> Login(UserLoginRequest request)
    {
        var encryptedEmail = _encryptionService.Encrypt(request.Email);

        var student = await _studentRepository.GetStudentByEmailAsync(encryptedEmail);
        if (student != null)
        {
            if (_encryptionService.VerifyHash(request.Password, student.Password))
            {
                var token = GenerateJwtToken(student.Id, UserRole.Student);
                return new AuthResponse { IsSuccess = true, Token = token, Id = student.Id, Role = UserRole.Student.ToString() };
            }
            else
            {
                return new AuthResponse { IsSuccess = false, ErrorMessage = "Invalid password." };
            }
        }

        var teacher = await _teacherRepository.GetTeacherByEmailAsync(encryptedEmail);
        if (teacher != null)
        {
            if (_encryptionService.VerifyHash(request.Password, teacher.Password))
            {
                var token = GenerateJwtToken(teacher.Id, UserRole.Teacher);
                return new AuthResponse { IsSuccess = true, Token = token, Id = teacher.Id, Role = UserRole.Teacher.ToString() };
            }
            else
            {
                return new AuthResponse { IsSuccess = false, ErrorMessage = "Invalid password." };
            }
        }

        return new AuthResponse { IsSuccess = false, ErrorMessage = "Invalid email or password." };
    }
    
    private string GenerateJwtToken(int userId, UserRole role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}