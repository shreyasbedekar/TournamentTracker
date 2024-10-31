using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        [Column("winnerid")]
        public int? WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public TeamModel Winner { get; set; }
        [Column("matchupround")]
        public int MatchupRound { get; set; }
    }
}
