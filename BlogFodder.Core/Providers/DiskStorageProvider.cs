using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using BlogFodder.Core.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Providers;

    /// <summary>
    /// Provider for saving and deleting files to disk
    /// </summary>
    public class DiskStorageProvider : IStorageProvider
    {
        private readonly IWebHostEnvironment _env;
        private readonly BlogFodderSettings _settings;

        public DiskStorageProvider(IWebHostEnvironment env,
                                    IOptions<BlogFodderSettings> settings)
        {
            _env = env;
            _settings = settings.Value;
        }

        /// <inheritdoc />
        public Task<FileSaveResult> CanUseFile(IBrowserFile file, bool onlyImages = false)
        {
            return Task.Run(() =>
            {
                var fileSaveResult = new FileSaveResult();

                if (onlyImages && !file.IsImage())
                {
                    fileSaveResult.ErrorMessages.Add("File must be an image only");
                    fileSaveResult.Success = false;
                }
                else
                {
                    // Check allowed filetypes
                    if (file.Name.Contains(_settings.AllowedFileTypes))
                    {
                        if (file.Size > _settings.MaxUploadFileSizeInBytes)
                        {
                            fileSaveResult.ErrorMessages.Add("File is too large");
                            fileSaveResult.Success = false;
                        }
                    }
                    else
                    {
                        fileSaveResult.ErrorMessages.Add("File not allowed");
                        fileSaveResult.Success = false;
                    }

                }
                return fileSaveResult;
            });
        }

        /// <inheritdoc />
        public Task<bool> DeleteFile(string? url, string? fileId = null)
        {
            return Task.Run(() =>
            {
                if (!url.IsNullOrWhiteSpace())
                {
                    var fullFilePath = Path.Combine(_env.WebRootPath, url.Replace("/", "\\"));
                    if (File.Exists(fullFilePath))
                    {
                        File.Delete(fullFilePath);
                        return true;
                    }   
                }
                return false;
            });
        }

        /// <inheritdoc />
        public async Task<FileSaveResult> SaveFile(IBrowserFile file, string contentidentifier, bool overwrite = true)
        {
            var fileSaveResult = await CanUseFile(file);
            fileSaveResult.OriginalFile = file;
            if (!fileSaveResult.Success)
            {
                return fileSaveResult;
            }

            try
            {
                var relativePath = Path.Combine(_settings.UploadFolderName ?? "uploads", contentidentifier);
                var dirToSave = Path.Combine(_env.WebRootPath, relativePath);
                var di = new DirectoryInfo(dirToSave);
                if (!di.Exists)
                {
                    di.Create();
                }
                var filePath = Path.Combine(dirToSave, file.Name);
                using (var stream = file.OpenReadStream(_settings.MaxUploadFileSizeInBytes))
                {
                    if (file.IsImage())
                    {
                        using var image = await Image.LoadAsync(stream);
                        image.OverMaxSizeCheck(_settings.MaxImageSizeInPixels);
                        await image.SaveAsync(filePath);
                        fileSaveResult.Width = image.Width;
                        fileSaveResult.Height = image.Height;
                    }
                    else
                    {
                        using var mstream = new MemoryStream();
                        await stream.CopyToAsync(mstream);
                        await File.WriteAllBytesAsync(filePath, mstream.ToArray());
                    }
                }
                fileSaveResult.SavedFileUrl = Path.Combine(relativePath, file.Name).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                fileSaveResult.ErrorMessages.Add(ex.Message);
                fileSaveResult.Success = false;
            }

            return fileSaveResult;
        }
        
        /// <inheritdoc />
        public Task<BlogFodderFile> ToBlogFodderFile(FileSaveResult fileSaveResult)
        {
            return Task.Run(() =>
            {
                var gabFile = new BlogFodderFile
                {
                    FileSize = fileSaveResult.OriginalFile?.Size ?? 0 
                };
                if (fileSaveResult.OriginalFile?.IsImage() == true)
                {
                    gabFile.ExtendedData.Add("imageWidth", fileSaveResult.Width);
                    gabFile.ExtendedData.Add("imageHeight", fileSaveResult.Height);
                }
                gabFile.FileType = fileSaveResult.OriginalFile?.Name.ToFileType() ?? BlogFodderFileType.Unknown;
                gabFile.DateCreated = DateTime.UtcNow;
                gabFile.Url = fileSaveResult.SavedFileUrl;
                return gabFile;
            });
        }
    }