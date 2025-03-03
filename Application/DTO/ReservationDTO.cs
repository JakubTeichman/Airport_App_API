using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTO
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string FlightId { get; set; }
        public string AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string From { get; set; }
        public string FromCode { get; set; }
        public string Destination { get; set; }
        public string DestinationCode { get; set; } 
        public FlightClass FlightClass { get; set; }
        public string Seat { get; set; }
    }
}
