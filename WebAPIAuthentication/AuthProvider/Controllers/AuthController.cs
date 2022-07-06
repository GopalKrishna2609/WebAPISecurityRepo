using AuthProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthProvider.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult LoginForAPI1([FromBody] UserAPIModel model)
        {
            //You need to fist make a DB call and check that the user exists in database. If not then return from here and do not generate token.
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.Name))
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Name),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, model.RoleType.ToString()),
                   new Claim(ClaimTypes.DateOfBirth,DateTime.Now.AddYears(-20).ToString()),
                   new Claim("Qualification","BTech"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = CreateToken(authClaims, _configuration["JWT:AudienceWebAPI1"], _configuration["JWT:Secret1"]);

                return Ok(token);
            }
            return BadRequest();
        }
        #region Login For Api 2
        [HttpPost]
        public ActionResult LoginForAPI2([FromBody] UserAPIModel model)
        {
            //You need to fist make a DB call and check that the user exists in database. If not then return from here and do not generate token.
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.Name))
            {

                var authClaims = new List<Claim>
                {
                    new Claim("Name", model.Name),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, model.RoleType.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = CreateToken(authClaims, _configuration["JWT:AudienceWebAPI2"], _configuration["JWT:Secret2"]);

                return Ok(token);
            }
            return BadRequest();
        }
        #endregion

        #region create tocken
        private string CreateToken(List<Claim> authClaims, string audience, string secretKey)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: audience,
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
