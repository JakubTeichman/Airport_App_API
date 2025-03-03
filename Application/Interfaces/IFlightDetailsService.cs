using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFlightDetailsService
    {
        IEnumerable<FlightDTO> GetAll();
        FlightDTO GetById(int id);
        FlightDTO AddNewFlight(CreateFlightDTO newFlight);
        List<Reservation> UpdateFlight(UpdateFlightDTO updateFlight, int id);
        bool DeleteFlight(int id);
    }
}
