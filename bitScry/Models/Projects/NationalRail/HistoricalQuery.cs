using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.NationalRail
{
    public class HistoricalQuery
    {
        [Display(Name = "From")]
        public string FromCrs { get; set; }

        [Display(Name = "To")]
        public string ToCrs { get; set; }

        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}
