//------------------------------------------------------------------------------
// <copyright file="Bindings.cs" company="Your Company">
// Copyright (c) Your Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.Interfaces;
    using Library.DAL.Repositories;
    using Ninject.Modules;

    /// <summary>
    /// Bindings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Bindings : NinjectModule
    {
        /// <summary>
        /// Load method.
        /// </summary>
        public override void Load()
        {
            this.LoadRepositoryLayer();
            this.LoadServicesLayer();
        }

        /// <summary>
        /// Load service layer.
        /// </summary>
        private void LoadServicesLayer()
        {
            Bind<IAuthorService>().To<AuthorService>();
            Bind<IBookAuthorService>().To<BookAuthorService>();
            Bind<IBookDomainService>().To<BookDomainService>();
            Bind<IBookEditionService>().To<BookEditionService>();
        }

        /// <summary>
        /// Load repository layer.
        /// </summary>
        private void LoadRepositoryLayer()
        {
            Bind<IAuthorRepository>().To<AuthorRepository>();
        }
    }
}
