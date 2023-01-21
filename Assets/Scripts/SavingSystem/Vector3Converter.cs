using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var result = new Vector3();

        result.x = jsonObject[nameof(Vector3.x)].ToObject<float>();
        result.y = jsonObject[nameof(Vector3.y)].ToObject<float>();
        result.z = jsonObject[nameof(Vector3.z)].ToObject<float>();

        return result;
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(Vector3.x));
        writer.WriteValue(value.x);
        writer.WritePropertyName(nameof(Vector3.y));
        writer.WriteValue(value.y);
        writer.WritePropertyName(nameof(Vector3.z));
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}
