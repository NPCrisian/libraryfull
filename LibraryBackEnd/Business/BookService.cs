using System;
using System.Collections.Generic;
using Business.Interfaces;
using Domain;
using Persistence.Interfaces;

namespace Business
{
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;
        private readonly IReservationRepository _reservationRepository;

        public BookService(IBookRepository bookRepository, IReservationRepository reservationRepository)
        {
            _bookRepository = bookRepository;
            _reservationRepository = reservationRepository;
        }
        
        public Book Create(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Isbn)) throw new Exception("Isbn cannot be empty!");
            if (string.IsNullOrWhiteSpace(book.Name)) throw new Exception("Name cannot be empty!");
            if (book.Price < 0.0) throw new Exception("Price cannot be smaller than 0!");
            if (book.AmountOfCopies < 0) throw new Exception("Amount of copies cannot be smaller than 0!");
            
            var existingBook = _bookRepository.ReadByIsbn(book.Isbn);
            if (existingBook != null)
                throw new Exception($"Book with this isbn ({book.Isbn}) already exists!");
            
            _bookRepository.Create(book);
            return book;
        }

        public List<Book> GetAll()
        {
            return _bookRepository.ReadAll();
        }

        public Book Get(Guid id)
        {
            var book = _bookRepository.Read(id);
            if(book == null)
                throw new Exception("Book not found!");
            return book;
        }
        
        public int GetAmountAvailableBooks(Guid id)
        {
            var book = _bookRepository.Read(id);
            if(book == null)
                throw new Exception("Book not found!");
            return book.AmountOfCopies - _reservationRepository.GetAmountOfActiveReservationsForBook(id);
        }
    }
}