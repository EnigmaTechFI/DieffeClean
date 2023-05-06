namespace DieffeClean.Presentation.Model.Auth;

public class ResetPasswordViewModel
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string OldPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}