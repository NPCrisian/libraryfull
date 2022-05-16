using System;
using System.Collections.Generic;
using Domain;

namespace Business.Interfaces
{
    public interface IReservationService
    {
        Reservation Get(Guid id);
        Reservation Create(Guid bookId);
        void ReturnBook(Guid reservationId, DateTime returnDateTime);
        List<Reservation> GetActiveReservationsForBook(Guid bookId);
        List<Reservation> GetReturnedReservationsForBook(Guid bookId);
    }
}