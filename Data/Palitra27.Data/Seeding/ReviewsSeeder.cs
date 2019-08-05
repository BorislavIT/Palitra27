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

    public class ReviewsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Reviews.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);
            var reviewList = new List<Review>();
            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var review = new Review() { Message = "Very good", Stars = random.Next(1, 5), UserName = admin.UserName, DateOfCreation = DateTime.UtcNow };
                reviewList.Add(review);
            }

            await dbContext.Reviews.AddRangeAsync(reviewList);
        }
    }
}
