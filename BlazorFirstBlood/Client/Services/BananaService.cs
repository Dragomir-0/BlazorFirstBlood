using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public class BananaService : IBananaService
    {
        private readonly HttpClient http;

        public BananaService(HttpClient http)
        {
            this.http = http;
        }

        public event Action OnChange;
        public int Bananas { get; set; } = 0;

        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }

        void BananasChanged() => OnChange.Invoke();

        
        public async Task AddBananas(int amount)
        {
            var result = await this.http.PutAsJsonAsync<int>("api/user/addBananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }


       
        public async Task GetBananas()
        {
            Bananas = await this.http.GetFromJsonAsync<int>("api/user/getBananas");
            BananasChanged();
        }
    }
}
