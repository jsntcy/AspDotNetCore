namespace CityInfo.API.Controllers
{
    using CityInfo.API.Entities;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Route("api/[controller]")]
    public class DummyController : Controller
    {
        private readonly CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{testdatabase}")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}