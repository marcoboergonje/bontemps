﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BonTemps.Data;
using BonTemps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BonTemps.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Klant> _signInManager;
        private readonly UserManager<Klant> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Klant> userManager,
            SignInManager<Klant> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Er zijn nog geen klantgegevens ingevuld.")]
            public int KlantGegevensId { get; set; }
            [Required(ErrorMessage = "U heeft nog geen voornaam ingevuld.")]
            public string Voornaam { get; set; }
            [Required(ErrorMessage = "U heeft nog geen achternaam ingevuld.")]
            public string Achternaam { get; set; }
            [Required(ErrorMessage = "U heeft nog geen geboortedatum ingevuld.")]
            public DateTime GeboorteDatum { get; set; }
            [Required(ErrorMessage = "U heeft nog geen geslacht ingevuld.")]
            public string Geslacht { get; set; }
            [Required(ErrorMessage = "U heeft nog geen telefoonnummer ingevuld.")]
            public string TelefoonNummer { get; set; }

            [Required(ErrorMessage = "U heeft nog geen e-mail adres ingevuld.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "U heeft nog geen wachtwoord ingevuld.")]
            [StringLength(100, ErrorMessage = "Het wachtwoord moet minimaal 6 tekens bevatten, en maximaal 100.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new Klant { UserName = Input.Email, Email = Input.Email };
                var rol = "Klant";
                user.Rolnaam = rol;
                user.Klantgegevens = new Klantgegevens
                {
                    Voornaam = Input.Voornaam,
                    Achternaam = Input.Achternaam,
                    GeboorteDatum = Input.GeboorteDatum,
                    Geslacht = Input.Geslacht,
                    TelefoonNummer = Input.TelefoonNummer
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user,rol);
                if (result.Succeeded)
                {
                   
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
