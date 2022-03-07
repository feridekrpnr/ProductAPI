using DitenProductAPI.Interfaces;
using DitenProductAPI.MyEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DitenProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly IJWTAuthenticateManager _jWTAuthenticateManager;

        private readonly Context context;

        public NameController(IJWTAuthenticateManager jWTAuthenticateManager, Context context)
        {
            _jWTAuthenticateManager = jWTAuthenticateManager;
            this.context = context;
        }

        // GET: api/<NameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User User)
        {
            var users = context.Users.ToList();
            foreach (var user in users)
            {
                if (user.UserName == User.UserName && user.Password == User.Password)
                {
                    var token = _jWTAuthenticateManager.createToken(user);
                    user.ApiToken = token;
                    context.SaveChanges();
                    return Ok(token);
                }
            }
            return Unauthorized();
        }

    }
}
