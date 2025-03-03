using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.WebScraping;
using Domain;
using Domain.Entities;

namespace Application.DTO
{
    public class FlightDTO
    {
        public int Id { get; set; } 
        public string FromCode { get; set; }
        public string From { get; set; }
        public string DestinationCode { get; set; }
        public string Destination { get; set; }
        public string FlightId { get; set; }
        public string AirlineId { get; set; } 
        public string AirlineName { get; set; }
        public string FullFlightId { get; set; }
        public string Duration { get; set; }
        public string Distance { get; set; }
        public string TimeDiffrence { get; set; }
        public string Date { get; set; }
        public string DepartureTime { get; set; }
        public HashSet<Reservation> FlightReservations { get; set; }
    }
}
