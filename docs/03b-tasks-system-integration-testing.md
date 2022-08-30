# Tasks for system integration testing

This section lists the tasks for system integration testing. We are focusing on the integration between frontend and backend (the API). To test the API we will use a tool called Postman. For setup and a quick introduction, visit the [setup page](03a-setup-system-integration-testing.md). 

For projects of a more realistic size testing every aspect of the API would 

## Task 1: Create Postman tests for the happy path scenarios

A happy path is a default scenario featuring no exceptional or error conditions (shameless copy-paste from Wikipedia). Creating a test for this type of path will tell us if the system behaves correctly when used as intended.

Some possible happy path scenarios are listed below. It includes all the foundational operations of our Forte Location API, and touches all the different endpoints.

- Add a new location
    - Check status code
    - Check returned message
- Get all locations
    - Check status code
    - Check that the location added in the previous step is present
- Get the details of the location 
    - Check status code
    - Check that the returned data contains the fields we expect
- Get a recommendation for one of the activities
    - Check status code
    - Check that a recommendation is returned
- Edit the location
    - Check status code
    - Check returned message
- Get all locations
    - Check status code
    - Check that the location edited in the previous step contains new values
- Delete the location
    - Check status code
    - Check returned message
- Get all locations
    - Check status code
    - Check that the location deleted in the previous step is not present

Try implementing the happy path test cases above.

When all requests are added, you can run through the entire collection's test suite by selecting the three dots next to the collection name, and then **Run collection**. This will create a nice overview and summary of the test results.

![](images\03a-run_collection.png)

## Task 2: Create Postman tests for fail scenarios

In addition to testing the happy path, a few "not so happy" scenarios should be included as well. The system will have to deal with errors and unexpected inputs in the real world, and when it does you want to be sure it responds correctly.

A few different scenarios with possible checks are listed below. Implement them in Postman. 

- Add a new location with an empty name
    - Check status code
    - Check returned message

- Add a new location with an undefined latitude value
    - Check status code
    - Check returned message

- Delete a non-existent location
    - Check status code
    - Check returned message

- Get the details of an invalid ID
    - Check status code
    - Check returned message

## Bonus tasks: 

<ol type="a">
    <li>Feel like there are some missing API tests? Add them!  
    Possible ideas are:
        <ul>
            <li>Edit an invalid ID</li>
            <li>Set invalid values when editing a location</li>
            <li>Get a recommendation for an invalid activity</li>
        </ul>
    </li>
    <li>Run your Postman test from the command-line. This is useful if you want to automate the tests, for instance by adding them to a pipeline. <a href="https://learning.postman.com/docs/running-collections/using-newman-cli/command-line-integration-with-newman/"> Hint, hint.</a></li>
    <li>Try creating variables where it fits. Some examples:
        <ul>
            <li>A variable called "valid_id" which has a valid location ID</li>
            <li>A variable called "invalid_id" which has an invalid location ID</li>
            <li>Variables for valid and invalid activity recommendations</li>
        </ul>
    </li>
    <li>Create one or more environments with variables that will (possibly) change between the different environments. Some examples:
        <ul>
            <li>The base URL</li>
            <li>Perhaps the location IDs will also change from one environment (different databases) to another.</li>
        </ul>
    </li>
    <li>Got another cool idea? Try it out!</li>
</ol>
