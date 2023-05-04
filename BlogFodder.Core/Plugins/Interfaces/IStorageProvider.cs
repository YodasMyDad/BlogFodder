using BlogFodder.Core.Media;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IStorageProvider
{
    /// <summary>
    /// Saves the file
    /// </summary>
    /// <param name="browserFile"></param>
    /// <param name="contentIdentifier"></param>
    /// <param name="overwrite"></param>
    public Task<FileSaveResult> SaveFile(IBrowserFile browserFile, string contentIdentifier, bool overwrite = true);


    /// <summary>
    /// Deletes the file
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fileId"></param>
    public Task<bool> DeleteFile(string? url, string? fileId = null);

    /// <summary>
    /// Checks to determine if a file can be used
    /// </summary>
    /// <param name="browserFile"></param>
    /// <param name="onlyImages"></param>
    public Task<FileSaveResult> CanUseFile(IBrowserFile browserFile, bool onlyImages = false);
    
    /// <summary>
    /// Method to convert a saved file into a BlogFodderFile object
    /// </summary>
    /// <param name="fileSaveResult"></param>
    /// <returns></returns>
    public Task<BlogFodderFile> ToBlogFodderFile(FileSaveResult fileSaveResult);
}