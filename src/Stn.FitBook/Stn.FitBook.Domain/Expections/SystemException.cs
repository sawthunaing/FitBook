using Stn.FitBook.Domain.Constants;

namespace Stn.FitBook.Domain.Exceptions;
public class SystemException : BaseException
{
    public SystemException(string message) : base(message)
    {
        ErrorCode = ErrorCodes.UnexpectedException;
        ErrorMessage = message;
    }
}
