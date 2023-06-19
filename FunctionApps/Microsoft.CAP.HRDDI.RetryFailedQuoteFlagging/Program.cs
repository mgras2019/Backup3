using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var host = Host.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(configurationBuilder =>
                   {
                       configurationBuilder.AddCommandLine(args);
                   })
                   .ConfigureFunctionsWorkerDefaults()
                   .ConfigureServices(services =>
                   {
                       services.AddLogging();
                       services.AddHttpClient();
                       services.AddHttpClient("tokenClient")
                       .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                       {
                           AllowAutoRedirect = false
                       });
                   })
                   .Build();

await host.RunAsync();