using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EFModelBackingFields;

public class Blog
{
    public int Id { get; set; }

    //Backing field for property Url
    private string _validatedUrl = default!;

    [Required]
    public string Url
    {
        get { return _validatedUrl; }
        set { _validatedUrl = value; }
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
