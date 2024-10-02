using Microsoft.AspNetCore.Mvc;
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
    public class ContactController : SurfaceController
    {
        private readonly ServicebusRequestManager _servicebusRequestManager;
        private readonly FormManager _formManager;

        public ContactController(ServicebusRequestManager servicebusRequestManager, FormManager formManager, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _servicebusRequestManager = servicebusRequestManager;
            _formManager = formManager;
        }

        public async Task<IActionResult> SubmitHandler(ContactFormModel form)
        {
            Dictionary<string, bool> errorList = new Dictionary<string, bool>
            {
                {"contact_name_error", _formManager.ValidateRegEx("^[\\p{L}\\p{M}\\'\\-\\. ]+$",form.Name) },
                {"contact_phone_error", _formManager.ValidateRegEx("^(?:\\+46|0)7[0-9]{8}$", form.PhoneNumber) },
                {"contact_email_error", _formManager.ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email) },
                {"contact_department_error",!string.IsNullOrEmpty(form.Department) }
            };

            if (errorList.ContainsValue(false))
            {
                foreach (var keyValue in errorList)
                {
                    if (keyValue.Value.Equals(false))
                        TempData[$"{keyValue.Key}"] = keyValue.Value;
                }

                TempData["contact_name"] = form.Name; TempData["contact_phone"] = form.PhoneNumber; TempData["contact_email"] = form.Email; TempData["contact_department"] = form.Department;
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#contact-form");
            }

            bool result = await _servicebusRequestManager.SendEmailAsync(EmailMapper.CreateContactEmail(form, _formManager.CreateContactHtmlMessage(form, "Contact")), "email_request");
            if (!result)
            {
                TempData["form-success"] = "fail";
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#contact-form");
            }

            System.Timers.Timer aTimer = new System.Timers.Timer();

            aTimer.Start();

            aTimer.Interval = 3000;

            aTimer.Stop();
            TempData["form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#contact-form");

   

        }
    }
}
