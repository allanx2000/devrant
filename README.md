# DevRant
Unofficial **csharp-client** for the *public* [devRant](https://www.devrant.io/) API. 

## Credits
The parts of the API design were based off of:

- https://github.com/LucaScorpion/JavaRant
- https://github.com/RekkyRek/RantScript/

## Prerequisites

- .NET Framework 4.5
- Visual Studio 2015

## API
### Installation

Install the NuGet package using the command below:

```
Install-Package DevRant
```

...or search for `DevRant` in the NuGet index.

### Getting started
The code below is an example how to use the library.

```
using DevRant;
using DevRant.Dtos;
....
using(var devRant = new DevRantClient())
{
    var profile = await devRant.GetProfileAsync("WichardRiezebos");
	var topTenRants = await devRant.GetRantsAsync(sort: RantSort.Top, limit: 10);
}
```

### Limitations

- Explicit use of `Task Async`.
- Missing Comments functionality
- Collabs model not fully implemented

## GUI

### Installation

- There are a few References to Innouvous MVVM, these DLLs are in the Libs folder
- Solution needs to include and a reference must be added [SQLiteWrapper](https://github.com/allanx2000/Innouvous.Utils/tree/master/Innouvous.Utils.SQLiteWrapper  "SQLiteWrapper") 

### Limitations
- Basic functionality works but still some things missing like news and Swag, Podcasts, etc.
- Others just point to the WebApp
- Login is sometimes buggy

