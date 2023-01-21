using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RectConverter : JsonConverter<Rect>
{
    public override Rect ReadJson(JsonReader reader, Type objectType, Rect existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var result = new Rect();

        result.x = jsonObject[nameof(Rect.x)].ToObject<float>();
        result.y = jsonObject[nameof(Rect.y)].ToObject<float>();
        result.width = jsonObject[nameof(Rect.width)].ToObject<float>();
        result.height = jsonObject[nameof(Rect.height)].ToObject<float>();

        return result;
    }

    public override void WriteJson(JsonWriter writer, Rect value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(value.x));
        writer.WriteValue(value.x);
        writer.WritePropertyName(nameof(value.y));
        writer.WriteValue(value.y);
        writer.WritePropertyName(nameof(value.width));
        writer.WriteValue(value.width);
        writer.WritePropertyName(nameof(value.height));
        writer.WriteValue(value.height);
        writer.WriteEndObject();
    }
}