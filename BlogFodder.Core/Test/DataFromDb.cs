using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Test;

public static class DataFromDb
{
    public static List<Post> GetData()
    {
        return new List<Post>
        {
            new Post
            {
                Id = 1,
                Name = "Test Blog One",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ContentItems = new List<PostContentItem>
                {
                    new PostContentItem
                    {
                        Id = 1,
                        PluginAlias = "",
                        PluginData = "",
                        SortOrder = 1,
                        PostId = 1
                    }
                }
            },
            new Post
            {
                Id = 2,
                Name = "Test Blog Two",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ContentItems = new List<PostContentItem>()
            },
        };
    }
}