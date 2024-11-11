using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
namespace CAPS.ViewModels
{
    public class PawnShopsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(PawnShopNameMaxLenght, ErrorMessage = PawnShopNameMaxLenghtErrorMsg)]
        [MinLength(PawnShopNameMinLenght, ErrorMessage = PawnShopNameMinLenghtErrorMsg)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(PawnShopCityMaxLenght, ErrorMessage = PawnShopCityMaxLenghtErrorMsg)]
        [MinLength(PawnShopCityMinLenght, ErrorMessage = PawnShopCityMinLenghtErrorMsg)]
        public string City { get; set; } = null !;

        public string? LocationUrl { get; set; }
        public List<PawnedItemViewModel> PawnedItems { get; set; } = new List<PawnedItemViewModel>();
    }
}
