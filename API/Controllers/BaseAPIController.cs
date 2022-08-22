using API.Helpers;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivitiy))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAPIController:ControllerBase
    {
        
    }
}