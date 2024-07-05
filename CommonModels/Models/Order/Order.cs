using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OrderService.Models;
using CommonModels.Models.Order;

namespace CommonModels.Models.Order
{
    public class Order : BaseModel
    {
        public ICollection<OrderRow> Rows { get; set; } = new List<OrderRow>();

        [Required]
        public int Client_Id { get; set; }

        [NotMapped]
        public Client? Client { get; set; }

        [Required]
        public int Products_List_Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? Status_Name { get; set; }

        [Required]
        public DateTime Creation_Date { get; set; }

        [Required]
        public DateTime Update_Date { get; set; }


        public static ValidationResult ValidateStatusName(string statusName, ValidationContext context)
        {
            var allowedStatuses = new List<string> { "New", "Processing", "Completed", "Cancelled" };
            if (!allowedStatuses.Contains(statusName))
            {
                return new ValidationResult($"Invalid status name. Allowed statuses are: {string.Join(", ", allowedStatuses)}");
            }
            return ValidationResult.Success;
        }
    }

}