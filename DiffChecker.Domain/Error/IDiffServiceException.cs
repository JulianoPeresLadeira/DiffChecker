using DiffChecker.Domain.Model;

namespace DiffChecker.Domain.Error
{
    public interface IDiffServiceException
    {
        public ErrorResponse ToErrorResponse();
    }
}