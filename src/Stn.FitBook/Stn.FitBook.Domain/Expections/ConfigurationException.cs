using Stn.FitBook.Domain.Constants;

namespace Stn.FitBook.Domain.Exceptions;
public class ConfigurationException : BaseException
{
    public ConfigurationException(string message) : base(message)
    {
        ErrorCode = ErrorCodes.ConfigurationException;
        ErrorMessage = message;
    }
}
