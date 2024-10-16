using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("emailaddress")]
        public string EmailAddress { get; set; }
        [Column("cellphonenumber")]
        public string CellphoneNumber { get; set; }
        public string FullName
        {
            get
            {
                return $"{ FirstName } { LastName }";
            }
        }
        [Column("teamid")]
        public int? TeamId { get; set; }

        [JsonIgnore]
        public TeamModel? Team { get; set; }

    }
}
