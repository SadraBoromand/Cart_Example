using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cart_Example.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDeatilId { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Count { get; set; }


        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
