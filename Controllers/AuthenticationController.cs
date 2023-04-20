using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;

        public AuthenticationController(AppDbContext appDbContext, IConfiguration config) {
        
            _appDbContext = appDbContext;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApplicationUser>> Register (ApplicationUser user)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Password = passwordHash;

            await _appDbContext.ApplicationUsers.AddAsync(user);

            await _appDbContext.SaveChangesAsync();

            return Ok(user);

        }




        [HttpPost("Login")]
        public async Task<ActionResult<ApplicationUser>> Login (ApplicationUserDTO usrDTO)
        {
            var user = _appDbContext.ApplicationUsers.Where(a => a.UserName == usrDTO.UserName).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if(BCrypt.Net.BCrypt.Verify(usrDTO.Password, user.Password) == false)
            {
                return BadRequest("Wrong Password");
            }

            var token = CreateToken(usrDTO);

            return Ok(token);
        }


        


        private string CreateToken(ApplicationUserDTO user)
        {

            List<Claim> claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _config.GetSection("Appsettings:TokenKey").Value!
                ));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claimsList,
                    signingCredentials: cred,
                    expires: DateTime.Now.AddDays(5)
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }














    }
}
