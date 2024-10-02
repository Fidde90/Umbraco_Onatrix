using System.Text.RegularExpressions;

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

        public string CreateServiceHtmlMessage(string subject, string recieverName, string? userMessage)
        {
            if(!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(recieverName))
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
                                        <div style='font-size:50px; color:black;'>Hello {recieverName}!</div>
                                        <div style='font-size:30px; color:lack;'>We have recived your message, and we will get back to you ass soon as possible! :)</div>
                                        <p style='color:darkgreen; font-size:20px;'>Best regards Onatrix team.<p>
                                        <br>
                                        <br>
                                        <hr>
                                        <p>Your question: {userMessage}</p>
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
                                        <div style='font-size:30px; color:lack;'>We have recived your message, and we will get back to you ass soon as possible!:) </div>
                                        <p style='color:darkgreen; font-size:20px;'>Best regards Onatrix team.<p>
                                </body>
                               </html>                       
                          ";

                return bodyMessage;
        }
    }
}
