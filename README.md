# ZopaLoans - rate calculation system 

## Overview

**ZopaLoans** a rate calculation system allowing prospective borrowers to obtain a quote from our pool of lenders for 36 month loans. It is based on the requirements provided by Zopa.

The system is a command-line application based on [.NET Core 2.0](https://www.microsoft.com/net/download/windows).


### Building the solution

To build solution from the source files, use the following command:
```
dotnet build
```
### Unit tests

To run unit tests, use the following command in the solution root folder:
```
dotnet test ZopaLoans.Tests 
```

### Using the console application

The application takes two input parameters:
* file name
* loan amount

A sample 'market.csv' file is provided in the application root folder. The loan amount must be 

The preferred way to use the application is going from the solution root folder to project folder (*ZopaLoans*) and run it with the standard 'dotnet run' command:
```
> cd ZopaLoans
> dotnet run <file name> <loan amount>
```

Example:
```
dotnet run market.csv 1000
```


### Notes

The application has a small setting file ('appsettings.json') with some default values which I didn't want to hardcode in the application.



