using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain;
using Domain.EntitiesDetails;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class FlightDetailsRepository : IFlightDetailsRepository
    {
        private static readonly ISet<FlightDetails> _flights = new HashSet<FlightDetails>()
        {
            //new Reservation(1, 2343, "W6", "Jakub", "Teichman", "Kraków", "Vienna", FlightClass.Economy, "A12", false),
            //new Reservation(2, 5436, "FR", "Karolina", "Teichman", "Warszawa", "London", FlightClass.Business, "B13", false)
        };
        public IEnumerable<FlightDetails> GetAll()
        {
            return _flights;
        }

        public FlightDetails GetByFullFlightId(string fullFlightId)
        {
            return _flights.SingleOrDefault(x => x.FullFlightId == fullFlightId);
        }

        public FlightDetails GetById(int id)
        {
            return _flights.SingleOrDefault(x => x.Id == id);
        }
        public FlightDetails Add(FlightDetails flight)
        {
            int newId = _flights.Count() + 1;
            while (GetById(newId) != null)
            {
                newId++;
            }
            flight.Id = newId;
            if (flight.Id.ToString().Length < 4)
            {
                for (int i = 0; i < 4 - flight.Id.ToString().Length; i++)
                {
                    flight.FlightId += "0";
                }
                flight.FlightId += flight.Id.ToString();
            }
            else { flight.FlightId = flight.Id.ToString(); }
            flight.FullFlightId = flight.AirlineId + flight.FlightId.ToString();
            flight.Created = DateTime.Now;
            _flights.Add(flight);
            return flight;
        }
        public void Delete(FlightDetails flight)
        {
            _flights.Remove(flight);
        }

        public void Update(FlightDetails flight)
        {
            flight.Updated = DateTime.Now;
        }
    }
}
