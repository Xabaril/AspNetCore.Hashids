using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;

namespace WebApi.Model
{
    public class FruitDto
    {
        [JsonConverter(typeof(LongHashidsJsonConverter))]
        public long Id { get; set; }
        [JsonConverter(typeof(NullableLongHashidsJsonConverter))]
        public long? NullableId { get; set; }
        public long NonHashid { get; set; }
        public string Name { get; set; }
    }
}
