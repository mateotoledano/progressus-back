using Microsoft.Extensions.Options;
using ProgressusWebApi.Utilities;
using System.Net.Mail;
using System.Net;
using ProgressusWebApi.Services.EmailServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProgressusWebApi.Services.EmailServices
{
	public class EmailSenderService : IEmailSenderService
	{
		private readonly GmailSetter _mailSetter;
		public EmailSenderService(IOptions<GmailSetter> mailSetter)
		{
			_mailSetter = mailSetter.Value;
		}

		public async Task<IActionResult> SendEmail(string subject, string body, string to)
		{
			try
			{
				var fromEmail = _mailSetter.Email;
				var password = _mailSetter.Password;

				// Define the HTML template
				string htmlTemplate = @"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Account Verification</title>
  <style>
    body {{ font-family: Arial, sans-serif; background-color: #f8f9f8; margin: 0; padding: 0; }}
    .email-container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; }}
    .email-header {{ background-color: #80b918; color: #ffffff; padding: 20px; text-align: center; }}
    .email-header h1 {{ margin: 0; font-size: 22px; }}
    .email-body {{ padding: 20px; text-align: center; color: #333333; }}
    .email-body p {{ margin: 16px 0; font-size: 16px; }}
    .verification-code {{ display: inline-block; font-size: 28px; font-weight: bold; color: #80b918; padding: 10px 20px; border: 1px solid #80b918; border-radius: 5px; background-color: #f4f9f4; }}
    .email-footer {{ background-color: #f8f9f8; color: #666666; text-align: center; font-size: 12px; padding: 10px; }}
    .email-footer a {{ color: #80b918; text-decoration: none; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h1>Progressus Gym</h1>
    </div>
    <div class='email-body'>
      <p>Hola,</p>
      <p>¡Gracias por registrarte en <b>Progressus Gym</b>!</p>
      <p>Para <b>verificar tu cuenta</b>, utiliza <b>el código</b> que aparece a continuación:</p>
      <div class='verification-code'>{0}</div>
      <p>Si no solicitó esto, puede ignorar este correo electrónico con seguridad.</p>
    </div>
    <div class='email-footer'>
      <p>© 2024 Progressus Gym. All rights reserved.</p>
      <p>
        <a href='#'>Privacy Policy</a> |
        <a href='#'>Terms of Service</a>
      </p>
    </div>
  </div>
</body>
</html>";


				// Format the HTML template with the body variable (the 4-digit code)
				string emailBody = string.Format(htmlTemplate, body);

				var message = new MailMessage();
				message.From = new MailAddress(fromEmail);
				message.Subject = subject;
				message.Body = emailBody;
				message.To.Add(new MailAddress(to));
				message.IsBodyHtml = true;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587, // You can use _mailSetter.Port if defined
					Credentials = new NetworkCredential(fromEmail, password),
					EnableSsl = true,
				};

				smtpClient.Send(message);

				return new OkObjectResult(HttpStatusCode.OK);
			} catch (Exception e)
			{
				throw new Exception("No se pudo enviar el email", e);
			}
		}
	}
}
