using System.Linq;

namespace System.Text.Json
{
    public static class JsonSerializerOptionsExtensions
    {
        public static IServiceProvider GetServiceProvider(this JsonSerializerOptions options)
        {
            return options.Converters.OfType<IServiceProvider>().FirstOrDefault()
                ?? throw new InvalidOperationException("ServiceProvider not found");
        }
    }
}
