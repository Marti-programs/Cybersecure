using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cybersecure.Services
{
    public static class SecurityService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public static (string hash, string salt) HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                var salt = Convert.ToBase64String(algorithm.Salt);
                var hash = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var algorithm = new Rfc2898DeriveBytes(enteredPassword, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                return hash == storedHash;
            }
        }

        public static string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[12]; // 12 znaków alfanumerycznych
            for (int i = 0; i < 12; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }


        public static (bool isValid, string message) ValidatePasswordPolicy(string password, string oldPassword, bool policyEnabled, bool requireLevenshtein)
        {
            if (!policyEnabled) return (true, string.Empty);

            if (password.Length < 14)
                return (false, "Hasło musi mieć co najmniej 14 znaków.");
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Hasło musi zawierać przynajmniej jedną wielką literę.");
            if (!Regex.IsMatch(password, @"[a-z]"))
                return (false, "Hasło musi zawierać przynajmniej jedną małą literę.");
            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Hasło musi zawierać przynajmniej jedną cyfrę.");
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?:{}|<>\-_+=]"))
                return (false, "Hasło musi zawierać przynajmniej jeden znak specjalny.");

            if (requireLevenshtein && !string.IsNullOrEmpty(oldPassword))
            {
                int distance = ComputeLevenshteinDistance(password, oldPassword);
                if (distance < 4)
                    return (false, $"Nowe hasło musi różnić się od poprzedniego o co najmniej 4 znaki (obecna odległość: {distance}).");
            }

            return (true, string.Empty);
        }
    }
}
