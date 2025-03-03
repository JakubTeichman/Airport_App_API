using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IFlightDetailsRepository _flightDetailsRepository;
        private readonly IMapper _mapper;
        public ReservationService(IReservationRepository reservationRepository,IFlightDetailsRepository flightDetailsRepository,IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _flightDetailsRepository = flightDetailsRepository;
            _mapper = mapper;
        }

        public ReservationDTO AddNewReservation(CreateReservationDTO newReservation)
        {
            var reservation = _mapper.Map<Reservation>(newReservation);
            reservation = ReservationComplementation(reservation);
            var flight = _flightDetailsRepository.GetAll().Where(x => x.FromCode == reservation.FromCode && x.DestinationCode == reservation.DestinationCode && x.Date == reservation.Date && x.DepartureTime == reservation.DepartureTime && x.AirlineId == reservation.AirlineId).FirstOrDefault();
            if (flight == null)
            {
                var newFlight = new FlightDetails(reservation.FromCode, reservation.DestinationCode, reservation.AirlineId, reservation.Date, reservation.DepartureTime);
                
                if (WebScrapingConnections.ShowAirlineDetails(newFlight.FromCode, newFlight.DestinationCode).ContainsKey(newFlight.AirlineId))
                {
                    newFlight.AirlineName = WebScrapingConnections.ShowAirlineName(newFlight.FromCode, newFlight.AirlineId);
                }
                else
                {
                    newFlight.AirlineName = WebScrapingConnections.ShowAirlineDetails(newFlight.FromCode, newFlight.DestinationCode).FirstOrDefault().Value;
                }
                
                newFlight = FlightComplementation(newFlight);
                _flightDetailsRepository.Add(newFlight);

                reservation.FlightId = newFlight.FlightId;
                reservation.FullFlightId = reservation.AirlineId + reservation.FlightId;
                HashSet<Reservation> reservations = new HashSet<Reservation>() { reservation };
                newFlight.FlightReservations = reservations;
                
            }
            else
            {
                reservation.FlightId = flight.FlightId;
                reservation.FullFlightId = reservation.AirlineId + reservation.FlightId;
                if (flight.FlightReservations != null)
                {
                    flight.FlightReservations.Add(reservation);
                }
                else
                {
                    HashSet<Reservation> reservations = new HashSet<Reservation>() { reservation };
                    flight.FlightReservations = reservations;
                }
            }
            _reservationRepository.Add(reservation);

            return _mapper.Map<ReservationDTO>(reservation);

        }

        public bool DeleteReservation(int id)
        {
            var reservation = _reservationRepository.GetById(id);
            if (reservation != null)
            {
                var flight = _flightDetailsRepository.GetByFullFlightId(reservation.FullFlightId);
                flight.FlightReservations.Remove(reservation);
                _reservationRepository.Delete(reservation);
                return true;
            }
            return false;
        }

        public IEnumerable<ReservationDTO> GetAll()
        {
            var reservations = _reservationRepository.GetAll();
            return _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
        }

        public ReservationDTO GetById(int id)
        {
            var reservation = _reservationRepository.GetById(id);
            return _mapper.Map<ReservationDTO>(reservation);    
        }

        public bool UpdateReservation(UpdateReservationDTO updateReservation)
        {
            var existingReservation = _reservationRepository.GetById(updateReservation.Id);

            if (existingReservation != null)
            {
                if (string.IsNullOrEmpty(updateReservation.Seat))
                {
                    updateReservation.Seat = existingReservation.Seat;
                }
                if (string.IsNullOrEmpty(updateReservation.Name))
                {
                    updateReservation.Name = existingReservation.Name;
                }
                if (string.IsNullOrEmpty(updateReservation.Surname))
                {
                    updateReservation.Surname = existingReservation.Surname;
                }
                if (string.IsNullOrEmpty(updateReservation.FlightClass.ToString()))
                {
                    updateReservation.FlightClass = existingReservation.FlightClass;
                }

                var reservation = _mapper.Map(updateReservation, existingReservation);
                _reservationRepository.Update(reservation);
                return true;
            }
            return false;
            
        }
        public void UpdateMultipleReservations(Reservation res)
        {
            var existingReservation = _reservationRepository.GetById(res.Id);
            if (string.IsNullOrEmpty(res.Seat))
            {
                res.Seat = existingReservation.Seat;
            }
            if (string.IsNullOrEmpty(res.Name))
            {
                res.Name = existingReservation.Name;
            }
            if (string.IsNullOrEmpty(res.Surname))
            {
                res.Surname = existingReservation.Surname;
            }
            if (string.IsNullOrEmpty(res.FlightClass.ToString()))
            {
                res.FlightClass = existingReservation.FlightClass;
            }
            var reservation = ReservationComplementation(res);
            reservation = _mapper.Map(res, existingReservation);
            _reservationRepository.Update(reservation);
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
        private Reservation ReservationComplementation(Reservation flight)
        {
            
            flight.From = AirportDetailsRapidAPI.LocationOfAirport(flight.FromCode).Result;
            flight.Destination = AirportDetailsRapidAPI.LocationOfAirport(flight.DestinationCode).Result;
            flight.From = AirportDetailsRapidAPI.LocationOfAirport(flight.FromCode).Result;
            flight.Destination = AirportDetailsRapidAPI.LocationOfAirport(flight.DestinationCode).Result;
            flight.AirlineName = WebScrapingConnections.ShowAirlineName(flight.FromCode, flight.AirlineId);
            flight.FullFlightId = flight.AirlineId + flight.FlightId;
            flight.Duration = AirportDetailsRapidAPI.DurationCounter(flight.FromCode, flight.DestinationCode).Result;
            flight.Distance = AirportDetailsRapidAPI.FormatedDistanceCounter(flight.FromCode, flight.DestinationCode).Result;
            flight.TimeDiffrence = AirportDetailsRapidAPI.TimeDiffrenceCounter(flight.FromCode, flight.DestinationCode).Result;
          
            return flight;
        }

        public bool IsSeatFree(CreateReservationDTO newReservation)
        {
            var chosenReservation = _reservationRepository.GetAll().Where(x => x.AirlineId == newReservation.AirlineId && x.FromCode == newReservation.FromCode && x.DestinationCode == newReservation.DestinationCode && x.DepartureTime == newReservation.DepartureTime && x.Date == newReservation.Date).ToList();
            if (chosenReservation != null)
            {
                foreach (var res in chosenReservation)
                {
                    if (res.Seat == newReservation.Seat) return false;
                }
                
            }
            return true;
        }

    }
}
