# EasyCache

EasyCache is a thin wrapper over .NET cache API to simplify the syntax. With a simple provider model. (resurrected from [https://www.nuget.org/packages/EasyCache](https://www.nuget.org/packages/EasyCache))

## Examples

### GetOrAddAsync
```csharp
var result = await cacheManager.GetO```csharp
var key="SomeCacheKey"
var id = 1;

var result = await cacheManager.GetOrAddAsync(key, () => GetDataFromDatabase(id), expiration);
...
 
private async Task<List<int>> GetDataFromDatabase(int id)
{
    await DoSomeDatabaseWork();
    return new List<int>();
}
```
