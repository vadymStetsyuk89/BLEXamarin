using StBox.Views.Controls.Validator.Contracts;

namespace StBox.Views.Controls.Validator.ValidationRules
{
    public class DigitRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
                return false;

            bool hasDigit = false;

            string validationString = value as string;

            foreach (char character in validationString)
            {
                if (char.IsDigit(character))
                {
                    hasDigit = true;
                }
                else
                {
                    return false;
                }
            }

            return hasDigit;
        }
    }
}
