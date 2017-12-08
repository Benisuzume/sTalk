using Newtonsoft.Json;

namespace sTalk.Server.Communication.Packets
{
    public static class Incoming
    {
        public static T Parse<T>(string buffer)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(buffer);
            }
            catch
            {
                return default(T);
            }
        }
    }
}