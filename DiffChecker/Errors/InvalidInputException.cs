using System;
using DiffChecker.Model;

namespace DiffChecker.Errors
{
    public class InvalidInputException : Exception, IDiffServiceException
    {
        public InvalidInputException(string message) : base(message)
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