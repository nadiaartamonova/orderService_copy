using CommonModels.Models.Products;
using OrderService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModels.Models.Order
{
    public class OrderRow : BaseModel
    {
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public Product? Product { get; set; }
    }
}
