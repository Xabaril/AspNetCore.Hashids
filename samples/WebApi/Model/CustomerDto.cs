using AspNetCore.Hashids.Json;
using System.Text.Json.Serialization;

namespace WebApi.Model
{
    public class CustomerDto
    {
        [JsonConverter(typeof(HashidsJsonConverter))]
        public int Id { get; set; }        
        [JsonConverter(typeof(NullableHashidsJsonConverter))]
        public int? NullableId { get; set; }
        public int NonHashid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
