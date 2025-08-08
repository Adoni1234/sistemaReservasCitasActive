
﻿using SistemaReservasCitas.Application.Interfaces;
﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SistemaReservasCitasApi.Application.Interfaces;
using System.Threading.Tasks;


namespace SistemaReservasCitasApi.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            // Constructor vacío para inyección de dependencias
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Simulación: en producción, usa SMTP (por ejemplo, MailKit)
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("El correo de destino no puede ser nulo o vacío.", nameof(to));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("El asunto no puede ser nulo o vacío.", nameof(subject));
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("El cuerpo del mensaje no puede ser nulo o vacío.", nameof(body));

            var emailConfig = _configuration.GetSection("EmailSettings");
            var email = emailConfig["Email"];
            var password = emailConfig["Password"];
            var host = emailConfig["Host"];
            var port = int.Parse(emailConfig["Port"]);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(email));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h2>{subject}</h2><p>{body}</p>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(email, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
 } 


