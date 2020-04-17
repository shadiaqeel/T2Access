using Microsoft.AspNetCore.Mvc;

namespace T2Access.API.Controllers
{
    //[StopWatch]
    // [Localisation]
    [ApiController]
    [Route("api/{lang}/{controller}/{action}")]
    public class ApiBaseController : ControllerBase
    {
    }
}
