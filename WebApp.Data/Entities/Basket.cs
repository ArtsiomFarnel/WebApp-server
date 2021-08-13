using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Data.Entities
{
    public class Basket : IEntityBase
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
