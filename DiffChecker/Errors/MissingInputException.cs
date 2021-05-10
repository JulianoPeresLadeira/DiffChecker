using System;
using DiffChecker.Model;

namespace DiffChecker.Errors
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