using CAPS.Global;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CAPS.Global.GlobalConstants;

namespace CAPS.DataModels
{
    public class PawnItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = IsRequiredMsg)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = IsRequiredMsg)]
        public decimal Value { get; set; }

        [Required]
        public ItemCategory Category { get; set; }


        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime PawnDate { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime DueDate { get; set; }

        public bool IsDeleted { get; set; }

        public PawnStatus Status { get; set; }

        public int PawnShopId { get; set; }

        [ForeignKey(nameof(PawnShopId))]
        public PawnShop PawnShop { get; set; }

        [ForeignKey(nameof(AppUserId))]
        public string AppUserId { get; set; }

        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        public List<Payment> Payments { get; set; } = new List<Payment>();

    }
}
