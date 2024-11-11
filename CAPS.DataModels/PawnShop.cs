using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
namespace CAPS.DataModels
{
    public class PawnShop
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(PawnShopNameMaxLenght, ErrorMessage = PawnShopNameMaxLenghtErrorMsg)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(PawnShopCityMaxLenght, ErrorMessage =PawnShopCityMaxLenghtErrorMsg)]
        public string City { get; set; } = null!;
        public string? LocationUrl { get; set; }
        public bool IsDeleted { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<PawnItem> PawnedItems { get; set; } = new List<PawnItem>();
    }
}
