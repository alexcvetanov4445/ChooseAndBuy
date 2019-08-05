namespace ChooseAndBuy.Web.BindingModels.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class DropdownRequired : ValidationAttribute
    {
        private const string DefaultSelectValue = "0";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isDefaultOption = value.ToString();

            if (isDefaultOption == DefaultSelectValue)
            {
                return new ValidationResult("Please select an option.");
            }

            return ValidationResult.Success;
        }
    }
}
