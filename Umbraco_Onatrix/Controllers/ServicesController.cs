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
using Umbraco_Onatrix.Managers;
using Umbraco_Onatrix.Mappers;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Controllers
{
    public class ServicesController : SurfaceController
    {
        private readonly ServicebusRequestManager _servicebusRequestManager;
        private readonly FormManager _formManager;
        public ServicesController(FormManager formManager, ServicebusRequestManager servicebusRequestManager, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _servicebusRequestManager = servicebusRequestManager;
            _formManager = formManager;
        }

        public async Task<IActionResult> SubmitHandler(ServiceQuestionFormModel form)
        {
            var errorList = new Dictionary<string, bool>
            {
                {"error_name", _formManager.ValidateRegEx("^[\\p{L}\\p{M}\\'\\-\\. ]+$", form.Name) },
                { "error_email", _formManager.ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email) },
                { "error_message", !string.IsNullOrEmpty(form.Message) }
            };

            if (errorList.ContainsValue(false))
            {
                foreach (var keyValue in errorList)
                {
                    if (keyValue.Value.Equals(false))
                        TempData[$"{keyValue.Key}"] = true;
                }

                TempData["name"] = form.Name; TempData["email"] = form.Email; TempData["message"] = form.Message;
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
            }

            string htmlBodyMessage = _formManager.CreateServiceHtmlMessage("Service question", form.Name, form.Message);
            bool result = await _servicebusRequestManager.SendEmailAsync(EmailMapper.CreateServiceEmail(form, htmlBodyMessage), "email_request");
            if (!result)
            {
                TempData["form-success"] = "fail";
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
            }

            TempData["form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
        }
    }
}
