using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Library.DAL.Repositories;
using log4net.Core;
using Microsoft.Extensions.Logging;
using Ninject.Modules;

namespace Library.BL.Infrastructure
{
    [ExcludeFromCodeCoverage]
    class Bindings: NinjectModule
    {
        public override void Load()
        {
            // Here I should have a switch to change impl (Mocks / Real)

            LoadRepositoryLayer();
            LoadServicesLayer();
        }

        private void LoadServicesLayer()
        {
            Bind<IAuthorService>().To<AuthorService>();
            Bind<IBookAuthorService>().To<BookAuthorService>();
            Bind<IBookDomainService>().To<BookDomainService>();
            Bind<IBookEditionService>().To<BookEditionService>();
        }

        private void LoadRepositoryLayer()
        {
            Bind<IAuthorRepository>().To<AuthorRepository>();
        }
    }
}
