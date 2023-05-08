using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Model.Auth;
using DieffeClean.Utils.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DieffeClean.WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly SignInManager<MyUser> _signInManager;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IEmailSender _emailSender;
    public AuthController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            MyUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return View(model);
            }
            
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        catch(Exception ex)
        {
            Logger.Error(ex.Message, ex);
            return View(model);
        }
    }
    
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                //_notyfToastService.Error("Email non valida.");
                return RedirectToAction("Login");
            }
 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                //_notyfToastService.Error("Nessun account trovato con questa email.");
                return RedirectToAction("Login");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var returnUrl = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new (string, string)[] { ("Noreply | Swirkey", email) }, "Resetta la tua password", returnUrl);
            List<(string, string)> replacer = new List<(string, string)> { ("[LinkResetPassword]", returnUrl) };
            var currentPath = Directory.GetCurrentDirectory();
            await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-reset-password.html", replacer);
            
            //_notyfToastService.Success($"Mail inviata correttamente! (a { email })");
            return RedirectToAction("Login");
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Login");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        try {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        catch(Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("Login");
        }
    }
    
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            Logger.Error($"Richiesta reset password (GET), nessun utente trovato con email: {email}");
            //_notyfToastService.Error($"Nessun utente trovato con email: {email}");
            return RedirectToAction("Login");
        }
        
        return View(new ResetPasswordViewModel()
        {
            Token = token,
            Email = email
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
            
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            Logger.Error($"Richiesta reset password (POST), nessun utente trovato con email: {model.Email}");
            //_notyfToastService.Error($"Nessun utente trovato con email: {model.Email}");
            return View(model);
        }
            
        var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if(!resetResult.Succeeded)
        {
            foreach (var error in resetResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return View(model);
        }
        return RedirectToAction("Login");
    }
}