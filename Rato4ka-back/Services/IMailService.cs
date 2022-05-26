using Rato4ka_back.Util;

namespace Rato4ka_back.Services
{
    public interface IMailService
    {
        public void SentMail(string email, MailSubject subject, string login, string key);
    }
}