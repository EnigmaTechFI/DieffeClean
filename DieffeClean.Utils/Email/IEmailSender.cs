namespace DieffeClean.Utils.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message, string templatePATH, List<(string, string)> replacer);
    }
}
