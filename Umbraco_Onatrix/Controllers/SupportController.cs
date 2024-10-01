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
    public class SupportController : SurfaceController
    {
        public SupportController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {

        }

        public IActionResult SubmitHandler(SupportFormModel form)
        {
            var emailResult = ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email);

            if (emailResult == false)
            {
                TempData["support_error_email"] = true;
                TempData["support_email"] = form.Email;
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString());
            }

            //gör massa kul

            TempData["support_form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString());
        }


        //fixa till en service
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
