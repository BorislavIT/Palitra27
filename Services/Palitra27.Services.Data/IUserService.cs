namespace Palitra27.Services.Data
{
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;

    public interface IUserService
    {
        ApplicationUserDTO FindUserByUsername(string username);

        void EditFirstName(ApplicationUser user, string firstName);

        void EditLastName(ApplicationUser user, string lastName);
    }
}
