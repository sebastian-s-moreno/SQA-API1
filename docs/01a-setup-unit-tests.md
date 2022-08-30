# Setup for unit tests

# Table of Contents
1. [Create the test project](#create-the-test-project)
2. [Example2](#example2)
3. [Third Example](#third-example)
4. [Fourth Example](#fourth-examplehttpwwwfourthexamplecom)

## Create the test project
In this exercise, we want to setup and prepare for testing the `LocationController` class.

Start by creating a new project in your solution. Choose the *MSTest Test Project* template, and name the project `Forte.Location.Api.Tests`.

![image](https://user-images.githubusercontent.com/25482321/150508910-02a1e4e2-20d2-42b6-9cc3-d07e55f22aef.png)

You have now created the project that will contain all our tests for the `LocationController` class.

To be able to use the actual `LocationController` methods in your tests, the **Forte.Location.Api** project must be added as a dependency in your test project. 

Inside the new test project, right-click on dependencies and choose *Add project reference*.  

Check `Forte.Location.Api` and click OK.

## Create the first test file

Expand the new test project, and look at the automatically created file **UnitTest1.cs**. It shows you how to structure your test files. 

The `[TestClass]` attribute denotes a class that contains unit tests. If this attribute is not present, none of the tests in the class will be run. 

The `[TestMethod]` attribute indicates a method is a test method, and in the same way as with `[TestClass]`, only the test methods with this attribute will be run when running the test class.

Create a class file named **LocationControllerTests.cs** inside the new project. Open the new file, add a using statement `using Microsoft.VisualStudio.TestTools.UnitTesting;`. Then add the `[TestClass]` attribute above the class declaration, and change the access modifier from `internal` to `public`, so that the test runner in Visual Studio is able to access the class.

You can delete the **UnitTest1.cs** file, since we will not use it for these exercises.

## The Moq framework

### Why do we use Moq?

The `LocationController` class is dependent on the `LocationService` class which is used in most of the `LocationController` methods. As we want to focus on the `LocationController`, and not the behaviour of the `LocationService`, we want to isolate the `LocationController` class. We create a "fake" replacement object, which simulates the behaviour of the `LocationService` class. This fake object is called a *mock*. For this we will use the mocking library *Moq*.

### Install Moq

Inside your new test project, right-click on dependencies and choose **Manage NuGet Packages**. Search for "Moq" and install the latest stable version. 

![image](https://user-images.githubusercontent.com/25482321/150527932-90fbc62b-edfc-43ad-83ef-368242641ba7.png)

### Create an instance of the LocationController in your test class
Since the `LocationController` class is the system under test (SUT), we need an instance of it. Create a field of type `LocationController` in the `LocationControllerTests` class.

```cs
private LocationController _locationController;
```

### Create a mock object
If you look at the constructor for the `LocatioController` class, you will see that it requires one parameters of type `ILocationService`. Let's create a mocks of this parameter with Moq.

At the top of **LocationControllerTests.cs**, write `using Moq;`.

Create the mock object as a member inside the `LocationControllerTests` class, right underneath the `LocationController` field we just created:
```cs
private Mock<ILocationService> _locationServiceMock;
```

Add `using` statements as required.

We will initialize all these fields in the next step.

### Create a test initializer method

Inside the `LocationControllerTests` class we want to create an initializer method which runs before each of our test cases. We decorate the method with the `[TestInitialize]` attribute, and fill it with the instructions we want to be executed before each test case.

Let's start by initializing `_locationController` and `_locationServiceMock`:

```cs
[TestInitialize]
public void Initialize() 
{ 
    // The code in here is run before each test case
    _locationServiceMock = new Mock<ILocationService>();
    _locationController = new LocationController(_locationServiceMock.Object);
}
```

### Instruct the mocks
Now that we have created the mocked `LocationService` object, we want to define some mocked locations (of type `LocationM`) that it can use for some of its actions.

Start by defining a method called `GetLocations()` inside the test class, like this:
```cs
private static List<LocationM> GetLocations()
        {
            return new List<LocationM>
            {
                new()
                {
                    ID= "040958d7-e085-4748-8518-8a23292c114b",
                    Name= "Oslo",
                    Latitude=59,
                    Longitude= 11,
                    WeatherData = new Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 1001,
                        AirTemperature = 5.6,
                        RelativeHumidity = 95.6,
                        WindFromDirection = 216.6,
                        WindSpeed = 13
                    }
                },
                new()
                {
                    ID= "6db14abf-f819-4816-99c0-3f11592603aa",
                    Name= "Trondheim",
                    Latitude=63,
                    Longitude= 10,
                    WeatherData = new Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 997.1,
                        AirTemperature = -1.6,
                        RelativeHumidity = 95.9,
                        WindFromDirection = 319.4,
                        WindSpeed = 9.3
                    }
                },
                new()
                {
                    ID= "5e525c01-d4c8-4c35-a7da-f4ad7b19dd59",
                    Name= "Bergen",
                    Latitude=60,
                    Longitude= 5,
                    WeatherData = new Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 997.1,
                        AirTemperature = -1.6,
                        RelativeHumidity = 95.9,
                        WindFromDirection = 319.4,
                        WindSpeed= 9.3
                    }
                }
            };
        }
```

We need to instruct the mock to behave as the object it replaces. This is done by using the mock's `Setup()` method.

Inside the test initializer method, add the following code:
```cs
// Add the mocked locations to a list
var locations = GetLocations();

// Return all the locations when GetLocations() is called
_locationServiceMock.Setup(mr => mr.GetLocations()).Returns(locations);

// Return a specific location by ID when GetLocations(id) is called
_locationServiceMock.Setup(mr => mr.GetLocation(
    It.IsAny<string>())).
    Returns((string i) => locations.
    Where(x => x.ID == i).
    Single());
```

The `LocationService` mock is now configured and the `LocationController` is ready for use. We still need to create some test cases. These will be created in a later step.

Next up - [Unit testing the LocationController class](01b-unit-testing-controller.md)