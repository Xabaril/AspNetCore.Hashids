# AspNetCore Hashids: Avoid predictable ids in your APIs

Sometimes developers don’t take care about security concerns when they develop a REST API. I know this scope is not straightforward but at least we should comply with a minimum of basic security rules in order to avoid the most common attacks.

The first thing we must take into account is to avoid using predictable ids like identity fields. The last vulnerability I’ve seen was a simple bash script which using a for statement it was making calls to the api passing an autoincremental id and retrieving all the confidentials documents from the system. It was an error configuring the authorization of the system, but the use of predictable ids facilitated the attack.

There are some ways to fix this kind of vulnerability, for example using GUIDs to represent this ids but from my side and for many DBA’s there are some caveats on the usage of this approach:

- 4 times larger than the traditional 4-byte index value
- Cumbersome to debug
- They are not monotonically increasing

Looking for a better alternative to avoid exposing our database ids to the clients is to use hashing ids (it creates short, unique, decryptable hashes from unsigned integers). There is a library written in many languages to generate short unique ids from integers hashids and of course there is an available version for .NET [hashids.net](https://hashids.org/net/)

## Getting started

You should install the package

```PowerShell
Install-Package AspNetCore.Hashids
```

Or via the .NET Core CLI

```csharp
dotnet add package AspNetCore.Hashids
```

In your **ConfigureServices** in the **Startup** class

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddHashids(setup => setup.Salt = "your_salt");
}
```

In your DTOs, decorate the properties that you want to be hash with the **JsonConverter** attribute **HashidsJsonConverter**

```csharp
public class CustomerDto
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    public int NonHashid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

It will be hashed:

```json
[
  {
    "id": "rA3d",
    "nonHashid": 10000,
    "firstName": "Luis",
    "lastName": "Ruiz"
  },
  {
    "id": "1vzX",
    "nonHashid": 20000,
    "firstName": "Unai",
    "lastName": "Zorrilla"
  }
]
```

Also you can use the HashidsRouteConstarint and the HashidsModelBinder to convert the hashid generated in the original integer value:

```csharp
[HttpGet]
[Route("{id:hashids}")]
[Produces(MediaTypeNames.Application.Json)]
public ActionResult<CustomerDto> Get(
    [ModelBinder(typeof(HashidsModelBinder))] int id)
{
    return Ok(customers.SingleOrDefault(c => c.Id == id));
}
```

You see a full example [here](https://github.com/Xabaril/AspNetCore.Hashids/samples/WebApi) and how to modify our swagger-ui to change the type of the hashids from integers to strings.

... and last but not least a big thanks to all our [contributors](https://github.com/Xabaril/Balea/graphs/contributors)!

## Code of conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
