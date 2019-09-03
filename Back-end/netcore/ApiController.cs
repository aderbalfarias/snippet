using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProjectNameApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ILogger _logger;

        public TestController(ITestService testService, ILogger<TestController> logger)
        {
            _testService = testService;
            _logger = logger;
        }

        // GET api/test/getall
        [HttpGet]
        [EnableCors]
        [Route("getall")]
        public IActionResult GetAll() => Ok(_testService.GetAll());

        // GET api/test/getbyid/{id}
        [HttpGet]
        [EnableCors]
        [Route("getbyid/{id:int:min(1)}")]
        public IActionResult GetById(int id) => Ok(_testService.GetById(id));

        // GET api/demo/getdetails/{id}/{modelId}
        [HttpGet]
        [EnableCors]
        [Route("getdetails/{id:int:min(1)}/{modelId:int:min(1)}")]
        public async Task<IActionResult> GetDetails(int id, int modelId) 
            => Ok(await _testService.GetDetails(id, modelId));
    }
}
