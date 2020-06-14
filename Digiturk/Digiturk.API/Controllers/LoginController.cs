using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Digiturk.API.Code;
using Digiturk.Entity.ProjectUser;
using Digiturk.Models.API.Request.Login;
using Digiturk.Models.API.Response.Login;
using Digiturk.Models.Base.Response;
using Digiturk.Repository.ProjectUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<BaseAPIResponse<SignInResponse>>> SignIn([FromBody] SignInRequest request)
        {
            var response = new BaseAPIResponse<SignInResponse>();
            var user = await repo.FirstOrDefaultByAsync(x => x.Email == request.Email && x.Password == request.Password);
            if (user == null)
            {
                response.SetErrorMessage("Hatalı e-posta adresi ya da şifre girdiniz.");
                return Ok(response);
            }
            response.Data = new SignInResponse();
            response.Data.Token = WriteToken(user);
            return Ok(response);
        }

        private string WriteToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("userID", user.Id));
            claims.Add(new Claim("firstName", user.FirstName));
            claims.Add(new Claim("lastName", (!string.IsNullOrWhiteSpace(user.LastName) ? user.LastName : "-")));
           

            var token = new JwtSecurityToken(
                issuer: "https://www.digiturk.com.tr",
                audience: "https://www.digiturk.com.tr",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(20),
                notBefore: DateTime.UtcNow,
                signingCredentials:
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("digiturk2020!!1!1!!1trm")), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
