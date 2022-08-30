# Creating component integration tests

## Creating test methods

Let's start with a couple of real simple integration tests, just to get our feet wet.

### Ensure success when getting all locations
Start by creating a test method where the only goal is to verify that our API returns an HTTP status code of `200` and a `Content-Type` header with the value `application/json; charset=utf-8`.

Remember that we did the boilerplate code in the `Setup` method, so we won't have to do that in our actual test methods.

Okay, so the first test method looks like this:
```cs
[TestMethod]
public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
{
    // Act
    var response = await _testClient.GetAsync("/api/weather/locations");

    // Assert
    Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
}
```
Hopefully, this looks familiar, as it's the exact same structure as our test methods from the unit testing section.

Try running the test now - it should pass.

### Ensure 404 Not Found when using unknown URL path
Okay, now you're on your own (not really :-)). Try creating an integration test in the same way as before, but this time, replace the URL path we used in the first test with a path that doesn't exist in our API, and make sure our API responds with a status code `404 Not Found`.

> :key: As before, if you're stuck, you can always look at the save points in the repository for inspiration.

### Ensure no locations are in the HTTP response when database is empty
Let's get a little more advanced. This test method should verify that when we try getting all locations, and our database is empty (as it should be at the moment), then our API will respond with an empty JSON array.

> :key: To get this test working, you might need to use some JSON utilities.

### Add location and verify correct response message
Our next integration test should verify that after we have added a location (with the `HTTP POST /api/weather/locations` operation), we get the correct message body in the HTTP response. You can use the SwaggerUI interface (or read the application code) to see what the proper response is. You can also add an assertion for the `200 OK` status code.

> :key: The point is to add the location programatically (in the test itself).

### Add location, then get all locations, and verify JSON array
As before, try adding a location. After adding it, do the get all locations operation and verify that the JSON array in the HTTP response is no longer empty.

### Add three locations and verify exact count
Add three locations, then get all locations, and verify the exact locations count in the HTTP response is equal to three.

In addition to this, add an assertion for the `AirPressureAtSeaLevel` value for the first element in the JSON array.

> :key: To get the correct value to use in the assertion, you can for instance use the Yr API from Postman or from a browser.

## Mocking dependencies
The last test you created may have passed now, but will probably give you challenges in the not so distant future. In only a couple of minutes, the `AirPressureAtSeaLevel` value passed from the Yr API has probably changed, and our test won't pass anymore.

How do we deal with situations like this, where we can't control the state of the test data? Mocks to the rescue!

You already learned about mocks with regards to unit testing and the Moq library. The concept is exactly the same in integration testing, but the mocks in integration testing typically encompass larger parts/components than what they do in unit tests.



### Mocking the Yr API
Our application's integration tests isn't actually dependent on the Yr API. It's only dependent on something that gives *the same kind of response* that the Yr API responds with. This is exactly what we are going to take advantage of by mocking the Yr API.

In the unit tests section, we used Moq. For mocking an external API, we're going to use a tool called *WireMock.Net*. This is a really cool, but simple tool which lets us easily mock something like the Yr API.

If you feel up for the challenge, try using the documentation (https://github.com/WireMock-Net/WireMock.Net) to mock the HTTP response we want from the Yr API.

If not, let's go through it step by step. 

#### Installing WireMock
Install the latest version of the NuGet package `WireMock.Net`.

#### Setting up the boilerplate code for WireMock
WireMock will start a server listening for traffic on a predefined URI path. When it receives traffic on this path, it will respond with a predefined HTTP response.

Start by creating a field for our mock server in our integration tests class:
```cs
private WireMockServer _wireMockServer;
```

Modify the `Setup` method so that it looks like this:
```cs
[TestInitialize]
public void Setup()
{
    var appFactory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            var wireMockServer = WireMockServer.Start();

            builder.ConfigureAppConfiguration((hostContext, app) =>
            {
                app.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("YrApi:BaseAddress", wireMockServer.Url)
                });
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LocationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<LocationDbContext>(
                    options => options.UseInMemoryDatabase("TestDb"));

                var context = services.BuildServiceProvider().GetRequiredService<LocationDbContext>();

                context.Database.EnsureDeleted();

                services.AddSingleton(wireMockServer);
            });
        });

    _testClient = appFactory.CreateClient();
    _wireMockServer = appFactory.Services.GetRequiredService<WireMockServer>();
}
```
It's kind of hard to see everything that was modified here, but I'll try to sum it up:

The line `var wireMockServer = WireMockServer.Start();` is probably self-explaining - we start the WireMock server which we'll use in our tests.

Here is the next part:
```cs
builder.ConfigureAppConfiguration((hostContext, app) =>
{
    app.AddInMemoryCollection(new KeyValuePair<string, string>[]
    {
        new KeyValuePair<string, string>("YrApi:BaseAddress", wireMockServer.Url)
    });
});
```
Here, we override the configuration of the base address of the Yr API used in the `Yr.Facade` project. The new address is fetched from the `Url` property of the `wireMockServer` instance we created.

Next, the line `services.AddSingleton(wireMockServer);` adds the WireMock server to our test web host services list.

Lastly, we initialize the field we created earlier with `_wireMockServer = appFactory.Services.GetRequiredService<WireMockServer>();`. This field is how we will be referencing our WireMock server instance in our integration tests.

Now we have everything we need to start using WireMock in our tests.

#### Configuring our mock server for HTTP requests & responses
The API for configuring WireMock's request and response logic is really simple. Look at the [stubbing documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Stubbing) for some really cool examples on creating logic on our WireMock server instances.

Try using the WireMock documentation to mock the Yr API response. A sample response from the Yr API looks like this:
```json
{
    "type": "Feature",
    "properties": {
        "meta": {
            "updated_at": "2022-08-28T16:36:40Z",
            "units": {
                "air_pressure_at_sea_level": "hPa",
                "air_temperature": "celsius",
                "cloud_area_fraction": "%",
                "precipitation_amount": "mm",
                "relative_humidity": "%",
                "wind_from_direction": "degrees",
                "wind_speed": "m/s"
            }
        },
        "timeseries": [
            {
                "time": "2022-08-28T16:00:00Z",
                "data": {
                    "instant": {
                        "details": {
                            "air_pressure_at_sea_level": 1019.0,
                            "air_temperature": 19.7,
                            "cloud_area_fraction": 42.3,
                            "relative_humidity": 47.5,
                            "wind_from_direction": 24.7,
                            "wind_speed": 3.0
                        }
                    }
                }
            }
        ]
    }
}
```

> :key: When creating the response, you can either use the JSON string itself, or serialize a .NET object to JSON. Both are valid, but the latter option is preferred.

If you are stuck, look at the code in `/save-points/02-component-integration-testing` for inspiration.

#### Other test case suggestions
If you are done creating the integration tests and mocks suggested above, but still want more, here are a few suggestions for integration test cases to supplement the others.
<details>
<summary>Additional integration test cases</summary>

1. Test the Delete operation by first adding a number of locations. Then, remove some of them. Get all locations now, and verify the count is correct.
2. Test the Update operation by first adding a location. Then update it. Lastly, get the location, and verify that the correct parts have been updated.
3. Can you come up with some other essential test cases for our API? Feel free to add them!
</details><br>'

That's it! All done with component integration testing. Now on to system integration testing.

**Next up - [Setup system integration testing](03a-setup-system-integration-testing.md)**