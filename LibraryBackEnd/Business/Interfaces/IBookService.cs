using System;
using System.Collections.Generic;
using Domain;

namespace Business.Interfaces
{
    public interface IBookService
    {
        Book Create(Book book);
        List<Book> GetAll();
        Book Get(Guid id);
        int GetAmountAvailableBooks(Guid id);
    }
}