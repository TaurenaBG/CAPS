using CAPS.Global;
using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CAPS.ViewModels
{
    public class ItemViewModel
    {


        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(ItemNameMaxLenght,ErrorMessage = ItemNameNotInRangeErrorMsg)]
        [MinLength(ItemNameMinLenght, ErrorMessage = ItemNameNotInRangeErrorMsg)]
        public string Name { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [MaxLength(ItemDescriptionMaxLenght, ErrorMessage = ItemDescriptionLenghtNotInRangeErrorMsg)]
        [MinLength(ItemDescriptionMinLenght, ErrorMessage = ItemDescriptionLenghtNotInRangeErrorMsg)]
        public string Description { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
        public decimal Value { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public ItemCategory Category { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime PawnDate { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime DueDate { get; set; }

        public List<SelectListItem> PawnShops { get; set; } = new List<SelectListItem>();

        public int PawnShopId { get; set; }


    }
}
