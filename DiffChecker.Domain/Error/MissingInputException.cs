using System;
using DiffChecker.Domain.Model;

namespace DiffChecker.Domain.Error
{
    public class MissingInputException : Exception, IDiffServiceException
    {
        public MissingInputException(string message) : base(message)
        {
        }

        public ErrorResponse ToErrorResponse()
        {
            return new ErrorResponse
            {
                StatusCode = 400,
                Message = Message
            };
        }
    }
}