namespace Stn.FitBook.Domain.Exceptions;
public class BaseException : Exception
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public BaseException(string message) : base(message) { }
}
