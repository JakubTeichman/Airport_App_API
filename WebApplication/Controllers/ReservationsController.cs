using System.Diagnostics.Eventing.Reader;
using Application.DTO;
using Application.Interfaces;
using Application.WebScraping;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationServise;
        private readonly IMapper _mapper;
        public ReservationsController(IReservationService reservationServise, IMapper mapper)
        {
            _reservationServise = reservationServise;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var reservations = _reservationServise.GetAll();
            return Ok(reservations);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reservation = _reservationServise.GetById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }
        [HttpPost]
        public IActionResult Create(CreateReservationDTO newReservation)
        {
            try
            {
                if (WebScrapingConnections.IsConnectionDirect(newReservation.FromCode, newReservation.DestinationCode))
                {
                    if (_reservationServise.IsSeatFree(newReservation))
                    {
                        var reservation = _reservationServise.AddNewReservation(newReservation);
                        return Created($"api/Reservation/{reservation.Id}", reservation);
                    }
                    else
                    {
                        return BadRequest("That seat is already taken, choose diffrent one.");
                    }
                }
                else { return BadRequest("You can only create reservation on direct flight!"); }
                
            }
            catch { return BadRequest("Incorrect iata code/codes!"); }
            
        }

        [HttpPut]
        public IActionResult Update(UpdateReservationDTO updateReservation)
        {
            if(_reservationServise.UpdateReservation(updateReservation))
            {
                if (_reservationServise.IsSeatFree(_mapper.Map<CreateReservationDTO>(_reservationServise.GetById(updateReservation.Id))))
                {
                    _reservationServise.UpdateReservation(updateReservation);
                    return NoContent();
                }
                else
                {
                    return BadRequest("That seat is already taken, choose diffrent one.");
                }

            }
            else { return NotFound($"There is no such reservation with id:{updateReservation.Id}"); }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if(_reservationServise.DeleteReservation(id))
            {
                return NoContent();
            }
            else { return NotFound($"There is no such reservation with id:{id}"); }            
        }
    }
}
