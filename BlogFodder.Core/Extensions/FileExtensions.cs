using BlogFodder.Core.Data;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Extensions;

    public static class FileExtensions
    {
        public static async Task<BlogFodderFile?> AddFileToDb<T>(this IBrowserFile browserFile, Guid id,
            HandlerResult<T> result,
            ProviderService providerService, BlogFodderDbContext dbContext)
        {
            return await browserFile.AddFileToDb(id.ToString(), result, providerService, dbContext);
        }

        /// <summary>
        /// Saves the BrowserFile as a BlogFodderFile using the set StorageProvider
        /// </summary>
        /// <param name="browserFile"></param>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <param name="providerService"></param>
        /// <param name="dbContext"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<BlogFodderFile?> AddFileToDb<T>(this IBrowserFile browserFile, string id, HandlerResult<T> result, 
            ProviderService providerService, BlogFodderDbContext dbContext)
        {
            var fileSaveResult = await providerService.StorageProvider!.SaveFile(browserFile, id);
            if (!fileSaveResult.Success)
            {
                foreach (var errorMessage in fileSaveResult.ErrorMessages)
                {
                    result.AddMessage(errorMessage, ResultMessageType.Warning);
                }

                return null;
            }

            // Create the file
            var file = await providerService.StorageProvider.ToBlogFodderFile(fileSaveResult);

            // Save the file first
            dbContext.Files.Add(file);

            // Set the file to the user
            return file;
        }
        
        /// <summary>
        /// Is this file a video based on the extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsVideo(this IBrowserFile file)
        {
            return file.Name.IsVideo();
        }

        /// <summary>
        /// Is this file a video based on the extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsVideo(this string fileName)
        {
            foreach (var fType in Constants.Files.VideoFileTypes)
            {
                if (fileName.EndsWith(fType, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is file an image
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsImage(this string fileName)
        {
            foreach (var fType in Constants.Files.ImageFileTypes)
            {
                if (fileName.EndsWith(fType, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is file an audio file
        /// </summary>
        /// <param name="browserFile"></param>
        /// <returns></returns>
        public static bool IsAudio(this IBrowserFile browserFile)
        {
            return browserFile.Name.IsAudio();
        }

        /// <summary>
        /// Is file an audio file
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsAudio(this string fileName)
        {
            foreach (var fType in Constants.Files.AudioFileTypes)
            {
                if (fileName.EndsWith(fType, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Is the file an image
        /// </summary>
        /// <param name="browserFile"></param>
        /// <returns></returns>
        public static bool IsImage(this IBrowserFile browserFile)
        {
            return browserFile.Name.IsImage();
        }

        /// <summary>
        /// Returns a file type from the film name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BlogFodderFileType ToFileType(this string fileName)
        {
            if(fileName.IsImage())
            {
                return BlogFodderFileType.Image;
            }

            if (fileName.IsVideo())
            {
                return BlogFodderFileType.Video;
            }

            if (fileName.IsAudio())
            {
                return BlogFodderFileType.Audio;
            }

            return BlogFodderFileType.File;
        }
    }