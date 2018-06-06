using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Home
{
    public class ContactForm
    {
        [Display(Name = "Email")]
        public string FromEmail { get; set; }

        [Display(Name = "Name")]
        public string FromName { get; set; }

        public string Message { get; set; }
    }
}
