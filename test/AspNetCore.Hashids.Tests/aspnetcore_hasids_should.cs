using AspNetCore.Hashids.Json;
using AspNetCore.Hashids.Mvc;
using AspNetCore.Hashids.Tests.Seedwork;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Hashids.Tests
{
    [Collection(nameof(AspNetCoreServer))]
    public class aspnetcore_hasids_should
    {
        private readonly ServerFixture fixture;

        public aspnetcore_hasids_should(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task hash_the_ids_of_the_dtos()
        {
            var response = await fixture
                .TestServer
                .CreateRequest("api/customers")
                .GetAsync();

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var customers = JsonSerializer.Deserialize<IEnumerable<CustomerDtoHashed>>(json, serializeOptions);
            customers.First().Id.Should().Be("omrA3dl2");
            customers.Last().Id.Should().Be("gj1vzXlz");
        }

        [Fact]
        public async Task allow_to_retrieve_resources_by_hashid_when_exists()
        {
            const string Hashid = "omrA3dl2";
            var response = await fixture
                .TestServer
                .CreateRequest($"api/customers/{Hashid}")
                .GetAsync();

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var customer = JsonSerializer.Deserialize<CustomerDtoHashed>(json, serializeOptions);
            customer.Id.Should().Be(Hashid);
        }

        [Fact]
        public async Task not_fails_when_retrieve_resources_by_hashid_that_does_not_exists()
        {
            const string Hashid = "cmrA3dl2";
            var response = await fixture
                .TestServer
                .CreateRequest($"api/customers/{Hashid}")
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task not_allow_add_or_update_resources_when_hashid_is_not_valid()
        {
            var dto = new CustomerDtoHashed
            {
                Id = "cmrA3dl2",
                FirstName = "Test",
                LastName = "Test"
            };
            var response = await fixture
                .TestServer
                .CreateRequest($"api/customers")
                .WithJsonBody(dto)
                .PostAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task doesn_not_allow_send_integer_instead_hashid_if_configuration_reject_it()
        {
            var dto = new CustomerDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test"
            };

            var response = await fixture
                .TestServer
                .CreateRequest($"api/customers")
                .WithJsonBody(dto)
                .PostAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private static readonly IEnumerable<CustomerDto> customers = new CustomerDto[]
        {
            new CustomerDto
            {
                Id = 5555,
                NonHashid = 10000,
                NullableId = 66666,
                FirstName = "Luis",
                LastName = "Ruiz"
            },
            new CustomerDto
            {
                Id = 6666,
                NonHashid = 20000,
                FirstName = "Unai",
                LastName = "Zorrilla"
            }
        };

        [HttpPost]
        [Route("")]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<IEnumerable<CustomerDto>> Post(CustomerDto dto)
        {
            return Ok();
        }

        [HttpGet]
        [Route("")]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<IEnumerable<CustomerDto>> Get()
        {
            return Ok(customers);
        }

        [HttpGet]
        [Route("{id:hashids}")]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<CustomerDto> Get(
            [FromRoute][ModelBinder(typeof(HashidsModelBinder))] int id)
        {
            return Ok(customers.SingleOrDefault(c => c.Id == id));
        }
    }

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

    public class CustomerDtoHashed
    {
        public string Id { get; set; }
        public string NullableId { get; set; }
        public int NonHashid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
