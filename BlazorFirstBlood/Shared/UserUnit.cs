using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Shared
{
    public class UserUnit
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public Unit Unit { get; set; }
        public int UnitID { get; set; }
        public int HitPoints { get; set; }
        public string ImageUrl { get; set; }
    }
}
