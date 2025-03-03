using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using Application.WebScraping;
using AutoMapper;
using AutoMapper.Internal;
using Domain;
using Domain.Common;
using Domain.Entities;
using Domain.EntitiesDetails;
using Domain.Interfaces;

namespace Application.Services
{
    public class FlightDetailsService : IFlightDetailsService
    {
        private readonly IFlightDetailsRepository _flightDetailsRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        public FlightDetailsService(IFlightDetailsRepository flightDetailsRepository,IReservationRepository reservationRepository ,IMapper mapper)
        {
            _flightDetailsRepository = flightDetailsRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public FlightDTO AddNewFlight(CreateFlightDTO newFlight)
        {
            var flight = _mapper.Map<FlightDetails>(newFlight);
            
            if(WebScrapingConnections.ShowAirlineDetails(flight.FromCode, flight.DestinationCode).ContainsKey(flight.AirlineId))
            {
                flight.AirlineName = WebScrapingConnections.ShowAirlineName(flight.FromCode, flight.AirlineId);
            }
            else
            {
                flight.AirlineName = WebScrapingConnections.ShowAirlineDetails(flight.FromCode, flight.DestinationCode).FirstOrDefault().Value;
            }
            
            flight = FlightComplementation(flight);
            _flightDetailsRepository.Add(flight);
            return _mapper.Map<FlightDTO>(flight);

        }

        public bool DeleteFlight(int id)
        {
            var flight = _flightDetailsRepository.GetById(id);
            if (flight != null)
            {
                foreach (var res in flight.FlightReservations)
                {
                    _reservationRepository.Delete(res);
                }
                _flightDetailsRepository.Delete(flight);
                return true;
            }            
            return false;
        }

        public IEnumerable<FlightDTO> GetAll()
        {
            var flights = _flightDetailsRepository.GetAll();
            return _mapper.Map<IEnumerable<FlightDTO>>(flights);
        }

        public FlightDTO GetById(int id)
        {
            var flight = _flightDetailsRepository.GetById(id);
            return _mapper.Map<FlightDTO>(flight);
        }

        public List<Reservation> UpdateFlight(UpdateFlightDTO updateFlight, int id)
        {
            List<Reservation> updateReservationDTOs = new List<Reservation>();
            var existingFlight = _flightDetailsRepository.GetById(id);

            if (string.IsNullOrEmpty(updateFlight.FromCode))
            {
                updateFlight.FromCode = existingFlight.FromCode;
            }
            if (string.IsNullOrEmpty(updateFlight.DestinationCode))
            {
                updateFlight.DestinationCode = existingFlight.DestinationCode;
            }
            if (string.IsNullOrEmpty(updateFlight.AirlineId))
            {
                updateFlight.AirlineId = existingFlight.AirlineId;
            }
            if (string.IsNullOrEmpty(updateFlight.DepartureTime))
            {
                updateFlight.DepartureTime = existingFlight.DepartureTime;
            }
            if (string.IsNullOrEmpty(updateFlight.Date))
            {
                updateFlight.Date = existingFlight.Date;
            }
            var flight = _mapper.Map(updateFlight, existingFlight);
            
            if (WebScrapingConnections.ShowAirlineDetails(flight.FromCode, flight.DestinationCode).ContainsKey(flight.AirlineId))
            {
                flight.AirlineName = WebScrapingConnections.ShowAirlineName(flight.FromCode, flight.AirlineId);
            }
            else
            {
                flight.AirlineName = WebScrapingConnections.ShowAirlineDetails(flight.FromCode, flight.DestinationCode).FirstOrDefault().Value;
            }

            flight = FlightComplementation(flight);

            if (flight.FlightReservations != null)
            {
                foreach (var res in flight.FlightReservations)
                {
                    res.AirlineId = flight.AirlineId;
                    res.FlightId = flight.FlightId;
                    res.FromCode = flight.FromCode;
                    res.DestinationCode = flight.DestinationCode;
                    res.Date = flight.Date;
                    res.DepartureTime = flight.DepartureTime;
                    res.From = AirportDetailsRapidAPI.LocationOfAirport(flight.FromCode).Result;
                    res.Destination = AirportDetailsRapidAPI.LocationOfAirport(flight.DestinationCode).Result;
                    res.AirlineName = WebScrapingConnections.ShowAirlineName(flight.FromCode, flight.AirlineId);
                    res.FullFlightId = flight.AirlineId + flight.FlightId;
                    res.Duration = AirportDetailsRapidAPI.DurationCounter(flight.FromCode, flight.DestinationCode).Result;
                    res.Distance = AirportDetailsRapidAPI.FormatedDistanceCounter(flight.FromCode, flight.DestinationCode).Result;
                    res.TimeDiffrence = AirportDetailsRapidAPI.TimeDiffrenceCounter(flight.FromCode, flight.DestinationCode).Result;
                    updateReservationDTOs.Add(res);
                }
            }
            var fullFlight = _mapper.Map(flight, existingFlight);
            fullFlight.FlightReservations = new HashSet<Reservation>(_reservationRepository.GetAll().Where(x => x.FullFlightId == fullFlight.FullFlightId).ToHashSet());
            _flightDetailsRepository.Update(fullFlight);

            return updateReservationDTOs;
        }
        private FlightDetails FlightComplementation(FlightDetails flight)
        {
            flight.From = AirportDetailsRapidAPI.LocationOfAirport(flight.FromCode).Result;
            flight.Destination = AirportDetailsRapidAPI.LocationOfAirport(flight.DestinationCode).Result;
            flight.AirlineName = WebScrapingConnections.ShowAirlineName(flight.FromCode, flight.AirlineId);
            flight.FullFlightId = flight.AirlineId + flight.FlightId;
            flight.Duration = AirportDetailsRapidAPI.DurationCounter(flight.FromCode, flight.DestinationCode).Result;
            flight.Distance = AirportDetailsRapidAPI.FormatedDistanceCounter(flight.FromCode, flight.DestinationCode).Result;
            flight.TimeDiffrence = AirportDetailsRapidAPI.TimeDiffrenceCounter(flight.FromCode, flight.DestinationCode).Result;
            
            return flight;
        }

    }
}
