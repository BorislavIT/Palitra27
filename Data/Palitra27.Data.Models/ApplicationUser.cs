// ReSharper disable VirtualMemberCallInConstructor
namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Palitra27.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string FirstName { get; set; }

        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public string FavouriteListId { get; set; }
        public virtual FavouriteList FavouriteList { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
