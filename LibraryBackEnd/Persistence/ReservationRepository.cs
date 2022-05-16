using System;
using System.Collections.Generic;
using Domain;
using Persistence.Interfaces;
using System.Linq;
using Domain.Enums;

namespace Persistence
{
    public class ReservationRepository : IReservationRepository
    {
        private List<Reservation> _reservations;
        
        public ReservationRepository()
        {
            _reservations = new List<Reservation>();
        }

        public void Create(Reservation book)
        {
            _reservations.Add(book);
        }

        public Reservation Read(Guid id)
        {
            return _reservations.SingleOrDefault(x => x.Id == id);
        }

        public List<Reservation> ReadAll()
        {
            return _reservations;
        }

        public List<Reservation> ReadActiveReservationsForBook(Guid bookId)
        {
            return _reservations
                .Where(x => x.State == ReservationState.Active)
                .Where(x => x.Book.Id == bookId)
                .ToList();
        }
        
        public List<Reservation> ReadReturnedReservationsForBook(Guid bookId)
        {
            return _reservations
                .Where(x => x.State != ReservationState.Active)
                .Where(x => x.Book.Id == bookId)
                .ToList();
        }

        public int GetAmountOfActiveReservationsForBook(Guid bookId)
        {
            return _reservations
                .Where(x => x.State == ReservationState.Active)
                .Count(x => x.Book.Id == bookId);
        }

        public void Update(Reservation reservation)
        {
            //obviously this is more mocking than real approach since the reservation will be updated in the service (by reference)
            var oldReservation = _reservations.SingleOrDefault(x => x.Id == reservation.Id);
            _reservations.Remove(oldReservation);
            _reservations.Add(reservation);
        }
    }
}