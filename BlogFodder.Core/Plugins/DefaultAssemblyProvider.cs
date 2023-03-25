using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace BlogFodder.Core.Plugins;

public class DefaultAssemblyProvider
{
  protected ILogger logger;

  /// <summary>
  /// Gets or sets the predicate that is used to filter discovered assemblies from a specific folder
  /// before thay have been added to the resulting assemblies set.
  /// </summary>
  public Func<Assembly, bool> IsCandidateAssembly { get; set; }

  /// <summary>
  /// Gets or sets the predicate that is used to filter discovered libraries from a web application dependencies
  /// before thay have been added to the resulting assemblies set.
  /// </summary>
  public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DefaultAssemblyProvider">AssemblyProvider</see> class.
  /// </summary>
  /// <param name="serviceProvider">The service provider that is used to create a logger.</param>
  public DefaultAssemblyProvider(IServiceProvider serviceProvider)
  {
    this.logger = serviceProvider.GetService<ILoggerFactory>()!.CreateLogger("BlogFodder.Core");
    this.IsCandidateAssembly = assembly =>
      assembly.FullName!.StartsWith("BlogFodder", StringComparison.OrdinalIgnoreCase);

        this.IsCandidateCompilationLibrary = library =>
      library.Name.StartsWith("BlogFodder", StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Discovers and then gets the discovered assemblies from a specific folder and web application dependencies.
  /// </summary>
  /// <param name="path">The extensions path of a web application.</param>
  /// <param name="includingSubpaths">
  /// Determines whether a web application will discover and then get the discovered assemblies from the subfolders
  /// of a specific folder recursively.
  /// </param>
  /// <returns>The discovered and loaded assemblies.</returns>
  public IEnumerable<Assembly?> GetAssemblies()
  {
    List<Assembly?> assemblies = new();

    this.GetAssembliesFromDependencyContext(assemblies);
    this.GetAssembliesFromPath(assemblies, AssemblyDirectory);
    return assemblies;
  }

  private void GetAssembliesFromDependencyContext(List<Assembly?> assemblies)
  {
    this.logger.LogInformation("Discovering and loading assemblies from DependencyContext");

    foreach (CompilationLibrary compilationLibrary in DependencyContext.Default?.CompileLibraries!)
    {
      if (this.IsCandidateCompilationLibrary(compilationLibrary))
      {
        Assembly? assembly = null;

        try
        {
          assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name));

          if (!assemblies.Any(a => string.Equals(a?.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
          {
            assemblies.Add(assembly);
            this.logger.LogInformation("Assembly '{FullName}' is discovered and loaded", assembly.FullName);
          }
        }

        catch (Exception e)
        {
          this.logger.LogWarning("Error loading assembly '{Name}'", compilationLibrary.Name);
          // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
          this.logger.LogWarning(message: e?.Message);
        }
      }
    }
  }
  
  private string AssemblyDirectory
  {
    get
    {
      string codeBase = Assembly.GetExecutingAssembly().Location;
      UriBuilder uri = new UriBuilder(codeBase);
      var path = Uri.UnescapeDataString(uri.Path);
      return Path.GetDirectoryName(path) ?? "";
    }
  }

  private void GetAssembliesFromPath(List<Assembly> assemblies, string path)
  {
    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
    {
      this.logger.LogInformation("Discovering and loading assemblies from path '{0}'", path);

      foreach (var extensionPath in Directory.EnumerateFiles(path, "*.dll"))
      {
        Assembly assembly;

        try
        {
          assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

          if (this.IsCandidateAssembly(assembly) && !assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
          {
            assemblies.Add(assembly);
            this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
          }
        }

        catch (Exception e)
        {
          this.logger.LogWarning("Error loading assembly '{0}'", extensionPath);
          this.logger.LogWarning(e.ToString());
        }
      }

      /*if (includingSubpaths)
        foreach (string subpath in Directory.GetDirectories(path))
          this.GetAssembliesFromPath(assemblies, subpath, includingSubpaths);*/
    }

    else
    {
      if (string.IsNullOrEmpty(path))
        this.logger.LogWarning("Discovering and loading assemblies from path skipped: path not provided", path);

      else this.logger.LogWarning("Discovering and loading assemblies from path '{0}' skipped: path not found", path);
    }
  }
}