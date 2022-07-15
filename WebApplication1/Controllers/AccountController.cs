using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context.ApplicationUser;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Helper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuth _repository;
        private readonly IMapper _mapper;
        private readonly JwtHandlerRepository _jwtHandler;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(IAuth repository,IMapper mapper,JwtHandlerRepository jwtHandlerRepository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _jwtHandler = jwtHandlerRepository;
            _userManager = userManager;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            if (await _repository.CheckUser(user.username))
                return BadRequest(new AuthResponseDto { ErrorMessage = "User already exit!" });

            ApplicationUsers AppUser = _mapper.Map<ApplicationUsers>(user);
            AppUser = await _repository.Register(AppUser, user.password);

            IdentityUser userIdentity = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = AppUser.UserGuid.ToString()
            };
            var result = await _userManager.CreateAsync(userIdentity);


            return Ok();
        }
        [HttpPost("signin")]
        public async Task<IActionResult> signin([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            if(await _repository.Login(userForAuthentication.username, userForAuthentication.Password))
            {
                var userGuild = await _repository.GetGuidFromUsername(userForAuthentication.username);
                var user = await _userManager.FindByNameAsync(userGuild.ToString());
                var signingCredentials = _jwtHandler.GetSigningCredentials();
                var claims = _jwtHandler.GetClaims(user);
                var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token ,username = userForAuthentication.username });
            }
            else{
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication!" });
            }
            return Ok();
        }
        [HttpPost("updateUser")]
        public async Task<IActionResult> updateUser([FromBody] EditUser user)
        {
            if(!String.IsNullOrEmpty(user.password) == true && await _repository.checkPasswordlastFive(user.username, user.password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Can't change your password" });
            else if (await _repository.UpdateUserInfo(user) == null)
                return BadRequest(new AuthResponseDto { ErrorMessage = "User not found!" });

            return Ok(new { Message = "Save Success" });
        }
    }
}
