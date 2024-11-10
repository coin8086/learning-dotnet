using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CustomValidator;

public class User : IValidatableObject
{
    [Required]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    //Note: Either attribute is OK for validation. Here both are used for demo purpose.
    //[Description]
    //[CustomValidation(typeof(User), nameof(User.ValidDescription))]
    public string? Description { get; set; }

    [Required]
    [RecursiveValidation]
    public Address Address { get; set; } = default!;

    /*
     * NOTE
     * 
     * The method must be public static and take a input parameter for the object to be validated.
     * It can take an optional parameter of type ValidationContext. See more at
     * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.customvalidationattribute.method?view=net-8.0
     */
    public static ValidationResult? ValidDescription(string description /*, [optional] ValidationContext context */)
    {
        if (description == null || description.Length < 50)
        {
            return new ValidationResult("Description length is at least 50.");
        }
        return ValidationResult.Success;
    }

    //The method will be called AFTER individual properties are validated and PASSED.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Email) && !Email.StartsWith(Name!, StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("Email must start with the Name.", [nameof(Email), nameof(Name)]);
        }
    }

    public override string ToString()
    {
        var opts = new JsonSerializerOptions() { WriteIndented = true };
        var json = JsonSerializer.Serialize(this, opts);
        return json;
    }
}
