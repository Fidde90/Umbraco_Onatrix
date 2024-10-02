using Microsoft.AspNetCore.Mvc;
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
    public class ContactController : SurfaceController
    {
        public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {

        }

        public IActionResult SubmitHandler(ContactFormModel form)
        {
            Dictionary<string, bool> errorList = new Dictionary<string, bool>
            {
                {"contact_name_error", ValidateRegEx("^[\\p{L}\\p{M}\\'\\-\\. ]+$",form.Name) },
                {"contact_phone_error", ValidateRegEx("^(?:\\+46|0)7[0-9]{8}$", form.PhoneNumber) },
                {"contact_email_error", ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email) },
                {"contact_department_error",!string.IsNullOrEmpty(form.Department) }
            };

            if (errorList.ContainsValue(false))
            {
                foreach (var keyValue in errorList)
                {
                    if (keyValue.Value.Equals(false))
                    {
                        TempData[$"{keyValue.Key}"] = keyValue.Value;
                    }
                }

                TempData["contact_name"] = form.Name;
                TempData["contact_phone"] = form.PhoneNumber;
                TempData["contact_email"] = form.Email;
                TempData["contact_department"] = form.Department;

                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#contact-form");
            }

            TempData["form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#contact-form");

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
    }
}
