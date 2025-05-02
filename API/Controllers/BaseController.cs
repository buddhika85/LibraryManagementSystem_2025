using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]                                         // gives automatic model bindning, if param is an object you do not need [FromBody] it will automatically look for http request bodys
    public class BaseApiController : ControllerBase            
    {
        // visible only to child classes in the same assembly
        private protected void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
    }
}
