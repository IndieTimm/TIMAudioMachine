using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var result = new Vector2();

        result.x = jsonObject[nameof(Vector2.x)].ToObject<float>();
        result.y = jsonObject[nameof(Vector2.y)].ToObject<float>();

        return result;
    }

    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(Vector2.x));
        writer.WriteValue(value.x);
        writer.WritePropertyName(nameof(Vector2.y));
        writer.WriteValue(value.y);
        writer.WriteEndObject();
    }
}
