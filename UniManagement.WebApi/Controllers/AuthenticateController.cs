using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniManagement.WebApi.Models;

namespace UniManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(model.Username);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string>? userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //userRoles.ToList().ForEach(r => authClaims.Add(new Claim(ClaimTypes.Role, r)));

                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                JwtSecurityToken token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            IdentityUser userExists = await _userManager.FindByNameAsync(model.Username);
            if(userExists != null)
            {
                return BadRequest(new Response() { Status = "Error", Message = "User already exists!" });
            }

            userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest(new Response() { Status = "Error", Message = "User email already registered!" });
            }

            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult? result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response() { Status = "Error", Message = "Unable to create user!" });
            }

            return Ok(new Response() { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            IdentityUser userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return BadRequest(new Response() { Status = "Error", Message = "Admin already exists!" });
            }

            userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest(new Response() { Status = "Error", Message = "Admin email already registered!" });
            }

            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult? result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response() { Status = "Error", Message = "Unable to create Admin!" });
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if(await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            if(await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new Response() { Status = "Success", Message = "Admin created successfully!" });
        }


        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            byte[] secret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            SymmetricSecurityKey authSignKey = new SymmetricSecurityKey(secret);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
