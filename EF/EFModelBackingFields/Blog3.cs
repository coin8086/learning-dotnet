using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFModelBackingFields;

[EntityTypeConfiguration(typeof(Blog3EntityConfig))]
public class Blog3
{
    public int Id { get; set; }

    //Backing field for Url getter and setter methods
    [JsonInclude]
    [JsonPropertyName("Url")]
    private string _validatedUrl = default!;

    public string GetUrl() => _validatedUrl;

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
