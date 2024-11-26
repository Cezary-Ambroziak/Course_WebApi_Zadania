using System;
using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Entities;
using Bogus.Extensions;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants1 = GetRestaurants1();
                    _dbContext.Restaurants.AddRange(restaurants1);
                    _dbContext.SaveChanges();
                }
                if (_dbContext.Dishes.Any())
                {
                    var dishes50 = GetDish();
                    _dbContext.Dishes.AddRange(dishes50);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                Name = "Manager"
            },
                new Role()
                {
                    Name = "Admin"
                },
            };

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description =
                        "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Price = 10.30M,
                        },

                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 5.30M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald Szewska",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Szewska 2",
                        PostalCode = "30-001"
                    }
                }
            };

            return restaurants;
        }

        private IEnumerable<Restaurant> GetRestaurants1()
        {
            var locale = "pl";
            Randomizer.Seed = new Random(1);

            var addressGenerator = new Faker<Address>(locale)
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());


            var restaurantGenerator = new Faker<Restaurant>(locale)
                .RuleFor(r => r.Name, f => f.Company.CompanyName().ClampLength(1,15))
                .RuleFor(r => r.Description, f => f.Random.String2(200, "abcde fghijkl mnopq rstu vwxyz żźćń łąśęó "))
                .RuleFor(r => r.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(r => r.HasDelivery, f => f.Random.Bool())
                .RuleFor(r => r.ContactEmail, f => f.Internet.Email())
                .RuleFor(r => r.ContactNumber, f => f.Phone.PhoneNumber("###-###-###"))
                .RuleFor(r => r.CreatedById, f => f.Random.ArrayElement(new[] { 1, 2, 3,4,5,6,7,8,9 }))
                .RuleFor(r => r.Address, f => addressGenerator.Generate());
            var restaurants = restaurantGenerator.Generate(50);

            return restaurants;
        }

        private IEnumerable<Dish> GetDish()
        {
            var locale = "pl";
            Randomizer.Seed = new Random(1);

            var dishGenerator = new Faker<Dish>(locale)
                .RuleFor(r => r.Name, f => f.Company.CompanyName().ClampLength(1, 30))
                .RuleFor(r => r.Description, f => f.Random.String2(200, "abcde fghijkl mnopq rstu vwxyz żźćń łąśęó "))
                .RuleFor(r => r.Price, f => f.Finance.Amount())
                .RuleFor(r => r.RestaurantId, f => f.Random.ArrayElement(new[] { 1, 2, 3, 1003, 1005, 1006, 1007, 1008, 1009 }));

            var dishes = dishGenerator.Generate(100);

            return dishes;
        }
    }
}
