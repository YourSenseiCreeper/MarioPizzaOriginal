using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FoodIngredient
    {
        [Index]
        [AutoIncrement]
        public int FoodIngredientId { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        public int IngredientId { get; set; }
        [Required]
        public double IngredientAmount { get; set; }
    }
}
