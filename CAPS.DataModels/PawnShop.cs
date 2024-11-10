using System.ComponentModel.DataAnnotations;

namespace CAPS.DataModels
{
    public class PawnShop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;
        public string? LocationUrl { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<PawnItem> PawnedItems { get; set; } = new List<PawnItem>();
    }
}
