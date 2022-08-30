# Component integration testing

In the previous sections, we tested as small units of work as we could. In this section, we will broaden our testing scope a bit, as we look to test multiple (sometimes all) of our components in the same test methods.

### Get weather details from an external API
As previously mentioned, we will fetch weather data from an external API - what better API for fetching weather data is there than Yr? 

You can read more about the API here: https://developer.yr.no/doc/

The application code for fetching data from the Yr API is already implemented. You can find it in the `Yr.Facade` project. The same applies to the data access code in the `Forte.Location.DataAccess` project.

The `Yr.Facade` and the `Forte.Location.DataAccess` are two of the components that we are looking to run tests against in our integration tests.

## Integration testing
To get started with our component integration tests, we start the same way as with unit tests, by creating a new MSTest project in our solution. You can call the project `Forte.Location.Api.IntegrationTests`.

To get our integration testing components working, install the following NuGet packages in our new project:
* `Microsoft.AspNetCore.Mvc.Testing`
* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.InMemory`

Inside the new project, we create a class called `LocationApiIntegrationTests`. This will be our main class for running integration tests. As with our unit test classes, give the class and its methods appropriate annotations (`[TestClass]`, `[TestMethod]` etc.).

We also need a setup method with the attribute `[TestInitialize]` which will run before each of our tests. Create your setup method and add the following line:
```cs
[TestInitialize]
public void Setup()
{
    var appFactory = new WebApplicationFactory<Program>();
}
```
We create a variable `appFactory` of type `WebApplicationFactory`. The `WebApplicationFactory` basically creates an in-memory test host for our application, and lets us run tests against it. We basically reference our `Program` class when we run `new WebApplicationFactory<Program>()`, and this makes sure that our in-memory application is configured exactly the same as our "original" application.

### Database testing
Most times (but not always), we don't want to test against our actual production database. Ideally, we would test against a database of *the same type* as the production database. So if we're using SQL Server in production, that's what we ideally want to use in the other environments as well.

For the sake of simplicity, we will use an in-memory database for our integration tests. Modify your `Setup` method so that it looks like this:
```cs
[TestInitialize]
public void Setup()
{
    var appFactory = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LocationDbContext>));
            services.Remove(descriptor);

            services.AddDbContext<LocationDbContext>(
                options => options.UseInMemoryDatabase("TestDb"));

            var context = services.BuildServiceProvider().GetRequiredService<LocationDbContext>();

            context.Database.EnsureDeleted();
        });
    });
}
```
There's a lot happening here. Try to understand what is happening in the rest of the constructor code before reading on. Done trying? 

Okay, to sum it up, we basically override some of the parts in the `WebHostBuilder` that's in our `Program` class with *other* parts, specific to our integration test cases. First, we remove the service type descriptor for our database context with the line `services.Remove(descriptor);`. In the next part, we add a new database context with the option `UseInMemoryDatabase`. This, as the name explains, lets our test web host use an in-memory database when running our tests. The name `TestDb` is arbitrary, and can basically be whatever you like. In the last three lines, we create a variable holding our database context. The `EnsureDeleted` method makes sure our database is cleared after each test method has finished, so that the state of one test method won't affect other test methods.

To sum it up, we replaced the original app database with an in-memory one that we can use when running our tests.

### HTTP Client
We need an HTTP client to run against our API in our different integration tests.

Let's create one in our `LocationApiIntegrationTests` class. Add a field:
```cs
private HttpClient _testClient;
```

Then initialize it in the `Setup` method, after the creation of the `appFactory`, with the following code:
```cs
_testClient = appFactory.CreateClient();
```
The `CreateClient` method creates an instance of type `HttpClient` suited for the `WebApplicationFactory`.

Now that we've created both a test host (the `WebApplicationFactory`) and our `HttpClient`, we basically have all of our boilerplate code done.

Let's move on to creating our first integration tests!

**Next up - [Creating the component integration tests](02b-creating-component-integration-tests.md)**