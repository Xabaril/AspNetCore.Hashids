using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Xunit;

namespace AspNetCore.Hashids.Tests.Seedwork
{
    public class ServerFixture
    {
        public TestServer TestServer { get; private set; }

        private static IHost _host;

        public ServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder
                    .ConfigureServices(services => services.AddSingleton<IServer>(serviceProvider => new TestServer(serviceProvider)))
                    .UseStartup<TestStartup>();
                })
                .Build();

            _host.StartAsync().Wait();

            TestServer = _host.GetTestServer();
        }
    }

    class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHashids(setup =>
                {
                    setup.Salt = "&`r&?mKW7}shDja%$l|bBS)DlA-WHz+-OP:8D#*PK|r{*_2Haxm(5Xj>l67s)5+h";
                    setup.MinHashLength = 8;
                })
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [CollectionDefinition(nameof(AspNetCoreServer))]
    public class AspNetCoreServer
        : ICollectionFixture<ServerFixture>
    {

    }

}
