﻿using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookEditionService : BaseService<BookEdition, IBookEditionRepository>, IBookEditionService
    {
        public BookEditionService(IBookEditionRepository repository, ILogger logger) : base(repository, new BookEditionValidator(), logger)
        {
        }
    }
}
