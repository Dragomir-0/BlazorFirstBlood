using Blazored.Toast.Services;
using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService toastService;
        private readonly HttpClient http;
        private readonly IBananaService bananaService;

        public UnitService(IToastService toastService, HttpClient http, IBananaService bananaService)
        {
            this.toastService = toastService;
            this.http = http;
            this.bananaService = bananaService;
        }
        public IList<Unit> Units { get; set; } = new List<Unit>();
        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        //public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>
        //{
        //    new UserUnit{UnitID=1, HitPoints=100}
        //};

        // May need to use a service
        public async Task AddUnit(int unitId)
        {
            var unit = Units.First(unit => unit.Id == unitId);
            var result = await this.http.PostAsJsonAsync<int>("api/userunit", unitId);
            if(result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.toastService.ShowError(await result.Content.ReadAsStringAsync());
            }
            else
            {
                await this.bananaService.GetBananas();
                this.toastService.ShowSuccess($"Your {unit.Title} has been built!", "Unit Built!");
            }            
        }

        public async Task LoadUnitsAsync()
        {
            if(Units.Count == 0)
            {
                Units = await this.http.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }

        public async Task LoadUserUnitsAsync()
        {
            MyUnits = await this.http.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
        }

        public async Task ReviveArmy()
        {
            var result = await this.http.PostAsJsonAsync<string>("api/UserUnit/revive", null);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                this.toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
            else
                this.toastService.ShowError(await result.Content.ReadAsStringAsync());

            await LoadUserUnitsAsync();
            await this.bananaService.GetBananas();

        }
    }
}
