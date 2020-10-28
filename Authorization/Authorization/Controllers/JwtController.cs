using System;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("A_VERY_SECRET_SECURITY_KEY_FOR_JWT_AUTH"));
            var signingCreds = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                { 
                    new Claim(ClaimTypes.Name, "Wynand"),
                    new Claim("Age", "40")
                }),
                SigningCredentials = signingCreds,
                Expires = DateTime.UtcNow.AddDays(7),
            });

            var genToken = tokenHandler.WriteToken(token);
            return Ok(genToken);
        }
    }
}