using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Controllers
{
    public class ServicesController : SurfaceController
    {
        public ServicesController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {

        }

        private static bool ValidateRegEx(string regEx, string valueToValidate)
        {
            if (regEx != null && valueToValidate != null)
            {
                Regex? RegEx = new(regEx!);

                if (RegEx.IsMatch(valueToValidate!))
                    return true;
            }
            return false;
        }

        public async Task<IActionResult> SubmitHandler(ServiceQuestionFormModel form)
        {
            var errorList = new Dictionary<string, bool>
            {
                { "error_name", ValidateRegEx("^[\\p{L}\\p{M}\\'\\-\\. ]+$", form.Name) },
                { "error_email", ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email) },
                { "error_message", !string.IsNullOrEmpty(form.Message) }
            };

            if (errorList.ContainsValue(false))
            {
                foreach (var keyValue in errorList)
                {
                    if (keyValue.Value.Equals(false))
                    {
                        TempData[$"{keyValue.Key}"] = true;
                    }
                }

                TempData["name"] = form.Name;
                TempData["email"] = form.Email;
                TempData["message"] = form.Message;

                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
            }


            using (var email = new MailMessage())
            {
                email.From = new MailAddress("onatrixteam@domain.com");
                email.To.Add(form.Email);
                email.Subject = "Message from us";
                string message = "We have recived your message and we will come back to you as soon as possible.";
                email.Body = $@"
                             <!DOCTYPE>
                               <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport'´content='width=device-width, initial-scale=1.0'>
                                    <title>{email.Subject}</title>
                                </head>
                                <body>
                                        <div style='font-size:50px; color:red;'>{message}</div>
                                </body>
                               </html>                       
                          ";

                email.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(); 
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    try
                    {
                        await smtpClient.SendMailAsync(email);
                    }
                    catch (Exception ex) { Debug.WriteLine("Error Sending email SMTP:" + ex.Message); }
                }              
            }
            
            TempData["form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
        }
    }
}
