using System;
using System.Security.Cryptography;
using System.Text;

namespace CakesWebApp.Services
{
    public class HashService : IHashService
    {
        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "MyAppPass#1237861273y";
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }

    public interface IHashService
    {
        string Hash(string stringToHash);
    }
}
