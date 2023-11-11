using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public string RefreshToken1 { get; set; }
        public string Email { get; set; }
    }
}
