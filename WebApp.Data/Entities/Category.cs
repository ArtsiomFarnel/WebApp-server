using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.Data.Entities
{
    public class Category : IEntityBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name is required")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
