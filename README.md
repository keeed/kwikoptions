# kwikoptions
A library for dynamically injecting Options in ServiceCollection.

 [![Downloads](https://img.shields.io/nuget/dt/KwikOptions?style=for-the-badge)](https://www.nuget.org/packages/KwikOptions/)
 [![Version](https://img.shields.io/nuget/v/KwikOptions?style=for-the-badge)](https://www.nuget.org/packages/KwikOptions/)

## Usage

```csharp
// Add this in your IServiceCollection.
// For example, in ASP.NET Core under Startup:
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    
    services.UseKwikOptions(Configuration);
}
```

## Basic Configuration

In your settings file:

```json
{
  "KwikOptions": {
    "OptionsTypes": [
      {
        "OptionsPath": "Sample",
        "Type": "MyProject.SampleOptions, MyProject",
        "Assembly": "MyProject.dll"
      }
    ]
  },
  "Sample": {
    "Value": "Hello"
  }
}
```
