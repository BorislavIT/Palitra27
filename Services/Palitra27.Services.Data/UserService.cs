namespace Palitra27.Services.Data
{
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;

    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db, IMapper mapper)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public ApplicationUserDTO GetUserByUsername(string username)
        {
            return this.mapper.Map<ApplicationUserDTO>(this.db.Users
                .FirstOrDefault(x => x.UserName == username));
        }

        public void EditFirstName(ApplicationUser user, string firstName)
        {
            if (user == null)
            {
                return;
            }

            user.FirstName = firstName;
            this.db.SaveChanges();
        }

        public void EditLastName(ApplicationUser user, string lastName)
        {
            if (user == null)
            {
                return;
            }

            user.LastName = lastName;
            this.db.SaveChanges();
        }

        public ApplicationUser GetDomainUserByUsername(string username)
        {
            return this.db.Users
                 .FirstOrDefault(x => x.UserName == username);
        }
    }
}