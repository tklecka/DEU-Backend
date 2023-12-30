# DEU
Digitale Einsatzunterstützung

## TODO for "plugin system"
1. Define an Interface
- First, define an interface for data fetching. This interface will declare the methods that any custom implementation must adhere to. For example:

```[csharp]
public interface IDataFetcher
{
    Task<Data> FetchDataAsync();
}
```

2. Implement a Default Data Fetcher
- Implement the default data fetching logic using this interface:
```[csharp]
public class DefaultDataFetcher : IDataFetcher
{
    public async Task<Data> FetchDataAsync()
    {
        // Default implementation
    }
}
```

3. Allow Custom Implementation
- Enable users to provide their custom implementation by following these steps:
- Load Dynamically: Allow loading assemblies dynamically at runtime. This can be done using reflection. Users can compile their implementation into a DLL which your application can load.
- Configuration: Provide a configuration option (like a configuration file or a database setting) where users can specify the path to their custom DLL and the class name that implements IDataFetcher.
- Load and Use Custom Implementation: At runtime, load the specified DLL, create an instance of the specified class, and use it if it implements IDataFetcher.

Example:

```[csharp]
public IDataFetcher GetDataFetcher()
{
    string customImplementationPath = GetCustomImplementationPathFromConfig();

    if (!string.IsNullOrEmpty(customImplementationPath))
    {
        Assembly customAssembly = Assembly.LoadFrom(customImplementationPath);
        Type type = customAssembly.GetType("CustomNamespace.CustomDataFetcher");

        if (typeof(IDataFetcher).IsAssignableFrom(type))
        {
            return (IDataFetcher)Activator.CreateInstance(type);
        }
    }

    return new DefaultDataFetcher();
}
```

4. Documentation
- Provide clear documentation to users on how to create their implementation. This should include:
- The interface they need to implement.
- How to compile their implementation into a DLL.
- Where and how to specify their custom implementation in the configuration.