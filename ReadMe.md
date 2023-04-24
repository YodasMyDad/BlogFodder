## Overview

We are seriously lacking an open source WordPress alternative in .Net land. A blogging/content platform that is really simple to use, has all the features you'd expect for a modern SEO focused content platform, but most importantly is really easy to create plugins or extend using .Net C# as much as possible.

.Net has made huge improvements recently, and adding the concepts of Razor Class Libraries and Razor Components. I believe we can create a plugin system that makes use of both these 'newish' concepts, and allow .Net developers, to create plugins, with .Net C# (As well as JS as when needed).

This is a Blazor Server app that is a POC and just me messing around and getting some initial code working.

### Progress

Right now, the plugin system is taking shape, everything you see is 'pluggable'. From the content items on a post, to the post preview, settings the works, the admin section itself. It uses MudBlazor for the Blazor components at the moment, but this may not be final as I still find them quite restrictive. However, it is all very rough and definitely subject to change. 

It's very simple right now, it has no authentication, and is just a simple front end that lists the posts made, and you can view an individual post. A post has content items, each of those is a plugin, and has a ContentPlugin to render on the front end. If you look at the code it should make sense, or read below in the plugin section.

The admin (/admin) section is where I have been focusing, and you can create a simple post which currently has two POC plugins available, A Rich Text Editor & Markdown Editor. Where you can create the post content content sections, sort them etc...

ADD SIMPLE ANIMATED GIF HERE SHOWING CURRENT FUNCTIONALITY

### Current Plugins

Sorry, only a couple at the moment. But, the final concept will be users can create Nuget packages that just reference the BlogFodder.Plugins project/nuget package. That's it. Oh, also, all plugins Namespaces must start with BlogFodder.Plugins.XX (Again, all this is subject to change)

#### IEditorPlugin

This controls the post editors, which are how the user can edit content and how it is previewed in the admin section, settings and how it is rendered in the front end (Also global settings).

The two implementations of this are **MarkdownEditorEditorPlugin** and **RichTextEditorEditorPlugin**, so check those out if you want to know more.

#### IBackOfficeNavigationItem

This just controls a link being added to a section in the admin navigation, can be a class, or a Blazor Page can inherit from the Interface.

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