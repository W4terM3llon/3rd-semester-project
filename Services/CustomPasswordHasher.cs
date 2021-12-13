using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystem.Services
{
    public class CustomPasswordHasher : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            //creating a salt
            var salt = CreateSalt();

            //Storing salt
            user.PasswordSalt = Convert.ToBase64String(salt);

            //hashing
            var hashedPasswordBytes = HashPasswordArgon2(password, salt);

            return Convert.ToBase64String(hashedPasswordBytes);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }

            byte[] storedPasswordSalt = Convert.FromBase64String(user.PasswordSalt);
            string givenPasswordHashed = Convert.ToBase64String(HashPasswordArgon2(providedPassword, storedPasswordSalt));
            if (hashedPassword == givenPasswordHashed)
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        private byte[] CreateSalt()
        {
            var salt = new byte[32];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return salt;
        }

        private byte[] HashPasswordArgon2(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(32);
        }
    }
}
