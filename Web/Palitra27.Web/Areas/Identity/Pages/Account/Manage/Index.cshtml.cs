﻿namespace Palitra27.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Palitra27.Data.Models;
    using Palitra27.Services.Data;

#pragma warning disable SA1649 // File name should match first type name
    public class IndexModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.userService = userService;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var userName = await this.userManager.GetUserNameAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var email = await this.userManager.GetEmailAsync(user);
            if (this.Input.Email != email)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, this.Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (this.Input.FirstName != user.FirstName)
            {
                this.userService.EditFirstName(user, this.Input.FirstName);
            }

            if (this.Input.LastName != user.LastName)
            {
                this.userService.EditLastName(user, this.Input.LastName);
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var userId = await this.userManager.GetUserIdAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: this.Request.Scheme);
            await this.emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.StatusMessage = "Verification email sent. Please check your email.";
            return this.RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            [StringLength(12, MinimumLength = 7, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
            public string PhoneNumber { get; set; }

            [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
            public string FirstName { get; set; }

            [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
            public string LastName { get; set; }
        }
    }
}
