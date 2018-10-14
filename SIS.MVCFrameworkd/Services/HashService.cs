using SIS.MVCFrameworkd.Logger.Contracts;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SIS.MVCFrameworkd.Services
{
    public class HashService : IHashService
    {
        private ILogger logger;

        public HashService(ILogger logger)
        {
            this.logger = logger;
        }

        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "MyAppPass#1237861273y";
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                this.logger.Log(hash);
                return hash;
            }
        }
    }

    
}
