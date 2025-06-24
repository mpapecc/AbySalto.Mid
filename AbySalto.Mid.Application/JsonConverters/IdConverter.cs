using System.Text.Json.Serialization;
using System.Text.Json;
using AbySalto.Mid.Application.Models;

namespace AbySalto.Mid.Application.JsonConverters
{
    public class IdConverter : JsonConverter<Id>
    {
        public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rawId = reader.GetInt32();

            return new Id(rawId);
        }

        public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.EncryptedId);
        }
    }
}
