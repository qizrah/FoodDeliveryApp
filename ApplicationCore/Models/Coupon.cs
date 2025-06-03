using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public decimal DiscountAmount { get; set; } // e.g., $5 off

        public decimal? MinimumOrderAmount { get; set; } // e.g., $25 min

        public int? MaxUses { get; set; } // optional: max number of times this can be used
        public int Uses { get; set; } // how many times it has been used

        public DateTime? ExpirationDate { get; set; }

        public DayOfWeek? ValidDayOfWeek { get; set; } // optional: limit to specific day

        public string? ApplicationUserId { get; set; } // for user-specific coupons

        public bool IsOneTimeUse { get; set; } = false; // default is false for general coupons

        public bool IsActive { get; set; } = true; // default active

        public bool IsUsed { get; set; } = false; // only true for one-time user coupons
    }
}
