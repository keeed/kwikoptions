using KwikOptions.Tools.WebApiTester.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KwikOptions.Tools.WebApiTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        public SampleController(IOptions<SampleOptions> sampleOptions)
        {
            SampleOptions = sampleOptions.Value;
        }

        public SampleOptions SampleOptions { get; private set; }

        [HttpGet]
        public string GetValue()
        {
            return SampleOptions.Value;
        }
    }
}