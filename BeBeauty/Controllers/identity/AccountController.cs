using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BeBeauty.DTOs.identity;
using BeBeauty.Models.identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BeBeauty.Controllers.identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SignInManager<ApplicationUser> SignInManager { get; }
        public IConfiguration Configuration { get; }

        public AccountController(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
        {
             userManager = _userManager;
            SignInManager = signInManager;
            Configuration = configuration;
        }

        public async Task <string> CreateToken(ApplicationUser userDto)
        {
            var userdata = new List<Claim>();
            userdata.Add(new Claim(ClaimTypes.Email, userDto.Email));

            userdata.Add(new Claim(ClaimTypes.GivenName,userDto.UserName));
            string key = Configuration["jwt:key"];
            
            var skey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(key));
            var signicher = new SigningCredentials(skey, SecurityAlgorithms.HmacSha256);

            var tokenobj = new JwtSecurityToken(
                claims: userdata,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: signicher



                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenobj);
            return  (token);

        }
        [ HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)

        {
            
            if (registerDto == null)
            {
                return BadRequest("Invalid data.");
            }
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                FullName = registerDto.UserName

            };

            var created_user= await userManager.CreateAsync(user, registerDto.Password);
            if (!created_user.Succeeded)
            {
                var errors = created_user.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }
           // var token = CreateToken(created_user);
            var userDto = new UserDto
            {

                UserName = user.UserName,
                Email = user.Email,
               
                token =  await CreateToken(user)
            };
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid data.");
            }
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            var result = await SignInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }
            var userDto = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                token = await CreateToken(user)
            };
            return Ok(userDto);
        }
    }
}
