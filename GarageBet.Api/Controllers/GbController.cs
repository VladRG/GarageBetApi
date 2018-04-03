using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class GbController : Controller
    {
        public IActionResult InternalServerError(string error)
        {
            return StatusCode(500, new
            {
                Error = error
            });
        }
    }
}
