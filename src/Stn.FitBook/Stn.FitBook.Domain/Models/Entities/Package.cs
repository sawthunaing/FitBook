using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class Package
    {
        public Package()
        {
            UserPackages = new HashSet<UserPackage>();
        }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Country { get; set; }
        public int? Credits { get; set; }
        public decimal? Price { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<UserPackage> UserPackages { get; set; }

        public static implicit operator List<object>(Package v)
        {
            throw new NotImplementedException();
        }
    }
}
