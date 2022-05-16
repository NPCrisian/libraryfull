using System;
using System.Collections.Generic;
using Domain;

namespace Persistence.Interfaces
{
    public interface IBookRepository
    {
        void Create(Book book);
        Book Read(Guid id);
        Book ReadByIsbn(string isbn); //can be id too
        List<Book> ReadAll();
    }
}