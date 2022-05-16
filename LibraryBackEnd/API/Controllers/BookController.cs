using System;
using System.Linq;
using API.Models;
using Business.Interfaces;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var book = _bookService.Get(id);
                return Ok(new BookDto
                {
                    Id = book.Id,
                    AmountOfCopies = book.AmountOfCopies,
                    Isbn = book.Isbn,
                    Name = book.Name,
                    Price = book.Price,
                    ActiveReservations = book.Reservations.Where(x => x.State == ReservationState.Active).Select(x => new ReservationDto
                    {
                        Id = x.Id,
                        DateTime = x.ReservationDateTime,
                        Fine = x.Fine,
                        InitialBookPrice = x.InitialBookPrice,
                        ReturnDateTime = x.ReturnDateTime,
                        State = x.State
                    }).ToList(),
                    FinishedReservations = book.Reservations.Where(x => x.State != ReservationState.Active).Select(x => new ReservationDto
                    {
                        Id = x.Id,
                        DateTime = x.ReservationDateTime,
                        Fine = x.Fine,
                        InitialBookPrice = x.InitialBookPrice,
                        ReturnDateTime = x.ReturnDateTime,
                        State = x.State
                    }).ToList(),
                    AmountOfCopiesAvailable = book.AmountOfCopies-book.Reservations.Count(x => x.State == ReservationState.Active)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var books = _bookService.GetAll();
                return Ok(books.Select(book => new BookDto
                {
                    Id = book.Id,
                    AmountOfCopies = book.AmountOfCopies,
                    Isbn = book.Isbn,
                    Name = book.Name,
                    Price = book.Price,
                    AmountOfCopiesAvailable = book.AmountOfCopies-book.Reservations.Count(x => x.State == ReservationState.Active)
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Create")]
        public IActionResult Create(CreateBookDto dto)
        {
            try
            {
                var book = new Book(dto.Isbn, dto.Name, dto.Price, dto.AmountOfCopies);
                _bookService.Create(book);
                return Ok(book.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}