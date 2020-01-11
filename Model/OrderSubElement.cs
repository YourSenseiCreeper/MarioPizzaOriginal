using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrderSubElement
    {
        [Index]
        [AutoIncrement]
        public int SubOrderElementId { get; set; }
        [Required]
        public int OrderElementId { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
