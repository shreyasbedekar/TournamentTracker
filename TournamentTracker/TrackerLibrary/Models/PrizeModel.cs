using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("placenumber")]
        public int PlaceNumber { get; set; }
        [Column("placename")]
        public string PlaceName { get; set; }
        [Column("prizeamount")]
        public decimal PrizeAmount { get; set; }
        [Column("prizepercentage")]
        public double PrizePercentage { get; set; }
    }
}
