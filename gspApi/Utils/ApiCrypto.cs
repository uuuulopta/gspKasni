namespace gspAPI.Utils;
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

public class ApiCrypto
{




    const string BULLETIN_KEY_BASE64 = "3+Lhz8XaOli6bHIoYPGuq9Y8SZxEjX6eN7AFPZuLCLs=";
    const string BULLETIN_IV_BASE64 = "IvUScqUudyxBTBU9ZCyjow==";
    public static byte[] KEY_BULLETIN = System.Convert.FromBase64String(BULLETIN_KEY_BASE64);
    public static byte[] IV_BULLETIN = System.Convert.FromBase64String(BULLETIN_IV_BASE64);
    // new Random().NextBytes(iv); // randomize the IV
    //
    // string plainText = ""; //Text to encode
    // byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
    //
    // byte[] cipherBytes = Encrypt(plainBytes, key, iv);
    // string cipherText = Convert.ToBase64String(cipherBytes);
    // Console.WriteLine("Cipher Text: " +cipherText);
    // Console.WriteLine("Key: " + Convert.ToBase64String(key));
    // Console.WriteLine("IV: " + Convert.ToBase64String(iv));
    //
    // byte[] decryptedBytes = Decrypt(cipherBytes, key, iv);
    // string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
    // Console.WriteLine("Decrypted Text: " + decryptedText);
    //
    // Console.ReadLine();


    static string Encrypt(byte[] plainBytes, byte[] key, byte[] iv)
    {
        byte[] encryptedBytes = null;

        // Set up the encryption objects
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            
            // Encrypt the input plaintext using the AES algorithm
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes,
                    0,
                    plainBytes.Length);
            }
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string cipherBase64, byte[] key, byte[] iv)
    {
        byte[] decryptedBytes = null;
        byte[] cipherBytes = Convert.FromBase64String(cipherBase64);
        // Set up the encryption objects
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Decrypt the input ciphertext using the AES algorithm
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                decryptedBytes = decryptor.TransformFinalBlock(cipherBytes,
                    0,
                    cipherBytes.Length);
            }
        }

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public static string getBulletinEncryptedPayload(string station_uid)
    {
        var data = new BulletinRequest(station_uid);
        var jsonStr = JsonConvert.SerializeObject(data);
        return Encrypt(Encoding.UTF8.GetBytes(jsonStr),KEY_BULLETIN,IV_BULLETIN);
    }
    
}

public class BulletinRequest
{
    
    public string station_uid;
    public string session_id = 'A' + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

   public BulletinRequest(string station_uid)
    
    {
        this.station_uid = station_uid;

    }
}

