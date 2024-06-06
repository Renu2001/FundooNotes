using ModelLayer;
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
        public static string EncryptPassWord(string encryptpassword)
        {
            var text = System.Text.Encoding.UTF8.GetBytes(encryptpassword);
            return System.Convert.ToBase64String(text);
        }
        public static string DecryptPassWord(string decryptPassword)
        {
            var text1 = System.Convert.FromBase64String(decryptPassword); ;
            return System.Text.Encoding.UTF8.GetString(text1);
        }
    }
}
