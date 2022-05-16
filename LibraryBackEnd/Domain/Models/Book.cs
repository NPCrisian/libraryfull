using System;
using System.Collections.Generic;

namespace Domain
{
    public class Book
    {
        public Guid Id { get; }
        public string Isbn { get; }
        public string Name { get; }
        public double Price { get; set; }
        public int AmountOfCopies { get; set; }
        public List<Reservation> Reservations { get; }

        public Book(string isbn, string name, double price, int amountOfCopies)
        {
            Id = Guid.NewGuid();
            Isbn = isbn;
            Name = name;
            Price = price;
            AmountOfCopies = amountOfCopies;
            Reservations = new List<Reservation>();
        }

        public void AddReservation(Reservation reservation)
        {
            Reservations.Add(reservation);
        }
    }
}