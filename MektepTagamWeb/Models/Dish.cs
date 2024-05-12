using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamWeb.Models
{
    public class Dish
    {
        public Guid Id { get; set; }
        [Display(Name = "Название блюда")]
        [Required]
        public string? Name { get; set; }
        [Display(Name = "Цена блюда")]
        [Required]
        public double? Price { get; set; }
        [Display(Name = "Описание блюда")]
        [Required]
        public string? Description { get; set; }
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<Transaction> Transactions { get; set; }
        public Dish()
        {
            Transactions = new List<Transaction>();
        }
    }
}
