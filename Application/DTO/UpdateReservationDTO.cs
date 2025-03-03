using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTO
{
    public class UpdateReservationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public FlightClass FlightClass { get; set; }
        public string Seat { get; set; }
    }
}
