using System.Security.Cryptography;
using System.Text;

namespace Adventure_project.Services;

public class EncryptionService
{
    public static byte[] GenerateDecryptionKey(string keyshare, string passphrase)
    {
        // Generate key: SHA256(keyshare + ":" + passphrase)
        string combined = $"{keyshare}:{passphrase}";
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        }
    }

    public static string? DecryptRoomFile(string encryptedFilePath, string keyshare, string passphrase)
    {
        try
        {
            if (!File.Exists(encryptedFilePath))
            {
                return null;
            }

            byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
            byte[] key = GenerateDecryptionKey(keyshare, passphrase);

            // Read IV length and IV
            if (encryptedData.Length < 4)
            {
                return null;
            }

            int ivLength = BitConverter.ToInt32(encryptedData, 0);
            if (encryptedData.Length < 4 + ivLength)
            {
                return null;
            }

            byte[] iv = new byte[ivLength];
            Array.Copy(encryptedData, 4, iv, 0, ivLength);

            byte[] ciphertext = new byte[encryptedData.Length - 4 - ivLength];
            Array.Copy(encryptedData, 4 + ivLength, ciphertext, 0, ciphertext.Length);

            // Decrypt
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch
        {
            // Return null on any decryption error (secure - don't expose error details)
            return null;
        }
    }
}

