## Lib.AspNetCore.ServerTiming 4.4.0
### Additions and Changes
- Added [metric filters](https://tpeczek.github.io/Lib.AspNetCore.ServerTiming/articles/advanced.html#metric-filters)

## Lib.AspNetCore.ServerTiming 4.3.0
### Additions and Changes
- Added .NET 6 TFM

## Lib.AspNetCore.ServerTiming 4.2.0
### Additions and Changes
- Added helper method for easy timing of non-generic tasks execution (thanks to @Vake93)

## Lib.AspNetCore.ServerTiming 4.1.0
### Bug Fixes
- Fix support for delivering metrics through HTTP trailers when available

## Lib.AspNetCore.ServerTiming 4.0.0
### Additions and Changes
- Changed support for ASP.NET Core 3.0.0 to ASP.NET Core 3.1.0
- Added support for ASP.NET Core in .NET 5

## Lib.AspNetCore.ServerTiming 3.2.0
### Additions and Changes
- Added support for delivering metrics through HTTP trailers when available. To check if HTTP trailers delivery is possible for current request one can check `IServerTiming.DeliveryMode` property.

## Lib.AspNetCore.ServerTiming 3.1.0
### Additions and Changes
- Added helper methods for easy timing of code blocks and tasks execution (thanks to @KeithHenry)

   ```cs
   using (serverTiming.TimeAction())
   {
       // Code block you want to time
       ...
   }
   ```

   ```cs
   var operationResult = await serverTiming.TimeTask(AynchronousOperationYouWantToTime());
   ```

   Both methods can take a metric name, if it won't be provided the default name is `{fileNameWithoutExtension}.{functionName}+{lineNumber}`.

## Lib.AspNetCore.ServerTiming 3.0.0
### Additions and Changes
- Added escaping of metric name so it's a valid header token (thanks to @KeithHenry)

## Lib.AspNetCore.ServerTiming 2.0.0
### Additions and Changes
- Updated to the latest version of specification (thanks to @cvazac)
- Improved testing support