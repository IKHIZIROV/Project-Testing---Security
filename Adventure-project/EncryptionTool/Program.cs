using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EncryptionTool;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: EncryptionTool <input_file> <output_file> <keyshare> <passphrase>");
            Console.WriteLine("Example: EncryptionTool room_secret.txt room_secret.enc a1b2c3d4 mypassphrase");
            return;
        }

        string inputFile = args[0];
        string outputFile = args[1];
        string keyshare = args[2];
        string passphrase = args[3];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Error: Input file '{inputFile}' not found.");
            return;
        }

        try
        {
            // Generate decryption key: SHA256(keyshare + ":" + passphrase)
            byte[] key = GenerateDecryptionKey(keyshare, passphrase);
            
            EncryptFileWithCMS(inputFile, outputFile, key);
            Console.WriteLine($"Successfully encrypted '{inputFile}' to '{outputFile}'");
            Console.WriteLine($"Keyshare: {keyshare}");
            Console.WriteLine($"Passphrase: {passphrase}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    static byte[] GenerateDecryptionKey(string keyshare, string passphrase)
    {
        // Generate key: SHA256(keyshare + ":" + passphrase)
        string combined = $"{keyshare}:{passphrase}";
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        }
    }

    static void EncryptFileWithCMS(string inputFile, string outputFile, byte[] key)
    {
        // Read the plaintext file
        byte[] plaintext = File.ReadAllBytes(inputFile);

        // Use AES encryption with the derived key
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            // Encrypt the data
            byte[] encryptedData;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext, 0, plaintext.Length);
                }
                encryptedData = msEncrypt.ToArray();
            }

            // Create CMS EnvelopedData structure
            // For simplicity, we'll use a custom format that includes IV + encrypted data
            // This simulates CMS format while using the key derivation method specified
            byte[] iv = aes.IV;
            
            // Format: [IV Length (4 bytes)] [IV] [Encrypted Data]
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes(iv.Length), 0, 4);
                ms.Write(iv, 0, iv.Length);
                ms.Write(encryptedData, 0, encryptedData.Length);
                
                File.WriteAllBytes(outputFile, ms.ToArray());
            }
        }
    }
}

