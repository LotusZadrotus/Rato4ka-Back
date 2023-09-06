using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rato4ka_back.Util;

namespace Rato4ka_back.Services.impl
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public MailService(ILogger<MailService> logger, IConfiguration configuration, IWebHostEnvironment env)
        {
            _logger = logger;
            _configuration = configuration;
            _env = env;
        }
        public void SentMail(string email, MailSubject subject, string login, string key)
        {
            try
            {
                var info = _env.IsDevelopment() ? _configuration.GetSection("Mail.Develop") : _configuration.GetSection("Mail");

                var client = new SmtpClient(info["Host"], Convert.ToInt32(info["Port"]))
                {
                    Credentials = new NetworkCredential(info["mail"], info["password"]),
                    EnableSsl = Convert.ToBoolean(info["EnableSSL"])
                };
                var msg = new MailMessage
                {
                    From = new MailAddress(info["mail"]), Subject = "Confirm your registration", IsBodyHtml = true
                };
                msg.To.Add(email);
                switch (subject)
                {
                    case MailSubject.Registration:
                        var body = ReadTemplate(info["TemplatePath"]);
                        body = body.Replace("{UserName}", login);
                        body = body.Replace("{Url}", $"{info["ApiURL"]}{login}?key={key}");
                        msg.Body = body;
                        break;
                    case MailSubject.ForgotPassword:
                        break;
                }
                client.Send(msg);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Can't sent a mail: {e.Message}");
                throw new Exception(e.Message, e);
            }
        }
        protected string ReadTemplate(string path)
        {
            var toReturn = string.Empty;
            using (var streamReader = new StreamReader(path))
            {
                toReturn = streamReader.ReadToEnd();
            }
            return toReturn;
        }
    }
}