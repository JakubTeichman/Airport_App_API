using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CreateFlightDTO
    {
        public string Date { get; set; }
        public string DepartureTime { get; set; }
        public string FromCode { get; set; }
        public string DestinationCode { get; set; }
        public string AirlineId { get; set; }

    }
}
