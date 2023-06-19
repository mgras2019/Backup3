using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.AddCommandLine(args);
                    })
                    .ConfigureFunctionsWorkerDefaults()
                    .ConfigureServices(services =>
                    {
                        services.AddLogging();
                    })
                    .Build();

await host.RunAsync();