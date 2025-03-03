using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using Application.WebScraping;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IMapper _mapper;
        public ConnectionService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ConnectionDTO GetAllTransfers(string iataCodeFrom, string iataCodeTo)
        {
            Connection connection = new Connection();
            var connectionDetails = WebScrapingConnections.ShowTransfers(iataCodeFrom, iataCodeTo);
            connection.IataCodeFrom = iataCodeFrom;
            connection.IataCodeTo = iataCodeTo;
            connection.IsDirect = false;
            connection = Complenetation(connection);
            connection.Cities = connectionDetails;
            return _mapper.Map<ConnectionDTO>(connection);
        }

        public ConnectionDTO GetAllConnections(string iataCodeFrom)
        {
            var connectionDetails = WebScrapingFlights.Connections(iataCodeFrom);
            connectionDetails.IataCodeFrom = iataCodeFrom;
            connectionDetails.IsDirect = true;
            connectionDetails = Complenetation(connectionDetails);
            connectionDetails.Airlines = WebScrapingConnections.ShowAirlineDetails(connectionDetails.IataCodeFrom);
            return _mapper.Map<ConnectionDTO>(connectionDetails);
        }
        public ConnectionDTO GetConnections(string iataCodeFrom, string iataCodeTo)
        {
            if(IsDirectConnection(iataCodeFrom, iataCodeTo) == false)
            {
                return GetAllTransfers(iataCodeFrom, iataCodeTo);
            }
            else
            {
                var connection = GetAllConnections(iataCodeFrom);
                connection.IataCodeTo = iataCodeTo;
                connection.Cities.Clear();
                return connection;
            }
        }

        public bool IsDirectConnection(string iataCodeFrom, string iataCodeTo)
        {
            return WebScrapingConnections.IsConnectionDirect(iataCodeFrom, iataCodeTo);
        }
        public ConnectionDTO AirportLocation(string iataCode)
        {
            var city = AirportDetailsRapidAPI.LocationOfAirport(iataCode).Result;
            Connection finalCity = new Connection();
            finalCity.IataCodeFrom = iataCode;
            finalCity.IsDirect = true;
            HashSet<string> connections = new HashSet<string>() {city};
            finalCity.Cities = connections;
            var location = _mapper.Map<ConnectionDTO>(finalCity);
            return location;
        }

        private Connection Complenetation(Connection connection)
        {
            connection.CityFrom = AirportDetailsRapidAPI.LocationOfAirport(connection.IataCodeFrom).Result;
            if (string.IsNullOrEmpty(connection.IataCodeTo) == false)
            {
                connection.CityTo = AirportDetailsRapidAPI.LocationOfAirport(connection.IataCodeTo).Result;
                if (IsDirectConnection(connection.IataCodeFrom, connection.IataCodeTo))
                {
                    connection.Airlines = WebScrapingConnections.ShowAirlineDetails(connection.IataCodeFrom, connection.IataCodeTo);
                }
            }
            else
            {
                connection.Airlines = WebScrapingConnections.ShowAirlineDetails(connection.IataCodeFrom);
            }
            
            return connection;
        }
    }
}
