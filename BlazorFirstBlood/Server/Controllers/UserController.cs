using BlazorFirstBlood.Server.Data;
using BlazorFirstBlood.Server.Services;
using BlazorFirstBlood.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUtilityService utitlityService;

        public UserController(DataContext context, IUtilityService utitlityService)
        {
            this.context = context;
            this.utitlityService = utitlityService;
        }

        #region Requests

        [HttpPut("addBananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            //Get and manipulate Entity
            var user = await this.utitlityService.GetUser();
            user.Bananas += bananas;

            //Save changes
            await this.context.SaveChangesAsync();

            //Reply
            return Ok(user.Bananas);
        }

        
        [HttpGet("getBananas")]
        public async Task<IActionResult> GetBananas()
        {
            var user = await this.utitlityService.GetUser();

            return Ok(user.Bananas);
        }


        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await this.context.Users
                .Where(user => !user.IsDeleted && user.IsConfirmed)
                .ToListAsync();

            users = users
                .OrderByDescending(u => u.Victories)
                .ThenBy(u => u.Defeats)
                .ThenBy(u => u.DateCreated)
                .ToList();

            int rank = 1;
            var response = users.Select(user => new UserStatistic
            {
                Rank = rank++,
                UserId = user.Id,
                Username = user.Username,
                Battles = user.Battles,
                Victories = user.Victories,
                Defeats = user.Defeats
            });


            return Ok(response);
        }
        #endregion Requests
    }
}