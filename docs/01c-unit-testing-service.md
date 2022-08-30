# Unit testing - service
In this session, you will add functionality and create tests to the service layer (in this case, the `LocationService` class) based on the interface defined when implementing the controller.

Again, we have external dependencies (the data access layer and the Yr facade) that we will mock out. That way we can still write and test code without actually having a fully functioning program, and only focus on the system under test.

## Creating test cases for the LocationService class
In this exercise, we will expand the test class `LocationServiceTests` with test methods.

In addition to that, we will also implement the vacation recommendation service and create unit tests for that.

Start by initializing the class. The constructor of `LocationService` takes two parameters, `ILocationRepository` and `YrFacade`, hence both of them need to be mocked. 

```cs
private LocationService _locationService;
private Mock<ILocationRepository> _locationRepositoryMock;
private Mock<IYrFacade> _yrFacadeMock;
```

Initialize the mocks in the `[TestInitialize]`-method, and instruct the mocks. You can add more instructions when you are creating the tests, and get a better understanding of what you need.


## Functionality - Getting weather data from Yr

To get weather details from the Yr API, we can either use `GetUpdatedDetails(string id)` or `GetUpdatedDetails(double? longitude, double? latitude)`. Both of them are declared, but not implemented, in the `LocationService` class.

#### GetUpdatedDetails(string id)
Start by creating a test method for `GetUpdatedDetails(string id)` that verifies that we can fetch weather data from Yr for an existing location. We can choose one of the locations that we mocked in the preparations, for example Bergen, which has ID = **5e525c01-d4c8-4c35-a7da-f4ad7b19dd59**.

#### GetUpdatedDetails(double? longitude, double? latitude)
In the same way as with ID, create both a test method and an actual method, but this time with longitude and latitude as parameters.

#### Other test case suggestions
Here are some other suggestions for test cases you should try to implement: 

1. GetDetails_InvalidString_ReturnsNull
2. GetDetails_InvalidCoordinates_ReturnsNull
3. GetDetails_NoInput_ReturnsNull

Some of these test cases require that the `GetLocation` method in the `LocationService` class is implemented. 

## Functionality - Create location
Just like the controller layer used CRUD operations to manage locations, our service layer does the same.

Create a test method for the `AddLocation` method. We will first write a test to verify that the boolean value `true` is returned whenever creating a location is successful.

Now, try creating a test to verify that the `AddLocation` method returns false if something goes wrong.

Lastly, implement the `AddLocation` method itself, to make the tests passing.

## Functionality - Read (get) location
Our service layer needs functionality both to get a list of *all* locations, as well as details about a *particular* location.

Implement some test methods for both of these methods, which has the following signatures:
```cs
public List<LocationM> GetLocations()
public LocationM? GetLocation(string id)
```
Each method should have at least one "happy-path" test, and one which checks for failures. 
Make the tests run by implementing the methods.

## Functionality - Update location
Implement at least two test methods for the `UpdateLocation` method, which has the following signature:
```cs
public Task<bool> UpdateLocation(string id, LocationM location)
```

Make the tests run by implementing the class method.

## Functionality - Delete location
Implement at least two test methods for the `DeleteLocation` method, which has the following signature:
```cs
public bool DeleteLocation(string id)
```

Make the tests run by implementing the method.

## Functionality - Vacation activity recommendation
We will now add functionality that will recommend the location that is the most suited to the activity the user has chosen.

Start by creating a new service interface in the **Forte.Location.Services** project:

```cs
public interface IRecommendationService
{
    public LocationM? GetRecommendedLocation(Activity activity);
}
```

You will also need an `Activity` data model, as Visual Studio warns you about:
```cs
namespace Forte.Location.Services.Models
{
    public enum Activity
    {
        Swimming,
        Sailing,
        Sightseeing,
        Skiing,
        Unspecified
    }
}
```

Now write an implementation of this interface which obeys the following rules:
- If `Activity` is **Sailing**, return the windiest location
- If `Activity` is **Sightseeing**, return the location with the lowest air pressure
- If `Activity` is **Skiing**, return the coldest location
- If `Activity` is **Swimming**, return the warmest location
- If `Activity` is **Unspecified**, return a random location

This time we created the implementation of the `RecommendationService` class first. Let's also create some tests for it.
Here are some suggestions for test cases you should implement: 

- GetRecommendedLocation_Swimming_ReturnsWarmestLocation
- GetRecommendedLocation_Sailing_ReturnsMostWindyLocation
- GetRecommendedLocation_Skiing_ReturnsColdestLocation
- GetRecommendedLocation_Sightseeing_ReturnsHighestAirPressureLocation
- GetRecommendedLocation_Unspecified_ReturnsRandomLocationNotNull
- GetRecommendedLocations_NoneLocationsExists_ReturnsNull




## Extra Tasks
If you have more time, you could try to implement some of the tests by using the NUnit framework instead of MSUnit. 
You could also try to use NSubstitute instead of Moq. NSubstitute is another mocking library.





Next up - [Component integration testing - Setup](02a-component-integration-testing-setup.md)
