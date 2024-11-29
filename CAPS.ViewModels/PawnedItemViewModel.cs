using CAPS.Global;


namespace CAPS.ViewModels
{
    public class PawnedItemViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Value { get; set; }

        public string Description { get; set; }

        public ItemCategory Category { get; set; }
    }
}
