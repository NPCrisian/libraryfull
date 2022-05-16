using System;
using System.Collections.Generic;

namespace API.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AmountOfCopies { get; set; }
        public int AmountOfCopiesAvailable { get; set; }
        public List<ReservationDto> ActiveReservations { get; set; }
        public List<ReservationDto> FinishedReservations { get; set; }
    }
}