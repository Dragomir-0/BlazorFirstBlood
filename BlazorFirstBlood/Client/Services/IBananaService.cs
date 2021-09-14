using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public interface IBananaService
    {
        event Action OnChange;
        int Bananas { get; set; }
        void EatBananas(int amount);

        /// <summary>
        /// Adds amount of Bananas to the current user
        /// </summary>
        /// <param name="amount">Amount of Bananas to add</param>
        /// <returns></returns>
        Task AddBananas(int amount);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Int Bananas</returns>
        Task GetBananas();
    }
}
