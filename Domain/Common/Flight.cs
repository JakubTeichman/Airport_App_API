using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class Flight : AuditableEntity
    {
        public string FromCode { get; set; }
        public string From { get; set; }
        public string DestinationCode { get; set; }
        public string Destination  { get; set; } 
        public string FlightId { get; set; } 
        public string AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string FullFlightId { get; set; }
        public string Duration { get; set; }  
        public string Distance { get; set; }
        public string TimeDiffrence { get; set; }
        public string Date { get; set; }
        public string DepartureTime { get; set; }

        public Flight () { }
        
        public Flight (string fromCode, string destinationCode, string airlineId, string date, string departureTime)
        {
            (FromCode, DestinationCode, AirlineId, Date, DepartureTime) = (fromCode, destinationCode, airlineId, date, departureTime);
        }
    }
}
