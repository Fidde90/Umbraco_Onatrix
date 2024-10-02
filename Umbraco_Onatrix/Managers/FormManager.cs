using System.Text.RegularExpressions;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Managers
{
    public class FormManager
    {

        public bool ValidateRegEx(string regEx, string valueToValidate)
        {
            if (regEx != null && valueToValidate != null)
            {
                Regex? RegEx = new(regEx!);

                if (RegEx.IsMatch(valueToValidate!))
                    return true;
            }
            return false;
        }

        public string CreateServiceHtmlMessage(ServiceQuestionFormModel form, string? subject)
        {
            if (form != null)
            {
                string bodyMessage = $@"
                             <!DOCTYPE>
                               <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport'´content='width=device-width, initial-scale=1.0'>
                                    <title>{subject}</title>
                                </head>
                                <body style='background-color:lightgray'>                                        
                                        <h1 style='font-size:50px; color:black;'>Hello {form.Name}!</h1>
                                        <p style='font-size:20px; color:lack;'>We have recived your message, and we will get back to you as soon as possible! :)</p>
                                        <p style='color:darkgreen; font-size:20px;'>Best regards Onatrix team.<p>
                                        <br>
                                        <br>
                                        <hr>
                                        <p>Your question: {form.Message}</p>
                                </body>
                               </html>                       
                          ";

                return bodyMessage;
            }

            return "";
        }

        public string CreateSupportHtmlMessage(string? subject)
        {
            string bodyMessage = $@"
                             <!DOCTYPE>
                               <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport'´content='width=device-width, initial-scale=1.0'>
                                    <title>{subject}</title>
                                </head>
                                <body style='background-color:lightgray'>                                        
                                        <p style='font-size:30px; color:lack;'>We have recived your message, and we will get back to you as soon as possible!:) </p>
                                        <p style='color:darkgreen; font-size:20px;'>Best regards Onatrix team.<p>
                                </body>
                               </html>                       
                          ";

            return bodyMessage;
        }
        public string CreateContactHtmlMessage(ContactFormModel form, string? subject)
        {
            if (form != null)
            {
                string bodyMessage = $@"
                             <!DOCTYPE>
                               <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport'´content='width=device-width, initial-scale=1.0'>
                                    <title>{subject}</title>
                                </head>
                                <body style='background-color:lightgray'>    
                                        <h1 style='font-size:50px; color:lack;' >Hello {form.Name}!</h1>
                                        <p style='font-size:20px; color:lack;'>We will give you a call on {form.PhoneNumber} as soon as we can!:) </p>
                                        <p style='color:darkgreen; font-size:20px;'>Best regards Onatrix team / { form.Department}.<p>
                                </body>
                               </html>                       
                          ";

                return bodyMessage;
            }

            return "";
        }
    }
}
