
using System.Collections.Generic;

namespace CleverBartender_v2.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // nav props
        //public List<Recipe> Recipes { get; set; }
    }
}
