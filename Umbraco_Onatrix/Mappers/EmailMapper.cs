using Umbraco_Onatrix.Managers;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Mappers
{
    public static class EmailMapper
    {
        public static EmailRequestModel CreateServiceEmail(ServiceQuestionFormModel form, string htmlBodyMessage)
        {
            if (form != null)
            {
                var email = new EmailRequestModel
                {
                    To = form.Email,
                    Subject = "Service question",
                    HtmlBody = htmlBodyMessage,
                    PlainText = "We have recived your message, and we will get back to you as soon as possible! :)"
                };

                return email;
            }

            return null!;
        }
        public static EmailRequestModel CreateSupportEmail(SupportFormModel form, string htmlBodyMessage)
        {
            if (form != null)
            {
                var email = new EmailRequestModel
                {
                    To = form.Email,
                    Subject = "Support",
                    HtmlBody = htmlBodyMessage,
                    PlainText = "We have recived your message, and we will get back to you as soon as possible! :)"
                };

                return email;
            }

            return null!;
        }
        public static EmailRequestModel CreateContactEmail(ContactFormModel form, string htmlBodyMessage)
        {
            if (form != null)
            {
                var email = new EmailRequestModel
                {
                    To = form.Email,
                    Subject = "Contact",
                    HtmlBody = htmlBodyMessage,
                    PlainText = $"We will give you a call on {form.PhoneNumber} as soon as we can!:)"
                };

                return email;
            }

            return null!;
        }
    }
}
