using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tmbd.Api.Controllers
{
    [Route("[controller]")]
    public class ProbeController : ControllerBase
    {
        [HttpGet, Authorize]
        public string Get()
        {
            return "Ok";
        }
    }
}
