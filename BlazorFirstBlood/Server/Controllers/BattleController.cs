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
    public class BattleController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUtilityService utilityService;

        public BattleController(DataContext context, IUtilityService utilityService)
        {
            this.context = context;
            this.utilityService = utilityService;
        }


        #region Requests

        [HttpPost]
        public async Task<IActionResult> StartBattle([FromBody] int opponentId)
        {
            var attacker = await this.utilityService.GetUser();
            var opponent = await this.context.Users.FindAsync(opponentId);
            if (opponent == null || opponent.IsDeleted)
            {
                return NotFound("Opponent not available.");
            }

            var result = new BattleResults();
            await Fight(attacker, opponent, result);

            return Ok();
        }

        #endregion

        #region Helper Methods

        private async Task Fight(User attacker, User opponent, BattleResults result)
        {
            var attackerArmy = await this.context.UserUnits
                .Where(u => u.UserID == attacker.Id && u.HitPoints > 0)
                .Include(u => u.Unit)
                .ToListAsync();

            var opponentArmy = await this.context.UserUnits
                .Where(u => u.UserID == opponent.Id && u.HitPoints > 0)
                .Include(u => u.Unit)
                .ToListAsync();

            var attackerDamageSum = 0;
            var opponentDamageSum = 0;

            int currentRound = 0;

            while (attackerArmy.Count > 0 && opponentArmy.Count > 0)
            {
                currentRound++;

                if (currentRound % 2 != 0)
                {
                    attackerDamageSum +=
                        FightRound(attacker, opponent, attackerArmy, opponentArmy, result);
                }
                else
                {
                    opponentDamageSum +=
                        FightRound(opponent, attacker, attackerArmy, opponentArmy, result);
                }
            }

            result.IsVictory = opponentArmy.Count == 0;
            result.RoundsFought = currentRound;

            if (result.RoundsFought > 0)
                await FinishFight(attacker, opponent, result, attackerDamageSum, opponentDamageSum);
        }

        private int FightRound(User attacker, User opponent, List<UserUnit> attackerArmy,
            List<UserUnit> opponentArmy, BattleResults result)
        {
            int randomAttackerIndex = new Random().Next(attackerArmy.Count);
            int randomOpponentIndex = new Random().Next(opponentArmy.Count);

            var randomAttacker = attackerArmy[randomAttackerIndex];
            var randomOpponent = opponentArmy[randomOpponentIndex];

            var damage =
                new Random().Next(randomAttacker.Unit.Attack)
                    - new Random().Next(randomOpponent.Unit.Defense);

            if (damage < 0) damage = 0;

            if (damage <= randomOpponent.HitPoints)
            {
                randomOpponent.HitPoints -= damage;
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} attacks " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title} with {damage} damage.");
                return damage;
            }
            else
            {
                damage = randomOpponent.HitPoints;
                randomOpponent.HitPoints = 0;
                opponentArmy.Remove(randomOpponent);
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} kills " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title}!");
                return damage;
            }
        }


        private async Task FinishFight(User attacker, User opponent, BattleResults result,
            int attackerDamageSum, int opponentDamageSum)
        {
            result.AttackerDammageSum = attackerDamageSum;
            result.OpponentDammageSum = opponentDamageSum;

            attacker.Battles++;
            opponent.Battles++;

            if (result.IsVictory)
            {
                attacker.Victories++;
                opponent.Defeats++;

                attacker.Bananas += opponentDamageSum;
                opponent.Bananas += attackerDamageSum * 10;
            }
            else
            {
                opponent.Victories++;
                attacker.Defeats++;

                opponent.Bananas += opponentDamageSum;
                attacker.Bananas += attackerDamageSum * 10;
            }

            StoreBattleHistory(attacker, opponent, result);

            await this.context.SaveChangesAsync();
        }

        private void StoreBattleHistory(User attacker, User opponent, 
            BattleResults result)
        {
            var battle = new Battle();

            battle.Attacker = attacker;
            battle.Opponent = opponent;
            battle.RoundsFought = result.RoundsFought;
            battle.WinnerDamage = result.IsVictory
                ? result.AttackerDammageSum
                : result.OpponentDammageSum;
            battle.Winner = result.IsVictory ? attacker : opponent;

            this.context.Battles.Add(battle);
        }

        #endregion
    }
}
