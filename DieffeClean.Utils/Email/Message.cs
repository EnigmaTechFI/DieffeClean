using MimeKit;

namespace DieffeClean.Utils.Email
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public List<MailboxAddress> Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<(string Name, string Address)> to, string subject, string content,
            IEnumerable<(string Name, string Address)> cc = null)
        {
            To = new List<MailboxAddress>();
            Cc = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.Name, x.Address)));
            if(cc != null && cc.Any())
                Cc.AddRange(cc.Select(x => new MailboxAddress(x.Name, x.Address)));
            Subject = subject;
            Content = content;
        }
    }
}
