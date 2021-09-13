using BlazorFirstBlood.Server.Data;
using BlazorFirstBlood.Shared;
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
    public class UnitController : ControllerBase
    {
        private readonly DataContext context;

        //Injection
        public UnitController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var units = await this.context.Units.ToListAsync();
            return Ok(units);
        }

        [HttpPost]
        public async Task<IActionResult> AddUnit(Unit unit)
        {
            this.context.Units.Add(unit);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Units.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            var dbUnit = await this.context.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null) return NotFound("Unit not found");
            dbUnit.Title = unit.Title;
            dbUnit.Attack = unit.Attack;
            dbUnit.Defense = unit.Defense;
            dbUnit.HitPoints = unit.HitPoints;
            dbUnit.BananaCost = unit.BananaCost;

            await this.context.SaveChangesAsync();
            return Ok(dbUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id, Unit unit)
        {
            var dbUnit = await this.context.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null) return NotFound("Unit not found");

            this.context.Units.Remove(dbUnit);
            await this.context.SaveChangesAsync();

            return Ok(await this.context.Units.ToListAsync());

        }
    }
}
