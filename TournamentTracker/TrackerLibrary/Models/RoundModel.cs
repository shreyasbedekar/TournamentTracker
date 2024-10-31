using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class RoundModel
    {
        public int Id { get; set; }
        public List<MatchupModel> Matchups { get; set; } = new List<MatchupModel>();
        public int RoundNumber { get; set; }
    }
}
