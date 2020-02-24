# SQL Server Data Provider for Snoozle

[![Build Status](https://dev.azure.com/charliejkendall/Snoozle/_apis/build/status/CharlieJKendall.Snoozle.SqlServer?branchName=master)](https://dev.azure.com/charliejkendall/Snoozle/_build/latest?definitionId=2&branchName=master)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Snoozle.SqlServer)](https://www.nuget.org/packages/Snoozle.SqlServer)
[![Latest Release](https://img.shields.io/github/v/release/charliejkendall/snoozle.sqlserver)](https://github.com/CharlieJKendall/Snoozle.SqlServer/releases/latest)

### Installation

The package can be downloaded directly from the [NuGet Gallery | Snoozle.SqlServer](https://www.nuget.org/packages/Snoozle.SqlServer) or installed using the Visual Studio package manager UI/console.

``` ps1
PM> Install-Package Snoozle.SqlServer
```

### Quickstart Guide

#### 1. Call the `.AddSnoozleSqlServer()` extension method from the `IMvcBuilder` in Startup.cs

Configuration can by provided using the builder lambda (as below) or via an `IConfigurationSection` read from a proper configuration provider (e.g. appsettings.json).

``` cs
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddControllers()
        .AddSnoozleSqlServer(
            options =>
            {
                options.ConnectionString = "Server=.;Database=Snoozle;Trusted_Connection=True;";
            });
}
```

#### 2. Create a model class representing your SQL data schema

This *must* inherit from the `IRestResource` marker interface. Nullable columns that are modelled by value types (e.g `int`, `long`, `DateTime`) should be nullable on the model.

``` cs
public class Cat : IRestResource
{
    public int Id { get; set; }

    public long? HairLength { get; set; }

    public string Name { get; set; }

    public DateTime DateCreated { get; set; }
}
```

#### 3. Create a resource configuration class for your model

Inherit from the `SqlResourceConfigurationBuilder` class, passing your model as the generic type parameter and overriding the abstract `Configure()` method from the base class.

There are two key methods that the base class provides: `ConfigurationForModel()` and `ConfigurationForProperty()`. These set model- and property-level configurations for the resource. Any property named `Id` or `<resource_type_name>Id` (e.g. `CatId`) will be automatically set by convention as the primary key/identifier. If your primary key has another name, it can be set using the `IsPrimaryIdentifier()` method on the property builder.

``` cs
public class CatResourceConfiguration : SqlResourceConfigurationBuilder<Cat>
{
    public override void Configure()
    {
        ConfigurationForModel().HasTableName("Cats").HasAllowedHttpVerbs(HttpVerb.All).HasRoute("catters");

        ConfigurationForProperty(x => x.HairLength).HasColumnName("HairLengthInMeters");
        ConfigurationForProperty(x => x.Id).HasColumnName("CatId").IsPrimaryIdentifier();
        ConfigurationForProperty(x => x.DateCreated).HasComputedValue().DateTimeNow();
        ConfigurationForProperty(x => x.Name).HasColumnName("CatName");
    }
}

```

You should now be able to run your web application and access your resource at `<root_url>/api/<resource>`, for example `curl -k https://localhost:44343/api/catters`. Unless you have specified otherwise, you should be able to POST, PUT, GET, DELETE to the resource.
