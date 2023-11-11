using Stn.FitBook.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Exceptions
{
    public class AmountNotMatchException : BaseException
    {
        public AmountNotMatchException(string message) : base(message)
        {
            ErrorCode = ErrorCodes.AmountNotMatchException;
            ErrorMessage = message;
        }
    }
}
