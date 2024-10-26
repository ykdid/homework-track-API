namespace Homework_track_API.Services.EncryptionService;

public interface IEncryptionService
{
    string Encrypt(string input);
    string Decrypt(string input);
    string Hash(string input);
    bool VerifyHash(string input, string hash);
}