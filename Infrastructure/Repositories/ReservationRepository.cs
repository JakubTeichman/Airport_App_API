using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;
using Domain.Entities;
using Domain.Interfaces;


namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private static readonly ISet<Reservation> _reservations = new HashSet<Reservation>();
        public IEnumerable<Reservation> GetAll()
        {
            return _reservations;
        }

        public List<Reservation> GetByFullFlightId(string fullFlightId)
        {
            return _reservations.Where(x => x.FullFlightId == fullFlightId).ToList();
        }

        public Reservation GetById(int id)
        {
            return _reservations.SingleOrDefault(x => x.Id == id);
        }
        public Reservation Add(Reservation reservation)
        {
            int newId = _reservations.Count() + 1;
            while(GetById(newId) != null)
            {
                newId++;
            }
            reservation.Id = newId;
            reservation.Created = DateTime.Now;
            _reservations.Add(reservation);
            return reservation;
        }
        public void Delete(Reservation reservation)
        {
            _reservations.Remove(reservation);
        }

        public void Update(Reservation reservation)
        {
            reservation.Updated = DateTime.Now;
        }
        
    }
}
