using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Constants
{
    public static class ErrorCodes
    {
        public static readonly string NoError = "00";
        public static readonly string Fail = "01";
        public static readonly string Processing = "02";
        public static readonly string ArgumentException = "03";
        public static readonly string ArgumentMissingException = "04";
        public static readonly string ConfigurationException = "05";
        public static readonly string InsufficientCreditException = "08";
        public static readonly string AmountNotMatchException = "09";
        public static readonly string UnexpectedException = "99";
    }

}
