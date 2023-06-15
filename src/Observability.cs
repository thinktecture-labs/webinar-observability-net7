using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Thinktecture.Webinars.SampleApi;

public class Observability
{
    public static readonly string ServiceName = typeof(Observability).Assembly.GetName().Name!;

    public static readonly string ServiceVersion = typeof(Observability).Assembly.GetName().Version != null
        ? typeof(Observability).Assembly.GetName().Version!.ToString()
        : "0.0.0";

    public static readonly ActivitySource Default = new ActivitySource(ServiceName);

    public static readonly Meter Meter = new Meter(ServiceName, ServiceVersion);

    public static readonly UpDownCounter<long> ProductCount =
        Meter.CreateUpDownCounter<long>("thinktecture.products.count", unit: "products", "Number of Products");
}
