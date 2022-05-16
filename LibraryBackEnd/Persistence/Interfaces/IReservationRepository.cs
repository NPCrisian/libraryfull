using System;
using System.Collections.Generic;
using Domain;

namespace Persistence.Interfaces
{
    public interface IReservationRepository
    {
        void Create(Reservation book);

        Reservation Read(Guid id);
        List<Reservation> ReadActiveReservationsForBook(Guid bookId);
        List<Reservation> ReadReturnedReservationsForBook(Guid bookId);
        int GetAmountOfActiveReservationsForBook(Guid bookId);
        void Update(Reservation reservation);
    }
}