using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;


namespace RestaurantAPI.Authorization
{
    public class MinimumDishesRequirementHandler : AuthorizationHandler<MinimumDishesRequirement>
    {
        private readonly RestaurantDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public MinimumDishesRequirementHandler(RestaurantDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _contextAccessor = accessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumDishesRequirement requirement)
        {
            var createdDishesCount = _context.Dishes.GroupBy(d => d.RestaurantId).Count();

            string restaurantId = _contextAccessor.HttpContext.Request.PathBase;

            if (createdDishesCount >= requirement.MinimumDishes)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
