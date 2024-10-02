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
                    PlainText = "We have recived your message, and we will get back to you ass soon as possible! :)"
                };

                return email;
            }

            return null!;
        }
    }
}
