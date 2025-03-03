using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interfaces
{
    public interface IConnectionService
    {
        ConnectionDTO AirportLocation(string iataCode);
        ConnectionDTO GetAllTransfers(string iataCodeFrom, string iataCodeTo);
        bool IsDirectConnection(string iataCodeFrom, string iataCodeTo);
        ConnectionDTO GetConnections(string iataCodeFrom, string iataCodeTo);
        ConnectionDTO GetAllConnections(string iataCodeFrom);
    }
}
