using DiffChecker.Model;

namespace DiffChecker.Errors
{
    public interface IDiffServiceException
    {
        public ErrorResponse ToErrorResponse();
    }
}