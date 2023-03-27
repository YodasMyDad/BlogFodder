## Entity Framework

`dotnet tool update --global dotnet-ef`

`cd Merch.Core`  
`dotnet ef --startup-project ../Merch.Web/ migrations add SetNull -o "Shared/Persistence/Migrations"`

`dotnet ef database update --context SqlLiteDbContext`

`cd ./project_with_migrations_folder`  
`dotnet ef --startup-project ../my_startup_project_path/ migrations add myMigration01`

Misc Links

https://learn.microsoft.com/en-gb/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns  
https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Miscellaneous/NewInEFCore7/JsonColumnsSample.cs

Razor Components

https://learn.microsoft.com/en-us/aspnet/core/blazor/components/css-isolation?view=aspnetcore-7.0
https://learn.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-7.0&tabs=visual-studio