using System;
using DiffChecker.Model;

namespace DiffChecker.Errors
{
    public class DiffProcessingException : Exception, IDiffServiceException
    {
        public DiffProcessingException(string message) : base(message)
        {
        }

        public ErrorResponse ToErrorResponse()
        {
            return new ErrorResponse
            {
                StatusCode = 500,
                Message = Message
            };
        }
    }
}