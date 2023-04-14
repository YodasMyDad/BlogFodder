using Microsoft.AspNetCore.Components;

namespace BlogFodder.Plugins.Layouts;

public class AdminTopBar : ComponentBase, IDisposable
{
    [CascadingParameter] public AdminLayout? AdminLayout { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        AdminLayout?.SetAdminTopBar(this);
        base.OnInitialized();
    }

    protected override bool ShouldRender()
    {
        var shouldRender = base.ShouldRender();
        if (shouldRender)
        {
            AdminLayout?.Update();
        }

        return base.ShouldRender();
    }

    public void Dispose()
    {
        AdminLayout?.SetAdminTopBar(null);
    }
}