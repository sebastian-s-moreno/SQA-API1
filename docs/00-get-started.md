# Get started
In these academy exercises you will create tests for an ASP.NET Core Web API. In short terms, the API keeps track of locations and some data related to the locations. The API also has operations for adding, updating and deleting locations, in addition to giving vacation recommendations.

The API consists of three different layers:
 - Controller layer (**Forte.Location.Api** project)
 - Service layer (**Forte.Location.Services** project)
 - Data layer (**Forte.Location.DataAccess** project)
  
<!-- 

TODO: Reformulate this to something with mocking?

As the data layer is not that relevant for the unit testing, we will save that code for later. You can see which methods the data layer should have by inspecting the interface `ILocationRepository`.

-->

You will write tests which verifies different parts of the system.

## Clone the project
The solution can be found at https://fortedigital.visualstudio.com/Software%20Quality%20Academy.

#### Clone directly into Visual Studio 2022
To clone the repository from Azure DevOps directly into Visual Studio, choose **Repos > Files** from the navigation menu to the left. Make sure the "Api" repository is chosen from the navigation bar at the top.

Click the "Clone" button, and then from the IDE option, choose "Visual Studio". Visual Studio opens, and you can choose to clone the repo to a directory of your choice.

## Open project in Visual Studio
After cloning the repository, you are presented with multiple identical solutions in Visual Studio. Open the one in the path "src".

## Start the solution
Start the `Forte.Location.Api` project in Visual Studio to verify that it works. It should open a browser and an index page with the title "My Weather API".

Next up - [Setup and preparation for unit tests](01a-setup-unit-tests.md)