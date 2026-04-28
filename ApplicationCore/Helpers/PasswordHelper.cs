using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using Serilog;
using System.Security.Cryptography;

namespace ApplicationCore.Helpers
{
    public class PasswordHelper(ILogger logger) : IPasswordHelper
    {

        public readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;
        private readonly ILogger _logger = logger;

        public string Hash(string password)
        {
            _logger.Information($"Hashing password");
            byte[] salt = RandomNumberGenerator.GetBytes(Constants.saltSize);

            var hashedPassword = HashHelper(password, salt);
            var hashedSalt = Convert.ToBase64String(salt);

            var saltedPassword = string.Concat(hashedPassword, "?", hashedSalt);
            return saltedPassword;
        }

        public bool Verify(string input, string hash)
        {
            _logger.Information($"Verifying password");
            string[] parts = hash.Split("?");
            byte[] salt = Convert.FromBase64String(parts[1]);
            var modifiedPassword = HashHelper(input, salt);
            var isValid = modifiedPassword == parts[0];
            return isValid;
        }

        private string HashHelper(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Constants.iterations, _algorithm, Constants.hashSize);
            var modifiedHashedPassword = Convert.ToBase64String(hash).Replace("?", "#");
            return modifiedHashedPassword;
        }
    }
}
