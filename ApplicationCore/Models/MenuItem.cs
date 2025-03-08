using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Menu Item")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than 1$")]
        public float price { get; set; }

        public int CategoryId { get; set; }
        public int FoodTypeId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        // Additional optional property for the uploaded file
        [NotMapped]
        public IFormFile? UploadedFile { get; set; }

        //Virtual objects
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; }
    }
}
