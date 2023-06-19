using System.Text.Json.Serialization;
using BlogFodder.Core.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Plugins.ContentEditors.Carousel;

public class CarouselItem
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    
    public string? Heading { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public Guid? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public IBrowserFile? Image { get; set; }
}