//------------------------------------------------------------------------------
// <copyright file="Injector.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Infrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Ninject;

    /// <summary>
    /// Defines the <see cref="Injector" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Injector
    {
        /// <summary>
        /// Defines the _kernel.
        /// </summary>
        private static IKernel? kernel;

        /// <summary>
        /// Gets the Kernel.
        /// </summary>
        public static IKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    throw new ArgumentNullException("Injection method should be called first!");
                }

                return kernel;
            }
        }

        /// <summary>
        /// The Inject.
        /// </summary>
        public static void Inject()
        {
            kernel = new StandardKernel(new Bindings());
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <typeparam name="T">Generic T class.</typeparam>
        /// <returns>The generic <see cref="T"/>.</returns>
        public static T Get<T>()
        {
            return kernel.Get<T>();
        }
    }
}
