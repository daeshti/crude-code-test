using System;

namespace CrudTest.Application.Common.Exceptions
{
    /**
     * An exception thrown when resource is not found for a query.
     * Can semantically map to an HTTP 404 error.
     */
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}