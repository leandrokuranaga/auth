using Auth.Demo.Models;
using Auth.Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Demo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : Controller
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public NameController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }
        // GET: NameController
        public ActionResult Index()
        {
            return View();
        }

        public string Get(string id)
        {
            return "value";
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "New Jersey", "N" };
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "New Jersey";
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);
            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

    }
}
