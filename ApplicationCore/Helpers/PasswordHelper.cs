using ApplicationCore.Interfaces;
using System.Security.Cryptography;

namespace ApplicationCore.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {

        private const int _saltSize = 16;
        private const int _hashSize = 32;
        private const int _iterations = 100000;
        private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);

            var hashedPassword = HashHelper(password, salt);
            var hashedSalt = Convert.ToBase64String(salt);

            var saltedPassword = string.Concat(hashedPassword, "?", hashedSalt);
            return saltedPassword;
        }

        public bool Verify(string input, string hash)
        {
            string[] parts = hash.Split("?");
            byte[] salt = Convert.FromBase64String(parts[1]);
            var modifiedPassword = HashHelper(input, salt);
            var isValid = modifiedPassword == parts[0];
            return isValid;
        }

        private string HashHelper(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _algorithm, _hashSize);
            var modifiedHashedPassword = Convert.ToBase64String(hash).Replace("?", "#");
            return modifiedHashedPassword;
        }
    }
}
