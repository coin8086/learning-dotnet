//See https://weblogs.asp.net/ricardoperes/net-8-data-annotations-validation

using System.ComponentModel.DataAnnotations;

namespace CustomValidator;

class Program
{
    static void TryValidateObject(object user)
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

    static void Main(string[] args)
    {
        var user = new User()
        {
            Name = "Rob",
            Email = "rob@xxx",
            Description = "Some description",
            Address = new Address()
        };
        TryValidateObject(user);

        TryValidateObject(user.Address);
    }
}
