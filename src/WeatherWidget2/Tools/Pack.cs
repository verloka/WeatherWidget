using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WeatherWidget2.Tools
{
    public static class Pack
    {
        public static T Deserialize<T>(string json)
        {
            T obj = default(T);
            try
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    var ser = new DataContractJsonSerializer(typeof(T), settings);
                    obj = (T)ser.ReadObject(ms);
                }
            }
            catch (Exception e)
            {

            }
            return obj;
        }
        public static string Serialize(object instance)
        {
            string json = string.Empty;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    var ser = new DataContractJsonSerializer(instance.GetType(), settings);
                    ser.WriteObject(ms, instance);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    { json = sr.ReadToEnd(); }
                }
            }
            catch { }

            return json;
        }
    }
}
