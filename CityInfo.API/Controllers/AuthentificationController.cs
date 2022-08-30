using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentification")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private IConfiguration _configuration;

        public class AuthentificationRequestBody
        {
            [Required]
            public string? UserName { get; set; }
            [Required]
            public string? Password { get; set; }
        }

        public AuthentificationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthentificationRequestBody authentificationRequestBody)
        {
            //Validate credentials
            var user = ValidateCredentials(authentificationRequestBody.UserName, authentificationRequestBody.Password);
            if (user is null)
            {
                return Unauthorized();
            }

            //Create a token
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>()
            {
                new Claim("sub", user.UserId.ToString()),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("city", user.City)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);
            var resultToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(resultToken);
        }

        private CityUserInfo ValidateCredentials(string? userName, string? password)
        {
            //Emulate credentials check
            return new CityUserInfo(5, "Alexey", "Konovalenko", "Odessa");
        }

        public class CityUserInfo
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityUserInfo(int userId, string firstName, string lastName, string city)
            {
                UserId = userId;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

    }
}
