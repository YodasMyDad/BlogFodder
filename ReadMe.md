## Overview

A plugin based Blazor blogging platform that is really easy to create plugins for or customise. 

The platform is built with Razor Class Libraries and Razor Components and has a plugin system built around these features which makes it easy to create plugins to extend / customise almost all areas of the Blog using Blazor. Front end is using Bootstrap V5 (But you could use what you want) and the backend/admin is built using MudBlazor.

View a brief overview video of the project here

**https://twitter.com/YodasMyDad/status/1667848590880907264**

This is a work in progress and I'm still trying to get it to a beta stage. I'm posting this publicly as I'm looking for other Blazor enthusiasts to get involved and help shape the project further.

**I am waiting for .Net 8 and the new Blazor (United) before there will be a v1 release, as I intend to port it all over to that and make use of their new SSR.**

### Get It Running

Make sure BlogFodder.App is the starting project, and just run it. On first run it creates the DB and adds some seed data, you can login using these details.

**admin@admin.com**  
**P@$$word1234**

If you want to use the social logins, add the social keys to the appSettings for the provider you want to use.

Then add your email address (Or a test email address) to the appSettings 'AdminEmailAddresses' list. Any email address in this list, when the user registers (Including via social logins) they are added to the Admin role so you can access the Admin section.

https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.App/appsettings.json#L88

**SqlLite or MSSQL are the only two DB's it supports at the moment**

The project is set to use SqlLite by default and all migrations have been generated for that. If you want to use MSSQL then you need to do the following

1. Update the `appSettings.json`. Change the `DatabaseProvider` to `SqlServer` and update `ConnectionString` to point to your MSSQL Db.

2. Once you have updated the `appSettings.json`, you need to regenerate the migrations as they were generated for SqlLite. Delete the '**Migrations**' folder in **BlogFodder.Core/Data**. Then run the EF Core migrations code. This will generate the MSSQL migration code.

   ```
   cd BlogFodder.Core
   dotnet ef --startup-project ../BlogFodder.App/ migrations add Initial -o "Data/Migrations"
   ```

3. Now just run the project as normal

### Current Plugins

The final concept will be where users can create Nuget packages that just reference the `BlogFodder.Plugins` nuget package (When the nuget package is created) and make sure the package starts with BlogFodder.Plugins.XX (Again, all this is subject to change)

The two main plugins are:

#### IPlugin

This is main plugin system for the blog. This allows you to render components in [specific places,](https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.Core/Plugins/Models/PluginDisplayArea.cs) have a settings page in the admin and also display a component on a post.

I have created (very rough) example Plugin, called **Comments**, a plugin that allows you to add comments on posts, manage them, approve and more. You can poke around the code here

https://github.com/YodasMyDad/BlogFodder/tree/master/BlogFodder.Plugins/Comments

Plugins have to be enabled first before they appear on the site, again you can see how this is done in the source code.

#### IEditorPlugin

This controls the content editors, which are how the user can edit content and how it is previewed in the admin section, settings and how it is rendered in the front end (Also global settings). 

The concept for the blog, is instead of having just a single RTE or Markdown Editor. Each block of content can be it's own editor and preview. 

I have a few content editors already (see them on the link below) and will try and add more complex ones when I have time.

https://github.com/YodasMyDad/BlogFodder/tree/master/BlogFodder.Plugins/ContentEditors

Some ideas for other editors for the future would be:

- Image Gallery Editor (Which options for layout)
- Table Editor (Create and edit tables)
- Image Generator Editor (Use DALL-E to create AI images from text prompt)
- Embed Tweet Editor (Allows you to paste a tweet URL and displays it on the front end)
- Code Editor (Code editor like ACE that allows you to edit the code and highlights it correctly on the front end)
- ChatGPT Editor (Uses TinyMCE or similar to allow you to create content)
- Amazon Affiliate Editor (Allows you to select products and create comparison tables with affiliate links)

**Then we have other ways to extend the platform with the following**

#### IBackOfficeNavigationItem

This just controls a link being added to a section in the admin navigation, can be a class, or a Blazor Page can inherit from the Interface.

If you are making a section in the admin with a number of pages, it's easier to just add a class, like I have done with the `PostNavigation.cs` for the Posts section in the admin. If it's just a single page, then it's probably easier to just Implement on the Razor component directly.

You can choose which section to add the link to and also add new sections of links by updating the appSettings and then adding your new section in your 

```json
"BackOffice": {
  "CustomNavigationSections": [
    "Custom"
  ]
}
```

#### IStorageProvider

You can create your own Storage provider for saving files. The app comes with the default `DiskStorageProvider` which stores all files to disk. 

https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.Core/Providers/DiskStorageProvider.cs

But you can implement your own version to use say Azure Blob storage. Then you just need to update the `appSettings` to use your version i.e.

`"IStorageProvider": "BlogFodder.Plugins.CustomProviders.AzureStorageProvider"`

#### IExternalAuthenticationProvider

While the login/authentication hasn't been hooked up yet, the site is using .Net Identity as the authentication provider, but will only allow login via external providers. This was all the password management can be handled by the providers.

You can add External Authentication providers by Implementing the `IExternalAuthenticationProvider`. The site will launch with 3 existing providers already created which are Facebook, Google & Microsoft. You can see the code for these providers in `BlogFodder.Plugins` project

https://github.com/YodasMyDad/BlogFodder/tree/master/BlogFodder.Plugins/Authentication

#### IStartUpPlugin

Allows you to add services during start up, example of using this in the Comments plugin

https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.Plugins/Comments/PostCommentsStartup.cs

#### IAdminDashboard

Allows you to render a component on the Admin dashboard, example of using it here, where I've created a Latest Posts dashboard.

https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.Plugins/Admin/Posts/LatestPostsDashboard.razor

#### Theme Customisation

I've gone with a different route with the theme engine, all the components that make up the front end are in the `appSettings`. So if you want to override/restyle the front end, you just need to create your own versions of the components and point to them in the `appSettings`.

For example, lets saying you wanted to customise the Header either by adding a new feature or changing the styling. Just create your own Header component, by copying the existing one. Then in the `appSettings` point the site to your version of the `Header` component

`"HeaderComponent": "BlogFodder.Plugins.MyCustomProject.FrontEnd.Header"`

This way you can completely customise and update the site without touching the core code.

https://github.com/YodasMyDad/BlogFodder/blob/master/BlogFodder.App/appsettings.json#L71

### Entity Framework

To update and make changes to the EF Core model, the migrations are in the .Core project. Migrations are auto applied at startup in the Program.cs.

```
dotnet tool update --global dotnet-ef

cd BlogFodder.Core
dotnet ef --startup-project ../BlogFodder.App/ migrations add Initial -o "Data/Migrations"

// Optional - as migrations are run on startup  
dotnet ef database update --context BlogFodderDbContext
```

### Calling Experienced OSS .Net & Blazor Fans

I am looking to create a small team, of folk much smarter than me to help me move/shape this POC/idea into something people can use. I have lot's of ideas for concepts and how things might work, It would seem I'm just not a good enough developer to put these all in action.

If this sounds of interest, please message or tweet me on Twitter [@YodasMyDad](https://twitter.com/YodasMyDad)
