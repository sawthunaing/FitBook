using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class UserPackage
    {
        public int UserPackageId { get; set; }
        public int? UserId { get; set; }
        public int? AvailableCredits { get; set; }
        public int? PackageId { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public virtual Package Package { get; set; }
        public virtual User User { get; set; }
    }
}
