using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using WebApi.Model;

namespace WebApi.Controllers
{
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
}
