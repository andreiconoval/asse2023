//------------------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Serilog;

    /// <summary>
    /// Defines the <see cref="LoggerExtensions" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggerExtensions
    {
        /// <summary>
        /// The LoggingInstance.
        /// </summary>
        /// <typeparam name="T">T Class.</typeparam>
        /// <returns>The <see cref="ILogger{T}"/>.</returns>
        public static ILogger<T> LoggingInstance<T>()
        {
            Log.Logger = Log.Logger ?? new LoggerConfiguration().WriteTo.Console().CreateLogger();
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// The TestLoggingInstance.
        /// </summary>
        /// <typeparam name="T">T Class.</typeparam>
        /// <returns>The <see cref="ILogger{T}"/>.</returns>
        public static ILogger<T> TestLoggingInstance<T>()
        {
            Log.Logger = Log.Logger ?? new LoggerConfiguration().CreateLogger();
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }
    }
}
