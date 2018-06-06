using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Home
{
    public class Blog
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Url
        {
            get { return "https://blog.bitscry.com/" + Date.ToString("yyyy/MM/dd") + "/" + Name; }
        }
    }
}
