## Entity Framework

`dotnet tool update --global dotnet-ef`

`cd BlogFodder.Core`  
`dotnet ef --startup-project ../BlogFodder.Web/ migrations add Initial -o "Data/Migrations"`  
`dotnet ef database update --context BlogFodderDbContext`

Misc Links

https://learn.microsoft.com/en-gb/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns  
https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Miscellaneous/NewInEFCore7/JsonColumnsSample.cs

Razor Components

https://learn.microsoft.com/en-us/aspnet/core/blazor/components/css-isolation?view=aspnetcore-7.0
https://learn.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-7.0&tabs=visual-studio
//https://stackoverflow.com/questions/50762385/loading-razor-class-libraries-as-plugins
//https://stackoverflow.com/questions/51003853/how-compiledrazorassemblypart-should-be-used-to-load-razor-views

    //Load this project and copy it
    //https://github.com/DominikAmon/RclIssueDemo/blob/master/RclDemo.WebApplication/Startup.cs