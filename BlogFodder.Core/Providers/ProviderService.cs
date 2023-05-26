using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Providers;

public class ProviderService
{
    private readonly BlogFodderSettings _settings;
    private readonly ExtensionManager _extensionManager;

    public ProviderService(IOptions<BlogFodderSettings> gabSettings, ExtensionManager extensionManager)
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
                var storageProviders = _extensionManager.GetInstances<IStorageProvider>(true);
                if (_settings.Plugins.IStorageProvider != null)
                {
                    _storageProvider = storageProviders[_settings.Plugins.IStorageProvider];
                }
            }
            return _storageProvider;
        }
    }

    private IEmailProvider? _emailProvider;
    
    public IEmailProvider? EmailProvider
    {
        get
        {
            if (_emailProvider == null)
            {
                var emailProviders = _extensionManager.GetInstances<IEmailProvider>(true);
                if (_settings.Plugins.IEmailProvider != null)
                {
                    _emailProvider = emailProviders[_settings.Plugins.IEmailProvider];
                }
            }
            return _emailProvider;
        }
    }
}