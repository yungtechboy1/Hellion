using System;

namespace Hellion.Database.Exceptions
{
    public class DatabaseNotInitializedException : Exception
    {
        private const string ErrorMessage = "Database service has not been initialized. Please call the DatabaseService.InitializeDatabase method.";

        /// <summary>
        /// Creates a new <see cref="DatabaseNotInitializedException"/>.
        /// </summary>
        public DatabaseNotInitializedException()
            : base(ErrorMessage)
        {
        }
    }
}
