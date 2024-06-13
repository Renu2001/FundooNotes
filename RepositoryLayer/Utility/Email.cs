using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Utility;
namespace RepositoryLayer.Utility
{
    public class Email
    {
        private readonly FundooContext fundooContext;
        private readonly Token _token;
        public Email(FundooContext fundooContext, Token token)
        {
            this.fundooContext = fundooContext;
            this._token = token;
        }

        public string SendMail(string emailId)
        {
            string token;
            var result = fundooContext.Users.FirstOrDefault(x => x.email == emailId);
            if (result != null)
                token = _token.GenerateToken(result);
            else
                throw new CustomizeException("Invalid Email");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("anonymous.u2003@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailId));
            email.Subject = "Password Reset Request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                        <html>
                        <head>
                        </head>
                        <body>
                          <div class='container'>
                            <div class='header'>
                              <h2>Password Reset Request</h2>
                            </div>
                            <div class='content'>
                              <p>Hello,</p>
                              <p>We received a request to reset the password for your account. If you did not make this request, you can ignore this email.</p>
                              <p>To reset your password, please click the button below:</p>
                              <p><a href = {"http://localhost:5284/api/Users/ResetPassword"}>http://localhost:5284/api/Users/ResetPassword+{token}</a></p>
                              <p>Thank you,<br/>The FundooNotes Team</p>
                            </div>
                          </div>
                        </body>
                        </html>"
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("anonymous.u2003@gmail.com", "dopb jonq ldin cozg");
            smtp.Send(email);
            smtp.Disconnect(true);
            return token;
        }
    }
}
