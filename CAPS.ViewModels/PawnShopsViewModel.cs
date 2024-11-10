using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.ViewModels
{
    public class PawnShopsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public string LocationUrl { get; set; }
        public List<PawnedItemViewModel> PawnedItems { get; set; } = new List<PawnedItemViewModel>();
    }
}
