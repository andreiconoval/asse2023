//------------------------------------------------------------------------------
// <copyright file="LibrarySettingsRepository.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------


namespace Library.DAL.Repositories
{
    using Library.DAL.DataMapper;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the <see cref="LibrarySettingsRepository" />.
    /// </summary>

    [ExcludeFromCodeCoverage]
    public class LibrarySettingsRepository : ILibrarySettingsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarySettingsRepository"/> class.
        /// </summary>
        public LibrarySettingsRepository()
        {
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <returns>The <see cref="LibrarySettings"/>.</returns>
        public LibrarySettings Get()
        {
            using (var ctx = new LibraryDbContext())
            {
                return ctx.Set<LibrarySettings>().Find(1);
            }
        }
    }
}
