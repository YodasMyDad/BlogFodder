using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Settings.Commands;

public class CreateUpdateSiteSettingsCommand : IRequest<HandlerResult<SiteSettings>>
{
    public IBrowserFile? Logo { get; set; }
    public SiteSettings SiteSettings { get; set; } = new();
    public bool IsUpdate { get; set; }
}