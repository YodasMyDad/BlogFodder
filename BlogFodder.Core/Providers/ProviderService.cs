using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Providers;

public class ProviderService
{
    private readonly BlogFodderSettings _settings;
    private readonly ExtensionManager _extensionManager;

    public ProviderService(IOptionsSnapshot<BlogFodderSettings> gabSettings, ExtensionManager extensionManager)
    {
        _settings = gabSettings.Value;
        _extensionManager = extensionManager;
    }

    private IStorageProvider? _storageProvider;

    public IStorageProvider? StorageProvider
    {
        get
        {
            if (_storageProvider == null)
            {
                var storageProviders = _extensionManager.GetInstances<IStorageProvider>();
                if (_settings.Plugins.IStorageProvider != null)
                {
                    _storageProvider = storageProviders[_settings.Plugins.IStorageProvider];
                }
            }
            return _storageProvider;
        }
    }
}