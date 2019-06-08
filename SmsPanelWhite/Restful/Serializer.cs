using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SmsPanelSms.Restful
{
	internal static class Serializer
	{
		public static T Deserialize<T>(this string json)
		{
            //if (string.IsNullOrWhiteSpace(json))
		    if (json.IsNullOrWhiteSpace())
            {
				return default(T);
			}
			T t = Activator.CreateInstance<T>();
			MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json));
			t = (T)(new DataContractJsonSerializer(t.GetType())).ReadObject(memoryStream);
			memoryStream.Close();
			return t;
		}

		public static string Serialize<T>(this T obj)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(obj.GetType());
			MemoryStream memoryStream = new MemoryStream();
			dataContractJsonSerializer.WriteObject(memoryStream, obj);
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}
}
public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value == null) return true;
        return string.IsNullOrEmpty(value.Trim());
    }
}