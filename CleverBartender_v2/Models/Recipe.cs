
namespace CleverBartender_v2.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int DrinkId { get; set; }
        public int IngredientId { get; set; }

        // Nav props
        //public Drink Drinks { get; set; }
        //public Ingredient Ingredients { get; set; }
        
    }
}
