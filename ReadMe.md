## Overview

We are seriously lacking an open source WordPress alternative in .Net land. A blogging/content platform that is really simple to use, has all the features you'd expect for a modern SEO focused content platform, but most importantly is really easy to create plugins or extend using .Net C# as much as possible.

.Net has made huge improvements recently, and adding the concepts of Razor Class Libraries and Razor Components. I believe we can create a plugin system that makes use of both these 'newish' concepts, and allow .Net developers, to create plugins, with .Net C# (As well as JS as when needed).

### What This Is Not

A fully working blogging engine. This is a Blazor Server app that is a **POC** and just me messing around and getting some initial code working.

Also, full disclosure, I'm a not a super smart .Net geek. So I'm just cobbling this together as I go, so open to opinions on how to improve it from an architecture or performance point of view.

### Progress

Right now, the plugin system is taking shape, everything you see is 'pluggable'. From the content items on a post, to the post preview, settings the works, the admin section itself. It uses MudBlazor for the Blazor components at the moment, but this may not be final as I still find them quite restrictive. However, it is all very rough and definitely subject to change. 

It's very simple right now, it has no authentication, and is just a simple front end that lists the posts made, and you can view an individual post. A post has content items, each of those is a plugin, and has a ContentPlugin to render on the front end. If you look at the code it should make sense, or read below in the plugin section.

The admin (/admin) section is where I have been focusing, and you can create a simple post which currently has two POC plugins available, A Rich Text Editor & Markdown Editor. Where you can create the post content content sections, sort them etc...

To get it running, just make sure BlogFodder.App is set as the Start Up project, and you have restored all the nuget packages.

### Current Plugins

The final concept will be users can create Nuget packages that just reference the `BlogFodder.Plugins` project/nuget package (When the nuget package is created). That's it. Oh, also, all plugins Namespaces must start with BlogFodder.Plugins.XX (Again, all this is subject to change)

#### IEditorPlugin

This controls the post editors, which are how the user can edit content and how it is previewed in the admin section, settings and how it is rendered in the front end (Also global settings).

The two implementations of this are **`MarkdownEditorEditorPlugin`** and **`RichTextEditorEditorPlugin`**, so check those out if you want to know more about how to use them. I will update these docs more when this project is more of an alpha than POC.

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

You can create your own Storage provider for saving files. The app comes with the default `DiskStorageProvider` which stores all files to disk. But you can implement your own version to use say Azure Blob storage. Then you just need to update the `appSettings` to use your version i.e.

`"IStorageProvider": "BlogFodder.Plugins.CustomProviders.AzureStorageProvider"`

#### IExternalAuthenticationProvider

While the login/authentication hasn't been hooked up yet, the site is using .Net Identity as the authentication provider, but will only allow login via external providers. This was all the password management can be handled by the providers.

You can add External Authentication providers by Implementing the `IExternalAuthenticationProvider`. The site will launch with 3 existing providers already created which are Facebook, Google & Microsoft. You can see the code for these providers in `BlogFodder.Plugins` project

#### IPostPlugin

*Coming soon*. Idea is to be able to add plugins onto a post. An example would be, comments, this could easily be a plugin for posts, just like newsletter sign up.

#### ISitePlugin

*Coming soon*. Similar to the above, idea is to have plugins which will be enabled in certain parts of the site, i.e. Header, Footer, SideBar.

#### IRenderPlugin

*Coming soon*. This plugin will be executed in middleware and will allow you to manipulate the final HTML of the page before it gets to the user.

Some ideas for this will be replace keys in the content, for example, you have have something `#Year#` in the content and that is replaced with the current year **2023**

#### Theme Customisation

I've gone with a different route with the theme engine, all the components that make up the front end are in the `appSettings`. So if you want to override/restyle the front end, you just need to create your own versions of the components and point to them in the `appSettings`.

For example, lets saying you wanted to customise the Header either by adding a new feature or changing the styling. Just create your own Header component, by copying the existing one. Then in the `appSettings` point the site to your version of the `Header` component

`"HeaderComponent": "BlogFodder.Plugins.MyCustomProject.FrontEnd.Header"`

This way you can completely customise and update the site without touching the core code.

### Entity Framework

To update and make changes to the EF Core model, the migrations are in the .Core project. Migrations are auto applied at startup in the Program.cs.

```
dotnet tool update --global dotnet-ef

cd BlogFodder.Core
dotnet ef --startup-project ../BlogFodder.App/ migrations add NameOfMigrationHere -o "Data/Migrations"

// Optional - as migrations are run on startup  
dotnet ef database update --context BlogFodderDbContext
```

### Calling Experienced OSS .Net & Blazor Fans

I am looking to create a small team, of folk much smarter than me (Which won't be hard 😂) to help me move this POC/idea into something people can use. I have lot's of ideas for concepts and how things might work, It would seem I'm just not a good enough developer to put these all in action.

If this sounds of interest, please message me or tweet me on Twitter [@YodasMyDad](https://twitter.com/YodasMyDad)

#### Future

I want to have themes and make all parts of the site pluggable, similar to WordPress, from the header, to side columns, to even intercepting the final output HTML and being able to adapt it as needed to inject stuff into the final markup.

Once I find a bit more time. I'll update everything further to include features that I want to add.