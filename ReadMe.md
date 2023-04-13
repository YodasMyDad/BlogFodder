# Overview

We are seriously lacking an open source Wordpress alternative in .Net land. 
A blogging platform that is really simple to use, but also really simple (And easy to understand) to create plugins or extend the platform.

Currently, from my experience, the only semi-decent blogs you can use in .Net, you need to install via plugins into a CMS like Umbraco or Orchard. 
Then to extend, you need to know how to use those platforms plugin system and use the language of their choice. For example Umbraco is currently AngularJs.

.Net has made huge improvements recently, and adding the concepts of Razor Class Libraries and Razor Components. I believe we can create a plugin system that makes
use of both these 'newish' concepts, and allow .Net developers, to create plugins, with .Net C# (As well as JS as when needed).

This POC is me just messing around with ideas of how that might work.

## Calling .Net & Blazor Fans

I am looking to create a small team, of much smarter than me folk, to help me move this POC/idea into something people can use. I have lot's of ideas for concepts and how
things might work, It would seem I'm just not a good enough developer to put these all in action.

If this sounds of interest, please message me on Twitter @YodasMyDad

### Concept

The idea is that all plugins are self contained Razor Class Libraries, because we can put the static assets in the wwwroot and use Razor Components 
for the UI / Plugins.

In this POC, I have created a blog post, which has two plugins available to use TODO FINISH THIS

I'm hoping you should just be able to clone, and run it, and see exactly how this starting point works.

### Future

I want to make all parts of the site pluggable, similar to Wordpress, from the header, to side columns, to even intercepting the final output HTML 
and being able to adapt it as needed. 

### Entity Framework

To update and make changes to the EF Core model, the migrations are in the .Core project. Migrations are auto applied at startup in the Program.cs.

`dotnet tool update --global dotnet-ef`

`cd BlogFodder.Core`  
`dotnet ef --startup-project ../BlogFodder.Web/ migrations add NameOfMigration -o "Data/Migrations"`

// Optional  
`dotnet ef database update --context BlogFodderDbContext`