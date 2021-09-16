using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient http;

        public BattleService(HttpClient http)
        {
            this.http = http;
        }
        public BattleResults LastBattle { get; set; } = new BattleResults();
        public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

        public async Task GetHistory()
        {
            History = await this.http.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
        }

        public async Task<BattleResults> StartBattle(int opponentId)
        {
            var result = await this.http.PostAsJsonAsync("api/battle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResults>();
            return LastBattle;
        }

        
    }
}
