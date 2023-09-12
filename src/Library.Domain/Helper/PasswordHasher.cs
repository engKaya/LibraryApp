using System.Security.Cryptography;

namespace Library.Domain.Helper
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16; //128 / 8, length in bytes
        private const int KeySize = 32; //256 / 8, length in bytes
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char SaltDelimeter = ';';
        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
            return string.Join(SaltDelimeter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }
        public static bool VerifyHashedPassword(string passwordHash, string password)
        {
            var pwdElements = passwordHash.Split(SaltDelimeter);
            var salt = Convert.FromBase64String(pwdElements[0]);
            var hash = Convert.FromBase64String(pwdElements[1]);
            var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}
