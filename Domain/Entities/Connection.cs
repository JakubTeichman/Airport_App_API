using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Connection
    {
        public string IataCodeFrom { get; set; }
        public string CityFrom { get; set; } 
        public string IataCodeTo { get; set; }
        public string CityTo { get; set; }
        public bool IsDirect { get; set; }
        public HashSet<string> Cities { get; set; }
        public Dictionary<string, string> Airlines { get; set; }
    }
}
