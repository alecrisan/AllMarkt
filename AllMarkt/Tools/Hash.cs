using System;
using System.Security.Cryptography;
using System.Text;

namespace AllMarkt.Tools
{
    public static class Hash
    {
        public static string ComputeSha256Hash(string rawData)
        {
            if (string.IsNullOrWhiteSpace(rawData)) throw new ArgumentNullException("The row data cannot be empty.");

            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
