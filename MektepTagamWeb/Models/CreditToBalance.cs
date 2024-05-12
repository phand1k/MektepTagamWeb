using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamWeb.Models
{
    public abstract class CreditToBalance
    {
        [MaxLength(12)]
        [Display(Name = "ИИН ученика")]
        [Required(ErrorMessage = "ИИН ученика обязательно для заполнения")]
        public string? IndividualNumberPerson { get; set; }
        [Display(Name = "Сумма для зачисления")]
        [Required(ErrorMessage = "Сумма для зачисления обязательна")]
        public double? Ammount { get; set; }
    }
}
