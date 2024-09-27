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
    public class ServicesController : SurfaceController
    {
        public ServicesController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : 
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {

        }

        public IActionResult SubmitHandler(ServiceQuestionFormModel form)
        {
            //    Regex emailRegEx = new("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
            //    Regex nameRegEx = new("^[\\p{L}\\p{M}\\'\\-\\. ]+$");

            //    Match emailResult = emailRegEx.Match(form.Email);
            //    Match nameResult = nameRegEx.Match(form.Name);

            if (!ModelState.IsValid)
            {

                TempData["error_name"] = "fail";
                TempData["error_email"] = "fail";
                TempData["error_message"] = "fail";

                TempData["name"] = form.Name;
                TempData["email"] = form.Email;
                TempData["message"] = form.Message;

                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
            }
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#service-form");
        }
    }
}
