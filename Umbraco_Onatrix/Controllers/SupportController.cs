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
    public class SupportController : SurfaceController
    {
        private readonly ServicebusRequestManager _servicebusRequestManager;
        private readonly FormManager _formManager;

        public SupportController(ServicebusRequestManager servicebusRequestManager, FormManager formManager, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) :
            base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _formManager = formManager;
            _servicebusRequestManager = servicebusRequestManager;
        }

        public async Task<IActionResult> SubmitHandler(SupportFormModel form)
        {
            var emailResult = _formManager.ValidateRegEx("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", form.Email);

            if (emailResult == false)
            {
                TempData["support_error_email"] = true; 
                TempData["support_email"] = form.Email;
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#side-menu-card");
            }

            bool result = await _servicebusRequestManager.SendEmailAsync(EmailMapper.CreateSupportEmail(form, _formManager.CreateSupportHtmlMessage("Onatrix Support")), "email_request");
            if (!result)
            {
                TempData["support_form-success"] = "fail";
                return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#side-menu-card");
            }

            TempData["support_form-success"] = "success";
            return Redirect(UmbracoContext.OriginalRequestUrl.ToString() + "#side-menu-card");
        }
    }
}
