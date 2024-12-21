using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EFModelBackingFields;

[EntityTypeConfiguration(typeof(Blog2EntityConfig))]
public class Blog2
{
    public int Id { get; set; }

    //Backing field for property Url
    private string _validatedUrl = default!;

    /*
     * NOTE
     *
     * The attribute BackingField doesn't work as documented at
     * https://learn.microsoft.com/en-us/ef/core/modeling/backing-field?tabs=data-annotations
     * So Blog2EntityConfig is used instead.
     */
    //[BackingField(nameof(_validatedUrl))]
    [Required]
    public string Url => _validatedUrl;

    public void SetUrl(string url)
    {
        //Validate url and set...
        _validatedUrl = url;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
