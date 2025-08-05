using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitasApi.Application.Interfaces;

namespace SistemaReservasCitas.Application.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
            // Constructor vacío para inyección de dependencias
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            // Simulación: en producción, usa SMTP (por ejemplo, MailKit)
            Console.WriteLine($"Enviando email a {to}: {subject} - {body}");
            return Task.CompletedTask;
        }
    }
}