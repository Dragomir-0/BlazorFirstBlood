using BlazorFirstBlood.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Client.Services
{
    public interface IUnitService
    {
        
        public IList<Unit> Units { get; set; }
        public IList<UserUnit> MyUnits { get; set; }
        void AddUnit(int unitId);
        Task LoadUnitsAsync();
    }
}
