using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities;

namespace Domain.EntitiesDetails
{
    public class FlightDetails : Flight
    {
        public int Id { get; set; }
        public HashSet<Reservation> FlightReservations { get; set; }
        public FlightDetails(string fromCode, string destinationCode, string airlineId, string date, string departureTime) : base(fromCode, destinationCode, airlineId, date, departureTime) { }
    }
}
