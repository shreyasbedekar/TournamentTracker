using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("teamcompetingid")]
        public int? TeamCompetingId { get; set; }
        [ForeignKey("TeamCompetingId")]
        public TeamModel TeamCompeting { get; set; }
        [Column("score")]
        public double Score { get; set; }
        public int? ParentMatchupId { get; set; }
        [ForeignKey("ParentMatchupId")]
        public MatchupModel ParentMatchup { get; set; }

    }
}
