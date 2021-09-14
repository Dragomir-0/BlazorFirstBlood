using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Shared
{
    public class BattleResults
    {
        public IList<string> Log { get; set; } = new List<string>();
        public int AttackerDammageSum { get; set; }
        public int OpponentDammageSum { get; set; }
        public bool IsVictory { get; set; }
        public int RoundsFought { get; set; }
    }
}
