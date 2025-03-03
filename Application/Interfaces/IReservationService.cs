using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<ReservationDTO> GetAll();
        ReservationDTO GetById(int id);
        ReservationDTO AddNewReservation(CreateReservationDTO newReservation);
        bool UpdateReservation(UpdateReservationDTO updateReservation);
        void UpdateMultipleReservations(Reservation res);
        bool DeleteReservation(int id);
        bool IsSeatFree(CreateReservationDTO newReservation);
    }
}
