using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Helpers
{
    public static class PasswordHasher
    {

        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000; // Number of iterations

        public static string HashPassword(string password)
        {
            return password;
        }

        public static bool VerifyPassword(string hash, string password)
        {
            return hash.Equals(password);
        }
    }
}
