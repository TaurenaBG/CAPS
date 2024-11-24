

using CAPS.DataModels;
using CAPS.Global;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CAPS.ViewModels
{
    public class ItemViewModel
    {


        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public ItemCategory Category { get; set; }

        [Required]
        public DateTime PawnDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        public List<SelectListItem> PawnShops { get; set; } = new List<SelectListItem>();

       
        public int PawnShopId { get; set; }


    }
}
