using System.ComponentModel.DataAnnotations;

namespace MektepTagamWeb.ViewModels
{
    public class TransactionCreateModel
    {
        [Required]
        [StringLength(12, ErrorMessage = "ИИН должен быть длиной 12 символов")]
        public string IndividualNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше нуля")]
        public double Amount { get; set; }
    }
}
