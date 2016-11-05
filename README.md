[![Build status](https://ci.appveyor.com/api/projects/status/r6inrwvd2ubsqv5m?svg=true)](https://ci.appveyor.com/project/sergeyzwezdin/cqs-net) [![Nuget version](https://img.shields.io/nuget/v/CqrsSpirit.svg)](https://www.nuget.org/packages/CqrsSpirit)

Lightweight library to implement CQRS pattern with .NET Core applications.

# Getting started

Install `CqrsSpirit` package into your .NET project.

```
PM> Install-Package CqrsSpirit
```

The library have ready extensions to register all stuff in `IServiceCollection` (which is used in .NET Core by default). Everything you need to register components is to call `AddCqrsSpirit()`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.AddCqrsSpirit();
}
```

To use queries and commands you'll need data context. It could be Entity Framework `DbContext`, plain .NET object based context or anything you want. To be able to resolve it you need to store it in IoC container as well:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.AddSingleton<Datasource.Datasource>();
    services.AddCqrsSpirit();
}
```

Keep in mind all stuff (e.g. queries, commands, workflows) will be added to IoC container on `AddCqrsSpirit()` method call so you wouldn't need to register it yourself.

## Query

To create query define interface based on `IQuery` and add `Execute()` method there:

```csharp
public interface IGetValuesQuery : IQuery
{
    Task<IEnumerable<Item>> ExecuteAsync(string filter = null);
}
```

To implement query logic you'll need to create class based on this interface. To access data context you'll need to inherit base class `ObjectQueryBase` as well:

```csharp
public class GetValuesQuery : ObjectQueryBase<Datasource.Datasource>, IGetValuesQuery
{
    public GetValuesQuery(Datasource.Datasource dbContext)
        : base(dbContext)
    {
    }

    public Task<IEnumerable<Item>> ExecuteAsync(string filter = null)
    {
        // ...
    }
}
```

Now you're ready to run this query from your application. Just inject `IGetValuesQuery` into your application components and run it:

```csharp
public class ValuesController : Controller
{
    protected readonly IGetValuesQuery GetValuesQuery;

    public ValuesController(IGetValuesQuery getValuesQuery)
    {
        GetValuesQuery = getValuesQuery;
    }

    [HttpGet]
    [Route("api/values")]
    public async Task<IEnumerable<Item>> Get(string filter = null)
    {
        var result = await GetValuesQuery.ExecuteAsync(filter);
        return result;
    }
}
```

## Command

To create command define plain .NET class based on `ICommand` marker interface and add all required data fields there:

```csharp
public class UpdateItemCommand : ICommand
{
    public UpdateItemCommand(int id, string value)
    {
        Id = id;
        Value = value;
    }

    public int Id { get; }

    public string Value { get; }
}
```

To handle this command you'll need to create command handler. To do it create class which implements `ICommandHandler<>` interface. To have access to data context you can use `ObjectCommandHandlerBase<>` instead:

```csharp
public class UpdateItemCommandHandler : ObjectCommandHandlerBase<UpdateItemCommand, Datasource.Datasource>
{
    public UpdateItemCommandHandler(Datasource.Datasource dbContext)
        : base(dbContext)
    {
    }

    public override async Task ExecuteAsync(UpdateItemCommand command)
    {
        // ...
    }
}
```

Now you can run this command by using `ICommandsDispatcher` interface:

```csharp
public class ValuesController : Controller
{
    protected readonly ICommandsDispatcher CommandsDispatcher;

    public ValuesController(ICommandsDispatcher commandsDispatcher)
    {
        CommandsDispatcher = commandsDispatcher;
    }

    [HttpPost]
    [Route("api/values/update")]
    public async Task Update(int id, string value)
    {
        await CommandsDispatcher.ExecuteAsync(new UpdateItemCommand(id, value));
    }
}
```

## Workflow

Workflow is kind of container for business logic. It could combine set of queries and commands into single operation which matters to end user.

To implement workflow rule create interface based on `IWorkflow` and implement it:

```csharp
public interface IMakeEvenItemsLowercaseWorkflow : IWorkflow
{
    Task ExecuteAsync();
}

public class MakeEvenItemsLowercaseWorkflow : IMakeEvenItemsLowercaseWorkflow
{
    protected readonly IGetValuesQuery GetValuesQuery;

    protected readonly ICommandsDispatcher CommandsDispatcher;

    public MakeEvenItemsLowercaseWorkflow(IGetValuesQuery getValuesQuery, ICommandsDispatcher commandsDispatcher)
    {
        GetValuesQuery = getValuesQuery;
        CommandsDispatcher = commandsDispatcher;
    }

    public async Task ExecuteAsync()
    {
        // Using query
        var items = await GetValuesQuery.ExecuteAsync();

        var itemsToUpdate = items.Select((Value, Index) => new { Index, Value })
            .Where(x => (x.Index % 2) != 0)
            .Select(x => x.Value);

        // Using commands
        foreach (Item item in itemsToUpdate)
        {
            await CommandsDispatcher.ExecuteAsync(new UpdateItemCommand(item.Id, item.Value?.ToLowerInvariant()));
        }
    }
}
```

Using workflows in application components is quite similar to `IQuery`:

```csharp
public class ValuesController : Controller
{
    protected readonly IMakeEvenItemsLowercaseWorkflow MakeEvenItemsLowercaseWorkflow;

    public ValuesController(IMakeEvenItemsLowercaseWorkflow makeEvenItemsLowercaseWorkflow)
    {
        MakeEvenItemsLowercaseWorkflow = makeEvenItemsLowercaseWorkflow;
    }

    [HttpPost]
    [Route("api/values/dosomethings")]
    public async Task Workflow()
    {
        await MakeEvenItemsLowercaseWorkflow.ExecuteAsync();
    }
}
```

See complete sample web application [[here]](https://github.com/sergeyzwezdin/cqrs-spirit/tree/master/sources/SampleWebApp).

# License

Code released under the MIT license.
