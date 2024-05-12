using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamWeb.Models
{
    public class CardCode
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Код карточки")]
        public string? Code { get; set; }
        public bool? IsDeleted { get; set; } = false;
        [Display(Name = "Дата создания")]
        public DateTime? DateOfCreated { get; set; } = DateTime.Now;
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        [ForeignKey("AspNetUserId")]
        [Display(Name = "Ученик")]
        public string? AspNetUserId { get; set; }
        public AspNetUser? AspNetUser { get; set; }
        public CardCode()
        {

        }
    }
}
