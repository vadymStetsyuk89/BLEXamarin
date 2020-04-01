using StBox.Views.Controls.Validator.Contracts;

namespace StBox.Views.Controls.Validator.ValidationRules
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public static readonly string FIELD_IS_REQUIRED_ERROR_MESSAGE = "This field is required";

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if ((value is string))
            {
                var validatedValue = value as string;

                return (!string.IsNullOrWhiteSpace(validatedValue) && !string.IsNullOrEmpty(validatedValue));
            }
            else
            {
                return value != null;
            }
        }
    }
}
