using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GM.CORE.Extensions
{
    //public static class JsonExtensions
    //{
    //    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    //    {
    //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    //    };

    //    public static T FromJson<T>(this string json) =>
    //        JsonSerializer.Deserialize<T>(json, _jsonOptions);

    //    public static string ToJson<T>(this T obj) =>
    //        JsonSerializer.Serialize<T>(obj, _jsonOptions);

    //}

    public class ItemJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(List<T>);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var response = new List<T>();
            JObject items = JObject.Load(reader);

            foreach (var item in items)
            {
                var p = JsonConvert.DeserializeObject<T>(item.Value.ToString());
                response.Add(p);
            }
            return response;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var item in (List<T>)value)
            {
                writer.WriteRawValue(JsonConvert.SerializeObject(item));
            }
            writer.WriteEndArray();
        }
    }
}