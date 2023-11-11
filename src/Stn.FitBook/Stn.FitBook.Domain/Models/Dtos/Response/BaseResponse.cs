using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Models.Dtos.Response
{
    public class BaseResponse
    {       
        public string ResponseCode { get; set; } ="00";
        public string ResponseMessage { get; set; } = "Success";
    }
}
