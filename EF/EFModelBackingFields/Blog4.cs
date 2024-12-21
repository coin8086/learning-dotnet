using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EFModelBackingFields;

public class Blog4
{
    public int Id { get; set; }

    [Required]
    public string Url { get; private set; } = default!;

    public void SetUrl(string url)
    {
        //Validate url and set...
        Url = url;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
