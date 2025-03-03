using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.EntitiesDetails;

namespace Domain.Entities
{
    public class Reservation : Flight
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public FlightClass FlightClass { get; set; }
        public string Seat { get; set; }

        public Reservation() { }
        public Reservation(string fromCode, string destinationCode, string airlineId, string date, string departureTime, FlightClass flightClass, string seat, string name, string surname) : base(fromCode, destinationCode, airlineId, date, departureTime)
        {
            Name = name;
            Surname = surname;
            FlightClass = flightClass;
            Seat = seat;
        }


    }
}
