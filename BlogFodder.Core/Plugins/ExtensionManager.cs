﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins
{
  /// <summary>
  /// Represents the assembly cache with the mechanism of getting implementations or instances of a given type.
  /// This is the global access point to the ExtCore type discovering mechanism.
  /// </summary>
  /// This Extension manager is adapted from code found in https://github.com/ExtCore/ExtCore
  public class ExtensionManager
  {
    private readonly IServiceProvider _serviceProvider;
    public ExtensionManager(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// Gets the first implementation of the type specified by the type parameter, or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the same type(s) is requested.
    /// </param>
    /// <returns>The first found implementation of the given type.</returns>
    public Type GetImplementation<T>(bool useCaching = false)
    {
      return GetImplementation<T>(null!, useCaching);
    }

    /// <summary>
    /// Gets the first implementation of the type specified by the type parameter and located in the assemblies
    /// filtered by the predicate, or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the same type(s) is requested.
    /// </param>
    /// <returns>The first found implementation of the given type.</returns>
    public Type GetImplementation<T>(Func<Assembly, bool>? predicate, bool useCaching = false)
    {
      return GetImplementations<T>(predicate, useCaching).FirstOrDefault()!;
    }

    /// <summary>
    /// Gets the implementations of the type specified by the type parameter.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the same type(s) is requested.
    /// </param>
    /// <returns>Found implementations of the given type.</returns>
    public IEnumerable<Type> GetImplementations<T>(bool useCaching = false)
    {
      return GetImplementations<T>(null, useCaching);
    }

    /// <summary>
    /// Gets the implementations of the type specified by the type parameter and located in the assemblies
    /// filtered by the predicate.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the same type(s) is requested.
    /// </param>
    /// <returns>Found implementations of the given type.</returns>
    public IEnumerable<Type> GetImplementations<T>(Func<Assembly, bool>? predicate, bool useCaching = false)
    {
      var type = typeof(T);

      if (useCaching && AssemblyManager.Types.TryGetValue(type, out var foundImplementation))
        return foundImplementation;

      var implementations = new List<Type>();

      foreach (var assembly in GetAssemblies(predicate))
        foreach (var exportedType in assembly?.GetExportedTypes()!)
          if (type.GetTypeInfo().IsAssignableFrom(exportedType) && exportedType.GetTypeInfo().IsClass)
            implementations.Add(exportedType);

      if (useCaching)
        AssemblyManager.Types[type] = implementations;

      return implementations;
    }
    
    /// <summary>
    /// Returns a type from a string name
    /// </summary>
    /// <param name="predicate">Optional filter</param>
    /// <param name="strFullyQualifiedName">Fully qualified name of the component</param>
    /// <param name="useCaching">Cache the type</param>
    /// <returns></returns>
    public Type? GetTypeFromName(Func<Assembly, bool>? predicate, string strFullyQualifiedName, bool useCaching = false)
    {
      if (useCaching && AssemblyManager.TypesByName.TryGetValue(strFullyQualifiedName, out var name))
      {
        return name;
      }

      var type = Type.GetType(strFullyQualifiedName);
      if (type == null)
      {
        foreach (var asm in GetAssemblies(predicate))
        {
          type = asm?.GetType(strFullyQualifiedName);
          if (type != null)
          {
            break;
          }
        }
      }

      if (useCaching && type != null)
      {
        AssemblyManager.TypesByName[strFullyQualifiedName] = type;
      }

      return type;
    }

    /// <summary>
    /// Gets the new instance of the first implementation of the type specified by the type parameter,
    /// or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <returns>The instance of the first found implementation of the given type.</returns>
    public T? GetInstance<T>(bool useCaching = false)
    {
      return GetInstance<T>(null, useCaching, new object[] { });
    }

    /// <summary>
    /// Gets the new instance (using constructor that matches the arguments) of the first implementation
    /// of the type specified by the type parameter or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <param name="args">The arguments to be passed to the constructor.</param>
    /// <returns>The instance of the first found implementation of the given type.</returns>
    public T? GetInstance<T>(bool useCaching = false, params object[] args)
    {
      return GetInstance<T>(null, useCaching, args);
    }

    /// <summary>
    /// Gets the new instance of the first implementation of the type specified by the type parameter
    /// and located in the assemblies filtered by the predicate or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <returns>The instance of the first found implementation of the given type.</returns>
    public T? GetInstance<T>(Func<Assembly, bool>? predicate, bool useCaching = false)
    {
      return GetInstances<T>(predicate, useCaching).FirstOrDefault().Value;
    }

    /// <summary>
    /// Gets the new instance (using constructor that matches the arguments) of the first implementation
    /// of the type specified by the type parameter and located in the assemblies filtered by the predicate
    /// or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <param name="args">The arguments to be passed to the constructor.</param>
    /// <returns>The instance of the first found implementation of the given type.</returns>
    public T? GetInstance<T>(Func<Assembly, bool>? predicate, bool useCaching = false, params object[] args)
    {
      return GetInstances<T>(predicate, useCaching, args).FirstOrDefault().Value;
    }

    /// <summary>
    /// Gets the new instances of the implementations of the type specified by the type parameter
    /// or empty enumeration if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <returns>The instances of the found implementations of the given type.</returns>
    public Dictionary<string, T> GetInstances<T>(bool useCaching = false)
    {
      return GetInstances<T>(null, useCaching, new object[] { });
    }

    /// <summary>
    /// Gets the new instances (using constructor that matches the arguments) of the implementations
    /// of the type specified by the type parameter or empty enumeration if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <param name="args">The arguments to be passed to the constructors.</param>
    /// <returns>The instances of the found implementations of the given type.</returns>
    public Dictionary<string, T> GetInstances<T>(bool useCaching = false, params object[] args)
    {
      return GetInstances<T>(null, useCaching, args);
    }

    /// <summary>
    /// Gets the new instances of the implementations of the type specified by the type parameter
    /// and located in the assemblies filtered by the predicate or empty enumeration
    /// if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <returns>The instances of the found implementations of the given type.</returns>
    public Dictionary<string, T> GetInstances<T>(Func<Assembly, bool>? predicate, bool useCaching = false)
    {
      return GetInstances<T>(predicate, useCaching, new object[] { });
    }

    /// <summary>
    /// Gets the new instances (using constructor that matches the arguments) of the implementations
    /// of the type specified by the type parameter and located in the assemblies filtered by the predicate
    /// or empty enumeration if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <param name="useCaching">
    /// Determines whether the type cache should be used to avoid assemblies scanning next time,
    /// when the instance(s) of the same type(s) is requested.
    /// </param>
    /// <param name="args">The arguments to be passed to the constructors.</param>
    /// <returns>The instances of the found implementations of the given type.</returns>
    public Dictionary<string, T> GetInstances<T>(Func<Assembly, bool>? predicate, bool useCaching = false, params object[] args)
    {
      var instances = new Dictionary<string, T>();

      foreach (var implementation in GetImplementations<T>(predicate, useCaching))
      {
        if (!implementation.GetTypeInfo().IsAbstract)
        {
          T? instance;
          if (args.Any())
          {
             instance = (T)Activator.CreateInstance(implementation, args)!; 
          }
          else
          {
            instance = (T)ActivatorUtilities.CreateInstance(_serviceProvider, implementation);
          }
          instances.Add(implementation.FullName ?? "WTF", instance);
        }
      }

      return instances;
    }

    public static IEnumerable<Assembly?> GetAssemblies(Func<Assembly, bool>? predicate)
    {
      if (predicate == null)
        return AssemblyManager.Assemblies;

      return AssemblyManager.Assemblies.Where(predicate!);
    }
  }
}