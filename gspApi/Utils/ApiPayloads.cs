namespace gspAPI.Utils;

using Newtonsoft.Json;

public static class ApiPayloads
{
    const string url = "https://online.bgnaplata.rs/publicapi/v2/api.php";
    public static HttpRequestMessage bulletinPayload(string station_uid)
    {
        string encrypted = ApiCrypto.getBulletinEncryptedPayload(station_uid);
        var payload = action_base("data_bulletin",encrypted);
        var mesg = new HttpRequestMessage(HttpMethod.Post, url);
        mesg.Content = new FormUrlEncodedContent(payload);
        return mesg;

    }

    private static Dictionary<string,string> action_base(string action, string Base)
    {
        var parameters = new Dictionary<string, string>();
        parameters.Add("action", action);
        parameters.Add("base",Base);
        return parameters;
    }
}

