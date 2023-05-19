using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Model.Auth;
using DieffeClean.Utils.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NToastNotify;

namespace DieffeClean.WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly SignInManager<MyUser> _signInManager;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IEmailSender _emailSender;
    private readonly IToastNotification _toastNotification;

    public AuthController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, IEmailSender emailSender, IToastNotification toastNotification)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _toastNotification = toastNotification;
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
                _toastNotification.AddErrorToastMessage("Email o password non corrette.");    
                return View(model);
            }
            
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Calendar", "Reservation");
            
            _toastNotification.AddErrorToastMessage("Email o password non corrette.");    
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
                _toastNotification.AddSuccessToastMessage("Se l'email inserita è presente nei nostri database, verrà inviata una mail per il ripristino della password.");    
                return RedirectToAction("Login");
            }
 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _toastNotification.AddSuccessToastMessage("Se l'email inserita è presente nei nostri database, verrà inviata una mail per il ripristino della password.");    
                return RedirectToAction("Login");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var returnUrl = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new (string, string)[] { ("Noreply | Swirkey", email) }, "Resetta la tua password", returnUrl);
            List<(string, string)> replacer = new List<(string, string)> { ("[LinkResetPassword]", returnUrl) };
            var currentPath = Directory.GetCurrentDirectory();
            await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-reset-password.html", replacer);
            
            _toastNotification.AddSuccessToastMessage("Se l'email inserita è presente nei nostri database, verrà inviata una mail per il ripristino della password.");    
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
            _toastNotification.AddErrorToastMessage("Parametri mancanti.");    
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
            _toastNotification.AddErrorToastMessage("La mail inserita non è presente nei nostri database.");    
            return View(model);
        }
            
        var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if(!resetResult.Succeeded)
        {
            foreach (var error in resetResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            _toastNotification.AddErrorToastMessage(resetResult.Errors.FirstOrDefault().Description);    
            return View(model);
        }
        _toastNotification.AddSuccessToastMessage("Password cambiata correttamente.");
        return RedirectToAction("Login");
    }
}