using System.ComponentModel.DataAnnotations;

namespace Umbraco_Onatrix.Models
{
    public class ServiceQuestionFormModel
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
