using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var result = new Quaternion
        {
            x = jsonObject[nameof(Quaternion.x)].ToObject<float>(),
            y = jsonObject[nameof(Quaternion.y)].ToObject<float>(),
            z = jsonObject[nameof(Quaternion.z)].ToObject<float>(),
            w = jsonObject[nameof(Quaternion.w)].ToObject<float>()
        };

        return result;
    }

    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(Quaternion.x));
        writer.WriteValue(value.x);
        writer.WritePropertyName(nameof(Quaternion.y));
        writer.WriteValue(value.y);
        writer.WritePropertyName(nameof(Quaternion.z));
        writer.WriteValue(value.z);
        writer.WritePropertyName(nameof(Quaternion.w));
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}