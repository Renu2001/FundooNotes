using BCrypt.Net;
using ModelLayer;
using Org.BouncyCastle.Crypto.Generators;
using RepositoryLayer.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Utility
{
    public class HashingPassword
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (SaltParseException ex)
            {
                throw new CustomizeException("Invalid password format.");
            }
            catch (Exception ex)
            {
                throw new CustomizeException("An error occurred while verifying the password.");
            }
            
        }
    }
}
