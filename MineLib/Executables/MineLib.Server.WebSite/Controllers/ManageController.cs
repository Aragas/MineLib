using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Models;
using MineLib.Server.Heartbeat.Models.ManageViewModels;
using MineLib.Server.Heartbeat.Services;

using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Controllers
{
    [Authorize]
    public sealed class ManageController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ILogger<ManageController> logger, UrlEncoder urlEncoder)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _urlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(urlEncoder));
        }

        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            /*
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";
            */

            var user = await GetCurrentUserAsync();
            var model = new IndexViewModel
            {
                Username = await _userManager.GetUserNameAsync(user),
                Email = await _userManager.GetEmailAsync(user)
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var email = user.Email;
                    if (model.Email != email)
                    {
                        var result = await _userManager.SetEmailAsync(user, model.Email);
                        if (!result.Succeeded)
                            AddErrors(result);
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View("Error");
            }
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    return View();
                }
                return RedirectToAction(nameof(SetPassword));
            }
            return View("Error");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("{Type}: '{Email}' changed their password successfully.", GetType().FullName, user.Email);
                        return RedirectToAction(nameof(ChangePassword));
                    }

                    AddErrors(result);
                    return View(model);
                }
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
            }
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    return RedirectToAction(nameof(ChangePassword));
                }

                return View();
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(SetPassword));
                    }

                    AddErrors(result);
                    return View(model);
                }
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return View(new TwoFactorAuthenticationViewModel
                {
                    HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                    Is2FAEnabled = user.TwoFactorEnabled,
                    RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
                });
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> DisableAuthenticatorWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TwoFactorEnabled == true)
            {
                return View(nameof(DisableAuthenticator));
            }
            return View("Error");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
                    return RedirectToAction(nameof(TwoFactorAuthentication));
                }

                AddErrors(result);
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var model = new EnableAuthenticatorViewModel();
                await LoadSharedKeyAndQrCodeUriAsync(user, model);

                return View(model);
            }
            return View("Error");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    // Strip spaces and hypens
                    var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

                    var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
                    if (is2faTokenValid)
                    {
                        await _userManager.SetTwoFactorEnabledAsync(user, true);
                        _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
                        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                        TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

                        return RedirectToAction(nameof(ShowRecoveryCodes));
                    }

                    ModelState.AddModelError("Code", "Verification code is invalid.");
                    await LoadSharedKeyAndQrCodeUriAsync(user, model);
                    return View(model);
                }
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            if (TempData[RecoveryCodesKey] is string[] recoveryCodes)
                return View(new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes });

            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return View(nameof(ResetAuthenticator));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _userManager.ResetAuthenticatorKeyAsync(user);
                _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

                return RedirectToAction(nameof(EnableAuthenticator));
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TwoFactorEnabled == true)
            {
                return View(nameof(GenerateRecoveryCodes));
            }
            return View("Error");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

                var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

                return View(nameof(ShowRecoveryCodes), model);
            }
            return View("Error");
        }

        /*
        // POST: /Manage/ResetAuthenticatorKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                _logger.LogInformation(1, "User reset authenticator key.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/GenerateRecoveryCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCode()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);
                _logger.LogInformation(1, "User generated new recovery code.");
                return View("DisplayRecoveryCodes", new DisplayRecoveryCodesViewModel { Codes = codes });
            }
            return View("Error");
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }
        */

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey, currentPosition, 4).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey, currentPosition, unformattedKey.Length - currentPosition);
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey) => string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("MineLib Project"),
                _urlEncoder.Encode(email),
                unformattedKey);

        private async Task LoadSharedKeyAndQrCodeUriAsync(User user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        #endregion
    }
}