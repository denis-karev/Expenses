using System.Security.Cryptography;

namespace Expenses.Api.Services.Encryption;

public sealed class EncryptionService
{
    private const Int32 SaltSize = 32;
    private const Int32 Iterations = 100_000;

    public String Encrypt(String value, Byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);
        
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        sw.Write(value);
        sw.Flush();
        
        return Convert.ToBase64String(ms.ToArray());
    }

    public String Decrypt(String value, Byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        
        using var ms = new MemoryStream(Convert.FromBase64String(value));
        
        Byte[] iv = new Byte[aes.IV.Length];
        ms.ReadExactly(iv, 0, iv.Length);
        aes.IV = iv;
        
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        
        return sr.ReadToEnd();
    }
}