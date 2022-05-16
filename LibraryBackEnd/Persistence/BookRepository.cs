using System;
using System.Collections.Generic;
using Domain;
using Persistence.Interfaces;
using System.Linq;

namespace Persistence
{
    public class BookRepository : IBookRepository
    {
        private List<Book> _books;
        
        public BookRepository()
        {
            _books = new List<Book>();
        }


        public void Create(Book book)
        {
            _books.Add(book);
        }

        public Book Read(Guid id)
        {
            return _books.SingleOrDefault(x => x.Id == id);
        }

        public Book ReadByIsbn(string isbn)
        {
            return _books.SingleOrDefault(x => x.Isbn.ToLower() == isbn.ToLower());
        }

        public List<Book> ReadAll()
        {
            return _books;
        }
    }
}