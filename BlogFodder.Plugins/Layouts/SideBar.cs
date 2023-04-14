using Microsoft.AspNetCore.Components;

namespace BlogFodder.Plugins.Layouts;

public class SideBar : ComponentBase, IDisposable
{
    [CascadingParameter] public MainLayout? MainLayout { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        MainLayout?.SetSideBar(this);
        base.OnInitialized();
    }

    protected override bool ShouldRender()
    {
        var shouldRender = base.ShouldRender();
        if (shouldRender)
        {
            MainLayout?.Update();
        }

        return base.ShouldRender();
    }

    public void Dispose()
    {
        MainLayout?.SetSideBar(null);
    }
}