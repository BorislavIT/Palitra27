namespace Palitra27.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]
        public void FindUserByUsernameShouldReturnUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindUserByUsernameShouldReturnUser_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "1";

            var user = mapper.Map<ApplicationUserDTO>(dbContext.Users.FirstOrDefault(x => x.UserName == username));
            var userFromService = usersService.FindUserByUsername(username);

            Assert.IsType<ApplicationUserDTO>(userFromService);
            Assert.Equal(user.UserName, userFromService.UserName);
        }

        [Fact]
        public void FindUserByUsernameShouldReturnNullIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindUserByUsernameShouldReturnNullIfInvalidUsername_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "totallyLegitUserName";

            var userFromService = usersService.FindUserByUsername(username);

            Assert.Null(userFromService);
        }

        [Fact]
        public void EditFirstNameShouldEditFirstName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditFirstNameShouldEditFirstName_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "1";

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
            usersService.EditFirstName(user, "firstName" );

            Assert.Equal("firstName", user.FirstName);
        }

        [Fact]
        public void EditFirstNameShouldntEditFirstNameIfInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditFirstNameShouldntEditFirstNameIfInvalidUser_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "1";

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
            var userToEdit = dbContext.Users.FirstOrDefault(x => x.UserName == "totallyLegitUsername");

            usersService.EditFirstName(userToEdit, "firstName");

            Assert.Null(user.FirstName);
        }

        [Fact]
        public void EditLastNameShouldEditLasttName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditLastNameShouldEditLastName_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "1";

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
            usersService.EditLastName(user, "lastName");

            Assert.Equal("lastName", user.LastName);
        }

        [Fact]
        public void EditLastNameShouldntEditLastNameIfInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditLastNameShouldntEditLastNameIfInvalidUser_Users_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();
            var usersService = new UsersService(dbContext, mapper);

            this.SeedDbWithUsers(dbContext);
            var username = "1";

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
            var userToEdit = dbContext.Users.FirstOrDefault(x => x.UserName == "totallyLegitUsername");

            usersService.EditLastName(userToEdit, "lastName");

            Assert.Null(user.LastName);
        }

        private void SeedDbWithUsers(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 5; i++)
            {
                dbContext.Users.Add(new ApplicationUser { UserName = $"{i}" });
                dbContext.SaveChanges();
            }
        }

        private IMapper SetUpAutoMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            return mockMapper.CreateMapper();
        }
    }
}
