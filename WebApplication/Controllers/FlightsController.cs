using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Application.WebScraping;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : Controller
    {
        private readonly IFlightDetailsService _flightServise;
        private readonly IReservationService _reservationServise;
        private readonly IMapper _mapper;
        public FlightsController(IFlightDetailsService flightServise, IReservationService reservationServise, IMapper mapper)
        {
            _flightServise = flightServise;
            _reservationServise = reservationServise;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var flights = _flightServise.GetAll();
            return Ok(flights);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var flight = _flightServise.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
        [HttpPost]
        public IActionResult Create(CreateFlightDTO newFlight)
        {
            try
            {
                if (WebScrapingConnections.IsConnectionDirect(newFlight.FromCode, newFlight.DestinationCode))
                {
                    var flight = _flightServise.AddNewFlight(newFlight);
                    return Created($"api/Flights/{flight.Id}", flight);
                }
                else
                {
                    return BadRequest("Yuo can only create a direct flight");
                }
                    
            }
            catch { return BadRequest("Incorrect iata code/codes!"); }
        }
        [HttpPut]
        [Route("{id}")]

        public IActionResult Update(UpdateFlightDTO updatedFlight, [FromRoute]int id)
        {
            var flight = _flightServise.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }
            try
            {
                if (WebScrapingConnections.IsConnectionDirect(updatedFlight.FromCode, updatedFlight.DestinationCode))
                {
                    var reservations = _flightServise.UpdateFlight(updatedFlight, id);
                    foreach (var res in reservations)
                    {
                        _reservationServise.UpdateMultipleReservations(res);
                    }
                    return NoContent();
                }
                else
                {
                    return BadRequest("You can only create a direct flight");
                }
                
            }
            catch 
            {
                _flightServise.UpdateFlight(_mapper.Map<UpdateFlightDTO>(flight), id);
                return BadRequest("Incorrect iata code/codes!"); 
            }
            
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if(_flightServise.DeleteFlight(id))
            {
                _flightServise.DeleteFlight(id);
                return NoContent();
            }
            else { return NotFound($"There is no such flight with id: {id}"); }
            
        }

    }
}
