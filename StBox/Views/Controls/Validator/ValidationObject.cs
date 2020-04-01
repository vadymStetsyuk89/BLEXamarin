using StBox.Environment;
using StBox.Views.Controls.Validator.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace StBox.Views.Controls.Validator
{
    /// <summary>
    /// How to use: 
    /// In your VM define ValidationObject<T> property and init it -
    /// DateOfBirth = _validationObjectFactory.GetValidatableObject<DateTime>();
    /// DateOfBirth.Validations.Add(new TomorrowDateLimitRule<DateTime>() { ValidationMessage = string.Format(_DATE_LIMIT_VALUE_ERROR_MESSAGE, DateTime.Now) });
    /// DateOfBirth.Value = DateTime.Now;
    /// DateOfBirth.PropertyChanged += OnDateOfBirthValuePropertyChanged; - not necessary, helps to track input changes
    /// 
    /// In your XAML - 
    /// <StackLayout Spacing="3">
    ///     <Label Style = "{StaticResource Key=Input_group_title_label}" Text="Date of Birth" />
    ///     <controls:ExtendedContentView BorderColor = "{Binding Path=DateOfBirth.IsValid, Converter={StaticResource Key=Bool_to_entry_wraper_error_border_color_converter}}" 
    ///         Style="{StaticResource Key=Input_group_entry_wraper}">
    ///         <controls:ExtendedDatePicker Date = "{Binding Path=DateOfBirth.Value, Mode=TwoWay}" MaximumDate="{x:Static system:DateTime.Today}" />
    ///     </controls:ExtendedContentView>
    ///     <Label IsVisible = "{Binding Path=DateOfBirth.IsValid, Converter={StaticResource Key=Reverce_bool_converter}}"
    ///         Style="{StaticResource Key=ValidationErrorLabelStyle}"
    ///         Text="{Binding Path=DateOfBirth.Errors, Converter={StaticResource FirstValidationErrorConverter}}" />
    /// </StackLayout>
    /// </summary>
    public class ValidationObject<T> : ExtendedBindableObject, IValidity
    {
        public static readonly string VALUE_PROPERTY_NAME = "Value";

        private readonly List<IValidationRule<T>> _validations;

        public ValidationObject()
        {
            _isValid = true;
            _errors = new List<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public List<IValidationRule<T>> Validations => _validations;

        private List<string> _errors;
        public List<string> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }

        private T _value;
        public T Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
