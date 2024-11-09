using CAPS.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.DataModels
{
    public class PawnItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Value { get; set; }


        [Required]
        public DateTime PawnDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

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
