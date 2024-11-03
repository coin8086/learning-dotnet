using System.ComponentModel.DataAnnotations;

namespace CustomValidator;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DescriptionAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return IsValid(value, null) == ValidationResult.Success;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext? context)
    {
        var str = value as string;
        if (str == null)
        {
            return new ValidationResult("Description is null.", [context?.MemberName ?? string.Empty]);
        }
        if (str.Length < 50)
        {
            return new ValidationResult("Description length is less than 50.", [context?.MemberName ?? string.Empty]);
        }

        return ValidationResult.Success;
    }
}
