using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Models.Dtos.Response
{
    public class ErrorDetail
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
