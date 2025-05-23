using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public double OrderTotal { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime DeliveryTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeliveryName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }

        [MaxLength(100)]
        public string? TransactionId { get; set; }
        public string PaymentStatus { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public string? CouponCode { get; set; }
        [NotMapped]
        public double DiscountAmount { get; set; } = 0;
    }

}
