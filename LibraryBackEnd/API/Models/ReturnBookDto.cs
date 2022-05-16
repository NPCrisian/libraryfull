using System;

namespace API.Models
{
    public class ReturnBookDto
    {
        public Guid ReservationId { get; set; }
        public DateTime ReturnDateTime { get; set; }
    }
}