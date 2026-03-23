using System;
using System.Security.Cryptography;

namespace UniManage.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            salt = Convert.ToBase64String(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }

        public static bool VerifyPassword(string password, string savedSalt, string savedHash)
        {
            byte[] saltBytes = Convert.FromBase64String(savedSalt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                string computed = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return computed == savedHash;
            }
        }
    }
}
