using System.ComponentModel.DataAnnotations;

namespace CustomValidator;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class RecursiveValidationAttribute : ValidationAttribute
{
    public string? DisplayName { get; set; }

    public override bool IsValid(object? value)
    {
        return IsValid(value, null) == ValidationResult.Success;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext? context)
    {
        if (value != null && context != null)
        {
            var displayName = DisplayName ?? context.DisplayName;
            var ctx = new ValidationContext(value) { DisplayName = displayName };
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(value, ctx, results, false))
            {
                return new ValidationResult($"{displayName}: {results[0].ErrorMessage}", [displayName]);
            }
        }
        return ValidationResult.Success;
    }
}
