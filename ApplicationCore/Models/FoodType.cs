﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class FoodType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "FoodType Name")]
        public string Name { get; set; }

        [NotMapped]

        public ICollection<MenuItemFoodType> MenuItemFoodTypes { get; set; }
    }
}
