using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTO
{
    public class CreateReservationDTO
    {
        public string Date { get; set; }
        public string DepartureTime { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FromCode { get; set; }
        public string DestinationCode { get; set; }
        public string AirlineId { get; set; }
        public FlightClass FlightClass { get; set; }
        public string Seat { get; set; }
    }
}
