using StBox.Views.Controls.Validator.Contracts;
using System.Text.RegularExpressions;

namespace StBox.Views.Controls.Validator.ValidationRules
{
    public class EmailRule<T> : IValidationRule<T>
    {
        public static readonly string INVALID_EMAIL_ERROR_MESSAGE = "Invalid email";
        private static readonly string REGEX_VALIDATION = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
                return false;

            string validatedValue = value as string;

            Regex regex = new Regex(REGEX_VALIDATION);
            Match match = regex.Match(validatedValue);

            return match.Success;
        }
    }
}
