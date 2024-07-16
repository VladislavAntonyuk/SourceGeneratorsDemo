using FluentValidation;
using FluentValidation.Validators;

namespace CreditCardValidation;

public static class CreditCardValidationService
{
    public static List<ValidationResult> ValidateCreditCards(List<BaseModel> models)
    {
        var results = new List<ValidationResult>();
        var creditCardValidator = new CreditCardValidator<string>();
        for (var index = 0; index < models.Count; index++)
        {
            var model = models[index];
            var properties = model.GetType().GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ValidateCreditCardAttribute)))
                .ToList();
            foreach (var property in properties)
            {
                var value = property.GetValue(model)?.ToString();

                if (value == null)
                {
                    continue;
                }

                if (creditCardValidator.IsValid(new ValidationContext<string>(value), value))
                {
                    results.Add(new ValidationResult(index + 1, property.Name.ToUpperInvariant(), value));
                }
            }
        }

        return results;
    }

    public static List<ValidationResult> SourceGeneratorValidateCreditCards(List<BaseModel> models)
    {
        var results = new List<ValidationResult>();
        var creditCardValidator = new CreditCardValidator<string>();
        for (var index = 0; index < models.Count; index++)
        {
            foreach (var property in models[index].GetPropertiesForCreditCardValidation())
            {
                var value = property.Value;

                if (value == null)
                {
                    continue;
                }

                if (creditCardValidator.IsValid(new ValidationContext<string>(value), value))
                {
                    results.Add(new ValidationResult(index + 1, property.Property, value));
                }
            }
        }

        return results;
    }
}