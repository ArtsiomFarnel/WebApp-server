using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.Data.Entities
{
    public class Product : IEntityBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name is required")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Field Cost is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Cost is required and it can't be lower than 0")]
        public float Cost { get; set; }
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
