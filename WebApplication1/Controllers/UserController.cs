using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context.ApplicationUser;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IAuth _repository;
        private readonly IMapper _mapper;
        public UserController(IAuth repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("getUserInfo")]
        public async Task<IActionResult> getUserinformation(string username)
        {
            ApplicationUsers user = _repository.GetUserInfo(username).Result;
            if (_repository.GetUserInfo(username) != null)
            {
                UserRegister AppUser = _mapper.Map<UserRegister>(user);
                return Ok(AppUser);
            }
            else
            {
                return BadRequest(new AuthResponseDto { ErrorMessage = "User not found!" });
            }
            return Ok();
        }
    }
}
