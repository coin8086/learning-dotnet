//See https://weblogs.asp.net/ricardoperes/net-8-data-annotations-validation

using System.ComponentModel.DataAnnotations;

namespace DataValidation;

class Program
{
    static void TryValidateObject(object user)
    {
        //The variable results as parameter to TryValidateObject is optional. It can be null.
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(user, new ValidationContext(user), results, true))
        {
            Console.WriteLine($"Invalid {user}");

            //Check each returned ValidationResult
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
        else
        {
            Console.WriteLine($"Valid {user}");
        }
    }

    static void ValidateObject(object user)
    {
        try
        {
            Validator.ValidateObject(user, new ValidationContext(user));
            Console.WriteLine($"Valid {user}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Invalid {user}");

            //Note that there's only one ValidationResult is returned by the exception.
            Console.WriteLine(ex.ValidationResult);
        }
    }

    static void Main(string[] args)
    {
        var user = new User()
        {
            Email = "xxx"
        };
        TryValidateObject(user);

        Console.WriteLine("-----------------");

        ValidateObject(user);
    }
}
