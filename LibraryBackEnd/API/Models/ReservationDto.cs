using System;
using Domain.Enums;

namespace API.Models
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public double Fine { get; set; } 
        public double InitialBookPrice { get; set; } 
        public ReservationState State { get; set; }
    }
}