using BlogFodder.Core.Media;
using BlogFodder.Core.Media.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Extensions;

    public static class FileExtensions
    {
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
        /// Checks if an image is over max size and resizes if it is
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxPixelSize"></param>
        public static void OverMaxSizeCheck(this Image image, int maxPixelSize)
        {
            if (image.Width > maxPixelSize || image.Height > maxPixelSize)
            {
                var size = new Size();
                if (image.Width > image.Height)
                {
                    size.Width = maxPixelSize;
                }
                else
                {
                    size.Height = maxPixelSize;
                }
                image.Mutate(x => x.Resize(size));
            }
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