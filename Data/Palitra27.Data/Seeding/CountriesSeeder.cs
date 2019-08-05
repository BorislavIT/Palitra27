namespace Palitra27.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Palitra27.Common;
    using Palitra27.Data.Models;

    public class CountriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            string[] names = new string[]
      {
  "Bulgaria",
  "Macedonia",
  "Serbia",
  "Greece",
  "Romania",
      };
            var countries = new List<Country>();

            foreach (var item in names)
            {
                var country = new Country { Name = item };
                countries.Add(country);
            }

            await dbContext.AddRangeAsync(countries);
        }
    }
}
