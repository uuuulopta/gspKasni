namespace gspAPI.Utils;

using Models;
using Newtonsoft.Json;

public static class ApiResponseDecryptors
{
    public static string decrpytBulletinResponse(string content)
    {
       return ApiCrypto.Decrypt(content, ApiCrypto.KEY_BULLETIN, ApiCrypto.IV_BULLETIN);
       
    }
}
