using BlazorFirstBlood.Server.Data;
using BlazorFirstBlood.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister request)
        {
            var response = await this.authRepo.Register(
                    new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Bananas = request.Bananas,
                        DateOfBirth = request.DateOfBirth,
                        IsConfirmed = request.IsConfirmed
                    }, request.Password
                );

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin request)
        {
            var response = await this.authRepo.Login(request.Email, request.Password);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
