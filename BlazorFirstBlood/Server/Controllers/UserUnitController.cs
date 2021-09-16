using BlazorFirstBlood.Server.Data;
using BlazorFirstBlood.Server.Services;
using BlazorFirstBlood.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserUnitController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUtilityService utilityServices;

        public UserUnitController(DataContext context, IUtilityService utilityServices)
        {
            this.context = context;
            this.utilityServices = utilityServices;
        }

        [HttpPost("revive")]
        public async Task<IActionResult> ReviveArmy()
        {
            var user = await utilityServices.GetUser();
            var userUnits = await this.context.UserUnits
                .Where(unit => unit.UserID == user.Id)
                .Include(unit => unit.Unit)
                .ToListAsync();

            int bananaCost = 1000;

            if (user.Bananas < bananaCost)
            {
                return BadRequest($"Not enough bananas. You need {bananaCost} bananas to revive your army.");
            }

            bool armyAlreadyAlive = true;
            foreach (var userUnit in userUnits)
            {
                if (userUnit.HitPoints <= 0) { 
                    armyAlreadyAlive = false;
                    userUnit.HitPoints = new Random().Next(0, userUnit.Unit.HitPoints);
                }
            }

            if (armyAlreadyAlive)
                return Ok("Your army is already alive");

            user.Bananas -= bananaCost;

            await this.context.SaveChangesAsync();

            return Ok("Army revived!");
        }

        [HttpPost]
        public async Task<IActionResult> BuildUserUnit([FromBody] int unitId)
        {
            var unit = await this.context.Units.FirstOrDefaultAsync<Unit>(u => u.Id == unitId);
            var user = await this.utilityServices.GetUser();

            //Replace with service response.
            if (user.Bananas < unit.BananaCost) return BadRequest("Not enough bananas!");

            user.Bananas -= unit.BananaCost;

            var newUserUnit = new UserUnit
            {
                UnitID = unit.Id,
                UserID = user.Id,
                HitPoints = unit.HitPoints
            };

            //re-write return + server response 
            //Re-visit error handler
            this.context.UserUnits.Add(newUserUnit);
            await this.context.SaveChangesAsync();
            return Ok(newUserUnit);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await this.utilityServices.GetUser();
            var userUnits = await this.context.UserUnits
                .Where(unit => unit.UserID == user.Id)
                .Include(unit => unit.Unit)
                .ToListAsync();

            var response = userUnits.Select(
                    unit => new UserUnitResponse
                    {
                        UnitId = unit.UnitID,
                        HitPoints = unit.HitPoints
                    }
                );

            return Ok(response);
        }
    }
}
