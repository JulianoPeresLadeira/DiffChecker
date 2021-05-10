using System;
using DiffChecker.Model;

namespace DiffChecker.Errors
{
    public class DataDecodingException : Exception, IDiffServiceException
    {
        public string EncodedData { get; set; }

        public DataDecodingException(string encodedData) : base()
        {
            EncodedData = encodedData;
        }

        public ErrorResponse ToErrorResponse()
        {
            return new ErrorResponse
            {
                StatusCode = 500,
                Message = $"Error converting {EncodedData} from base64."
            };
        }
    }
}