using System.ComponentModel.DataAnnotations;

namespace Umbraco_Onatrix.Models
{
    public class ServiceQuestionFormModel
    {
        public string Name { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
