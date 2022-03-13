using AspNetCore.Hashids.Options;
using HashidsNet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCore.Hashids.Json
{
    public class LongHashidsJsonConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                var hashid = GetHashids(options)
                    .DecodeLong(stringValue);

                if (hashid.Length == 0)
                {
                    throw new JsonException("Invalid hash.");
                }

                return hashid[0];
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if ( GetHashIdOptions(options).AcceptNonHashedIds)
                {
                    return reader.GetInt64();
                }

                throw new JsonException(@$"Element is decorated with {nameof(HashidsJsonConverter)} 
but is reading a non hashed id. To allow deserialize numbers set AcceptNonHashedIds to true.");
            }

            throw new JsonException();
        }


        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(
                GetHashids(options).EncodeLong(value));
        }

        private IHashids GetHashids(JsonSerializerOptions options)
        {
            return options.GetServiceProvider().GetRequiredService<IHashids>();
        }

        private HashidsOptions GetHashIdOptions(JsonSerializerOptions options)
        {
            return options.GetServiceProvider().GetRequiredService<HashidsOptions>();
        }
    }
}
