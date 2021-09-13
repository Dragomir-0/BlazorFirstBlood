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

        public UnitService(IToastService toastService, HttpClient http)
        {
            this.toastService = toastService;
            this.http = http;
        }
        public IList<Unit> Units { get; set; } = new List<Unit>();
        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        //public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>
        //{
        //    new UserUnit{UnitID=1, HitPoints=100}
        //};

        // May need to use a service
        public void AddUnit(int unitId)
        {
            var unit = Units.First(unit => unit.Id == unitId);
            MyUnits.Add(new UserUnit { UnitID = unit.Id, HitPoints = unit.HitPoints });
            this.toastService.ShowSuccess($"Your {unit.Title} has been built!", "Unit Built!");
        }

        public async Task LoadUnitsAsync()
        {
            if(Units.Count == 0)
            {
                Units = await this.http.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }
    }
}
