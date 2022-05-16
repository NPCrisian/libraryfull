using System;
using System.Collections.Generic;
using Business.Interfaces;
using Domain;
using Domain.Enums;
using Persistence.Interfaces;

namespace Business
{
    public class ReservationService : IReservationService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository, IBookRepository bookRepository)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
        }
        
        public Reservation Get(Guid id)
        {
            var reservation = _reservationRepository.Read(id);
            if(reservation == null)
                throw new Exception($"Reservation not found!");
            return reservation;
        }
        
        public Reservation Create(Guid bookId)
        {
            var book = _bookRepository.Read(bookId);
            if(book == null)
                throw new Exception($"Book not found!");
            
            var amountOfReservations = _reservationRepository.GetAmountOfActiveReservationsForBook(bookId);
            if(amountOfReservations+1 > book.AmountOfCopies)
                throw new Exception($"No available book for now!");
            var reservation = new Reservation(book);
            book.AddReservation(reservation);
            _reservationRepository.Create(reservation);
            return reservation;
        }

        public void ReturnBook(Guid reservationId, DateTime returnDateTime)
        {
            var reservation = _reservationRepository.Read(reservationId);
            if(reservation == null)
                throw new Exception($"Reservation not found!");
            if(reservation.State != ReservationState.Active)
                throw new Exception($"Book was already brought back!");
            if(returnDateTime.Date < reservation.ReservationDateTime.Date)
                throw new Exception($"Return date cannot be smaller than initial reservation time!");
            var amountOfDaysLate = (returnDateTime.Date - reservation.ReservationDateTime.Date).Days-14;
            if (amountOfDaysLate > 0)
            {
                reservation.Fine = reservation.InitialBookPrice * 0.01 * amountOfDaysLate;
                reservation.State = ReservationState.ReturnedLate;
            }
            else
                reservation.State = ReservationState.Returned;
            reservation.ReturnDateTime = returnDateTime;
            _reservationRepository.Update(reservation);
        }

        public List<Reservation> GetActiveReservationsForBook(Guid bookId)
        {
            var book = _bookRepository.Read(bookId);
            if (book == null)
                throw new Exception("Book not found!");
            return _reservationRepository.ReadActiveReservationsForBook(bookId);
        }

        public List<Reservation> GetReturnedReservationsForBook(Guid bookId)
        {
            var book = _bookRepository.Read(bookId);
            if (book == null)
                throw new Exception("Book not found!");
            return _reservationRepository.ReadReturnedReservationsForBook(bookId);
        }
    }
}