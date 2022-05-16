using System;
using Domain.Enums;

namespace Domain
{
    public class Reservation
    {
        public Guid Id { get; }
        public DateTime ReservationDateTime { get; }
        public DateTime ReturnDateTime { get; set; }
        public double Fine { get; set; } //can be an apart class that also has some extra tracking info like: paid/unpaid state, when was paid,
        public double InitialBookPrice { get; } //question for business: what if the price of the book changes after you made a reservation? should the fine be calculated based on the initial book price?
        public Book Book { get; } //set null if book deleted but keep reservation
        public ReservationState State { get; set; }

        public Reservation(Book book)
        {
            Id = Guid.NewGuid();
            ReservationDateTime = DateTime.Now;
            Fine = 0.0;
            InitialBookPrice = book.Price;
            State = ReservationState.Active;
            Book = book;
        }
    }
}