# Clienta
An easy place for staff to create, manage, and assign clients.

## Details
A ASP.NET Core (3.1) MVC application, using Razor.

## Functionality
* Create and manage employees
* Create and manage clients
* Assign clients a consultant 
* Record a client's current and previous addresses

## Running the Application
### Visual Studio via IIS
#### Visual Studio
To have Visual Studio NuGet handle retreiving the packages for you, see the [official Microsoft Docs page for Installing and Reinstalling Packages with Package Restore](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore). This also details how to restore packages manually. 

Run the project using the 

#### Visual Studio Code
* `dotnet restore`
* `dotnet run`

For more information on restoring packages via the `dotnet` CLI, visit the [Restore using dotnet CLI](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore#restore-using-the-dotnet-cli).

### Remotely
#### Docker
Navigate to the solution root and issue the following commands
```
docker-compose build
docker-compose up
```

### Testing
A `.Test` project (NUnit) exists within the solution, containing unit and integration tests. These can be run from within Visual Studio via the `Test` menu. 

For those using the `dotnet` CLI, such as with Visual Studio Code, use `dotnet test` or a test-runner extension to acheive this. For more information on `dotnet test`, visit the [Microsoft documentation page](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test).


## Using Clienta
### Logging in
A login will be seeded into the IdentityServer database, upon running the application. 

```
Email: alice@example.com
Password: password123
```