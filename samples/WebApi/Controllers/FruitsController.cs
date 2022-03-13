using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/fruits")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        private static readonly IEnumerable<FruitDto> fruits = new FruitDto[]
        {
            new FruitDto
            {
                Id = long.MaxValue,
                NonHashid = 10000,
                NullableId = 66666,
                Name = "Apple"
            },
            new FruitDto
            {
                Id = 8888,
                NonHashid = 20000,
                Name = "Banana"
            }
        };

        [HttpGet]
        [Route("")]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<IEnumerable<FruitDto>> Get()
        {
            return Ok(fruits);
        }

        [HttpGet]
        [Route("{id:hashids}")]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<FruitDto> Get(
            [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
        {
            return Ok(fruits.SingleOrDefault(c => c.Id == id));
        }
    }
}
