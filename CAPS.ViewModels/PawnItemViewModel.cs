

using CAPS.Global;

namespace CAPS.ViewModels
{
    public class PawnItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalValue { get; set; }
        public decimal ValueWithTax { get; set; }  // Value with 20% added
        public PawnStatus Status { get; set; }
    }
}
