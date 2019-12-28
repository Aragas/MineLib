using System;
using System.Security.Cryptography;
using System.Text;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Class that will store only password's hash. Hash type is Sha-512.
    /// </summary>
    public class PasswordStorage
    {
        private const string NoPassword = "PUT_PASSWORD_HERE";

        private static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                password = NoPassword;

            using var sha512 = SHA512.Create();
            var hashedPassword = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

            return BitConverter.ToString(hashedPassword).Replace("-", "").ToLowerInvariant();
        }


        /// <summary>
        /// Password's hash. Hash type is Sha-512.
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Temporary storage. Will be set empty after <see cref="Hash"/> is requested. 
        /// You can set the password here instead of creating a new instance.
        /// </summary>
        public string Password { set { HashPassword(value); } }


        /// <summary>
        /// Empty constructor. Set the password in <see cref="Password"/>.
        /// </summary>
        public PasswordStorage() { }
        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="passwordOrHash">The password/the password's Sha-512 hash.</param>
        /// <param name="doHash">Set true is password needs to be hashed.</param>
        public PasswordStorage(string passwordOrHash, bool doHash = true)
        {
            if (doHash)
                Password = passwordOrHash;
            else
                Hash = passwordOrHash;
        }
        
        /// <summary>
        /// Returns <see cref="Hash"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Hash;
    }
}