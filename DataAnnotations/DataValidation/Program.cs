//See https://weblogs.asp.net/ricardoperes/net-8-data-annotations-validation

using System.ComponentModel.DataAnnotations;

namespace DataValidation;

class Program
{
    static void TryValidateUser(User user)
    {
        //The variable results as parameter to TryValidateObject is optional. It can be null.
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(user, new ValidationContext(user), results, true))
        {
            Console.WriteLine($"Invalid {user}");
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

    static void ValidateUser(User user)
    {
        try
        {
            Validator.ValidateObject(user, new ValidationContext(user));
            Console.WriteLine($"Valid {user}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Invalid {user}");
            Console.WriteLine(ex.ValidationResult);
        }
    }

    static void Main(string[] args)
    {
        var user = new User()
        {
            Email = "xxx"
        };
        TryValidateUser(user);

        Console.WriteLine("-----------------");

        ValidateUser(user);
    }
}
