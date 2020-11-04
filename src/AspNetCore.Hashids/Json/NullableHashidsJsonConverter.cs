using HashidsNet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCore.Hashids.Json
{
    public class NullableHashidsJsonConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                var hashid = GetHashids(options).Decode(stringValue);

                if (hashid.Length == 0)
                {
                    throw new JsonException("Invalid hash.");
                }

                return hashid[0];
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            throw new JsonException();
        }


        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(GetHashids(options).Encode(value.Value));
            }
        }

        private IHashids GetHashids(JsonSerializerOptions options)
        {
            return options.GetServiceProvider().GetRequiredService<IHashids>();
        }
    }
}
