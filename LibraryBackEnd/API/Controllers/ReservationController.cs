using System;
using API.Models;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<BookController> _logger;

        public ReservationController(ILogger<BookController> logger, IReservationService reservationService)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var reservation = _reservationService.Get(id);
                return Ok(new ReservationDto
                {
                    Id = reservation.Id,
                    DateTime = reservation.ReservationDateTime,
                    Fine = reservation.Fine,
                    InitialBookPrice = reservation.InitialBookPrice,
                    ReturnDateTime = reservation.ReturnDateTime,
                    State = reservation.State
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Create/{bookId}")]
        public IActionResult Create(Guid bookId)
        {
            try
            {
                var reservation = _reservationService.Create(bookId);
                return Ok(reservation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("ReturnBook")]
        public IActionResult ReturnBook(ReturnBookDto dto)
        {
            try
            {
                _reservationService.ReturnBook(dto.ReservationId, dto.ReturnDateTime);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}