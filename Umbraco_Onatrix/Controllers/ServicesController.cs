using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections;
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


            var email = new EmailRequestModel
            {
                To = form.Email,
                Subject = "Question",
                HtmlBody = $@"
                             <!DOCTYPE>
                               <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport'´content='width=device-width, initial-scale=1.0'>
                                    <title>Your Question</title>
                                </head>
                                <body>
                                        
                                        <div style='font-size:50px; color:red;'>Hello {form.Name}!</div>
                                        <div style='font-size:30px; color:red;'>We have recied your message</div>
                                </body>
                               </html>                       
                          ",
                PlainText = "We have recived your message"
            };

            await using var client = new ServiceBusClient("Endpoint=sb://onatrix-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=O37OuVCIncd1pkD0drttiwGJo7G5lqe7s+ASbMICBnE=");
            ServiceBusSender sender = client.CreateSender("email_request");
            var json = JsonConvert.SerializeObject(email);
            ServiceBusMessage message = new ServiceBusMessage(json) { ContentType = "application/json" };
            try
            {
                await sender.SendMessageAsync(message);
            }
            catch (Exception ex) { Debug.WriteLine($"ERROR :: Sending to email queue {ex.Message}"); }
            



            TempData["form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
        }
    }
}
