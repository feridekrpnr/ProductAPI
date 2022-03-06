using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DitenProductAPI.MyEntities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }

        public bool IsDeleted { get; set; }

        public int Quantity { get; set; }

    }
}
