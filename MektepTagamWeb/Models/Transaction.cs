using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamWeb.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public double? Amount { get; set; }
        [ForeignKey("CardCodeId")]
        public Guid? CardCodeId { get; set; }
        public CardCode? CardCode { get; set; }
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DateOfCreatedTransaction { get; set; } = DateTime.Now;
        [ForeignKey("DishId")]
        public Guid? DishId { get; set; }
        public Dish? Dish { get; set; }
    }
}
