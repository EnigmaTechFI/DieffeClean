using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Model.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DieffeClean.WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly SignInManager<MyUser> _signInManager;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public AuthController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
    public async Task<IActionResult> ForgotPassword(string Email)
    {
        try
        {
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
        return View(new ResetPasswordViewModel()
        {
            Token = token,
            Email = email
        });
    }
}