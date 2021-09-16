using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public interface IBattleService
    {
        BattleResults LastBattle { get; set; }
        IList<BattleHistoryEntry> History { get; set; }
        Task<BattleResults> StartBattle(int opponentId);
        Task GetHistory();
    }
}
