using ClaimBNB.Dtos;
using ClaimBNB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClaimBNB.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]    
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IAuthService authService;
        private readonly IUserService userService;

        public AuthController(IAuthService authService, IUserService userService, IConfiguration config)
        {
            this.config = config;
            this.authService = authService;
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //validar request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await userService.ExistUserByName(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var createdUsed = await authService.RegisterUser(userForRegisterDto);

            if (createdUsed != null)
                return StatusCode(201);
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            UserForListDto userFromRepo = await authService.Login(userForLoginDto.UserName.ToLower(),
                                                       userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();            

            return Ok(new
            {
                token = authService.GenerateJwtToken(userFromRepo),
                user = userFromRepo
            });
        }        
    }
}
