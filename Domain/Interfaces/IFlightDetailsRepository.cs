using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.EntitiesDetails;

namespace Domain.Interfaces
{
    public interface IFlightDetailsRepository
    {
        IEnumerable<FlightDetails> GetAll();
        FlightDetails GetById(int id);
        FlightDetails GetByFullFlightId(string fullFlightId);
        FlightDetails Add(FlightDetails flight);
        void Update(FlightDetails flight);
        void Delete(FlightDetails flight);
    }
}
