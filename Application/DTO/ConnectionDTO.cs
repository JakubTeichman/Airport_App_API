using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.WebScraping;
using Domain;

namespace Application.DTO
{
    public class ConnectionDTO
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
