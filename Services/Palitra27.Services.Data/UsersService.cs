namespace Palitra27.Services.Data
{
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;

    public class UsersService : IUsersService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public UsersService(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public ApplicationUserDTO FindUserByUsername(string username)
        {
            var user = this.FindDomainUserByUsername(username);

            return this.mapper.Map<ApplicationUserDTO>(user);
        }

        public void EditFirstName(ApplicationUser user, string firstName)
        {
            if (user == null)
            {
                return;
            }

            user.FirstName = firstName;
            this.dbContext.SaveChanges();
        }

        public void EditLastName(ApplicationUser user, string lastName)
        {
            if (user == null)
            {
                return;
            }

            user.LastName = lastName;
            this.dbContext.SaveChanges();
        }

        private ApplicationUser FindDomainUserByUsername(string username)
        {
            var user = this.dbContext.Users
                .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}