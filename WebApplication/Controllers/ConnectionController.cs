using Application.Interfaces;
using Application.WebScraping;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : Controller
    {
        private readonly IConnectionService _connectionServise;

        public ConnectionController(IConnectionService connectionServise)
        {
            _connectionServise = connectionServise;
        }

        [HttpGet("location-of-{iataCode}")]
        public async Task<IActionResult> CheckAirportLocationAsync([FromRoute]string iataCode)
        {
            var city = _connectionServise.AirportLocation(iataCode);
            return Ok(city);
        }
        [HttpGet("details-of-{iataCode}")]
        public async Task<IActionResult> CheckAirportDetailsAsync([FromRoute] string iataCode)
        {
            var details = AirportDetailsRapidAPI.AirportDetailsFinder(iataCode).Result;
            return Ok(details);
        }
        [HttpGet("{iataCodeFrom}-to-{iataCodeTo}")]
        public async Task<IActionResult> CheckTransfersAsync([FromRoute] string iataCodeFrom, [FromRoute]string iataCodeTo)
        {
            var connection = _connectionServise.GetConnections(iataCodeFrom, iataCodeTo);
            return Ok(connection);
        }
        [HttpGet("from-{iataCodeFrom}")]
        public async Task<IActionResult> CheckConnectionsAsync([FromRoute] string iataCodeFrom)
        {
            var connection = _connectionServise.GetAllConnections(iataCodeFrom);
            return Ok(connection);
        }





    }
}
