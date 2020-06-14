using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digiturk.API.Code;
using Digiturk.Models.API.Request.Login;
using Digiturk.Models.API.Response.Login;
using Digiturk.Repository.ProjectUser;
using Microsoft.AspNetCore.Mvc;

namespace Digiturk.API.Controllers
{
    public class LoginController : BaseController<LoginController>
    {
        private readonly UserRepository repo;

        public LoginController(
            UserRepository repo

            )
        {
            this.repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInRequest request)
        {



            return Ok();

        }
    }
}
