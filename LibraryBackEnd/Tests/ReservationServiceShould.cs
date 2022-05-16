using System;
using Business;
using Business.Interfaces;
using Domain;
using Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using Persistence;

namespace Tests
{
    public class ReservationServiceShould
    {
        private IBookService _bookService;
        private IReservationService _reservationService;
        
        [SetUp]
        public void Setup()
        {
            var bookRepository = new BookRepository();
            var reservationRepository = new ReservationRepository();
            _bookService = new BookService(bookRepository, reservationRepository);
            _reservationService = new ReservationService(reservationRepository, bookRepository);
        }

        [Test]
        public void CreateAndGetReservation()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);
            var timeStamp = DateTime.Now;
            
            //Act
            var createdReservation = _reservationService.Create(book.Id);

            //Assert
            createdReservation.Id.Should().NotBe(Guid.Empty);
            createdReservation.Fine.Should().Be(0.0);
            createdReservation.State.Should().Be(ReservationState.Active);
            createdReservation.InitialBookPrice.Should().Be(book.Price);
            createdReservation.ReservationDateTime.Should().BeAfter(timeStamp);
        }
        
        [Test]
        public void FailOnCreateReservationWithNotExistingBook()
        {
            //Assert
            Assert.Throws<Exception>(() => _reservationService.Create(Guid.Empty));
        }
        
        [Test]
        public void FailOnCreateReservationForUnavailableBook()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);
            for (int i = 0; i < amountOfCopies; i++)
                _reservationService.Create(book.Id);
            
            //Assert
            Assert.Throws<Exception>(() => _reservationService.Create(book.Id));
        }
        
        [Test]
        public void ReturnBook()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);
            var reservation = _reservationService.Create(book.Id);
            var returnDateTime = DateTime.Now.AddDays(10);
            
            //Act
            _reservationService.ReturnBook(reservation.Id, returnDateTime);

            //Assert
            reservation.State.Should().Be(ReservationState.Returned);
            reservation.Fine.Should().Be(0.0);
        }
        
        [Test]
        public void ReturnBookWithFine()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);
            var reservation = _reservationService.Create(book.Id);
            var returnDateTime = DateTime.Now.AddDays(20);

            //Act
            _reservationService.ReturnBook(reservation.Id, returnDateTime);

            //Assert
            reservation.State.Should().Be(ReservationState.ReturnedLate);
            reservation.Fine.Should().Be(20.0*0.01*6);
        }
        
        [Test]
        public void FailOnReturnBookWithReturnDateTimeSmallerThanReservationDateTime()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);
            var reservation = _reservationService.Create(book.Id);
            
            //Assert
            Assert.Throws<Exception>(() => _reservationService.ReturnBook(reservation.Id, DateTime.Now.AddDays(-1)));
        }
        
        [Test]
        public void GetActiveReservationsForBook()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);

            for (int i = 0; i < 5; i++)
                _reservationService.Create(book.Id);
            
            //Act
            var activeReservations = _reservationService.GetActiveReservationsForBook(book.Id);

            //Assert
            activeReservations.Count.Should().Be(5);
        }

        [Test]
        public void GetReturnedReservationsForBook()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);

            for (int i = 0; i < 5; i++)
            {
                var reservation = _reservationService.Create(book.Id);
                _reservationService.ReturnBook(reservation.Id, DateTime.Now);
            }
            
            //Act
            var returnedReservations = _reservationService.GetReturnedReservationsForBook(book.Id);

            //Assert
            returnedReservations.Count.Should().Be(5);
        }
    }
}