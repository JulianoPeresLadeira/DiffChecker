using System;
using DiffChecker.Domain.Model;

namespace DiffChecker.Domain.Error
{
    public class PersistenceException : Exception, IDiffServiceException
    {
        public PersistenceException() : base()
        {
        }

        public ErrorResponse ToErrorResponse()
        {
            return new ErrorResponse
            {
                StatusCode = 500,
                Message = "Failure Persisting diff data"
            };
        }
    }
}