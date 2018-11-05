using Microsoft.EntityFrameworkCore;

namespace CleverBartender_v2.Models
{
    public class IngredientContext : DbContext
    {
        public IngredientContext(DbContextOptions<IngredientContext> options)
           : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
