# Processors

Processors are run on the collected metrics before they are delivered to the client, and may modify the collection. This allows for flexible filtering of which
metrics are made available to which callers.

The processors are specified as a collection on the ````options```` in ````UseServerTiming````

```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        ...
			
		app.UseServerTiming(options => {
            options.Processors.Add( ... );
        });
			
		...
    }
}
```


## Built in Processors

A number of built in processors are provided in the ````Lib.AspNetCore.ServerTiming.Processors```` namespace that can:

* Restrict metrics to the development environment
* Remove the descriptions from metrics when used outside the development environment
* Restrict metrics to an IP address or IP range

These can be added to the collection manually as shown above, or more conveniently through extension methods on the options collection:

```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        ...
			
		app.UseServerTiming(options => {
            options.RestrictMetricsToIp("127.0.0.1");
        });
			
		...
    }
}
```

## Custom Processors

You can add custom processors to the collection either by appending an object
that implements ````IServerTimingProcessor```` to the ````Processors```` 
collection, or by using the ````AddCustomProcessor```` extension method and supplying a
lambda function. e.g.

````cs
	app.UseServerTiming(options => {
        options.AddCustomProcessor((context, metrics) =>
        {
            if (System.DateTime.Now.Hour >=18) metrics.Add(new ServerTimingMetric("Time to stop debugging this and go for a drink!"));
            return true;
        });
    });
````
The lambda function (or Process method in a custom processor class) should return true if later processors in the collection
should be executed, or false if processing should stop and the Metrics collection be returned to the caller.

If processors remove all metrics, no ````ServerTiming```` header is sent.
