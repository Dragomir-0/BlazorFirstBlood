using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public interface ILeaderboardService
    {
        public IList<UserStatistic> Leaderboard { get; set; }
        Task GetLeaderBoard();
    }
}
