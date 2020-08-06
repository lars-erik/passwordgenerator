using System;
using Newtonsoft.Json;
using Superpower.Model;

namespace PasswordGenerator.Tests
{
    public class TextSpanConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var span = (TextSpan) value;
            serializer.Serialize(writer, new
            {
                Value = span.ToString(),
                span.IsAtEnd,
                span.Length,
                span.Position,
                span.Source
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TextSpan) == objectType;
        }
    }
}