using System;
using Business;
using Business.Interfaces;
using Domain;
using NUnit.Framework;
using Persistence;
using FluentAssertions;

namespace Tests
{
    public class BookServiceShould
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
        public void CreateBook()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Act
            var createdBook = _bookService.Create(book);

            //Assert
            createdBook.Id.Should().NotBe(Guid.Empty);
            createdBook.Isbn.Should().Be(isbn);
            createdBook.Price.Should().Be(price);
            createdBook.AmountOfCopies.Should().Be(amountOfCopies);
        }
        
        [Test]
        public void FailOnCreateBookWithEmptyIsbn()
        {
            //Arrange
            string isbn = "";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithWhiteSpaceIsbn()
        {
            //Arrange
            string isbn = " ";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithNullIsbn()
        {
            //Arrange
            string isbn = null;
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithEmptyName()
        {
            //Arrange
            string isbn = "isbn";
            string name = "";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithWhiteSpaceName()
        {
            //Arrange
            string isbn = "isbn";
            string name = " ";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithNullName()
        {
            //Arrange
            string isbn = "isbn";
            string name = null;
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithNegativePrice()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = -20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void FailOnCreateBookWithNegativeAmountOfCopies()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = -10;
            var book = new Book(isbn, name, price, amountOfCopies);

            //Assert
            Assert.Throws<Exception>(() => _bookService.Create(book));
        }
        
        [Test]
        public void GetAmountAvailableBooks()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            var book = new Book(isbn, name, price, amountOfCopies);
            _bookService.Create(book);

            var amountOfReservations = 7;
            for (int i = 0; i < amountOfReservations; i++)
                _reservationService.Create(book.Id);
            var reservationToFinish = _reservationService.Create(book.Id);
            _reservationService.ReturnBook(reservationToFinish.Id, DateTime.Now);
            
            //Act
            var amountAvailableBooks = _bookService.GetAmountAvailableBooks(book.Id);

            //Assert
            amountAvailableBooks.Should().Be(amountOfCopies - amountOfReservations);
        }
        
        [Test]
        public void GetAll()
        {
            //Arrange
            string isbn = "isbn";
            string name = "name";
            double price = 20.0;
            int amountOfCopies = 10;
            int amountOfBooksToCreate = 3;

            for (int i = 0; i < amountOfBooksToCreate; i++)
            {
                var book = new Book($"{isbn}{i}", name, price, amountOfCopies);
                _bookService.Create(book);
            }

            //Act
            var allBooks = _bookService.GetAll();

            //Assert
            allBooks.Count.Should().Be(amountOfBooksToCreate);
        }
    }
}