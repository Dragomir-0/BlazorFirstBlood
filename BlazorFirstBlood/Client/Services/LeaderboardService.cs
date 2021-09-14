using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient http;

        public LeaderboardService(HttpClient http)
        {
            this.http = http;
        }
        public IList<UserStatistic> Leaderboard { get; set; }
        public async Task GetLeaderBoard()
        {
            Leaderboard = await this.http.GetFromJsonAsync<IList<UserStatistic>>("api/user/leaderboard");
        }
    }
}
