using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumDishesRequirement : IAuthorizationRequirement
    {
        public int MinimumDishes { get; }

        public MinimumDishesRequirement(int minimumDishes)
        {
            MinimumDishes = minimumDishes;
        }
    }
}
