using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.CAP.ICEM.Business.Interface;
using Microsoft.CAP.ICEM.Business.Service;
using Microsoft.CAP.PAM.ICEM.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var appsettings = new AppSettings();

var keyVaultEndpoint = Environment.GetEnvironmentVariable("KeyVaultUrl");

var azureServiceTokenProvider = new AzureServiceTokenProvider();
SecretClient secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());

appsettings.ClientId = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("ClientId")).Result?.Value.Value;
appsettings.Instance = Environment.GetEnvironmentVariable("Instance");
appsettings.Domain = Environment.GetEnvironmentVariable("Domain");
appsettings.ClientSecret = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("ClientSecret")).Result?.Value.Value;
appsettings.ICEMResourceUrl = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("ICEMResourceUrl")).Result?.Value.Value;
appsettings.ICEMBaseAddress = Environment.GetEnvironmentVariable("ICEMBaseAddress");
appsettings.RequestorTeamsEndpoint = Environment.GetEnvironmentVariable("RequestorTeamsEndpoint");
appsettings.PartnerTypesEndpoint = Environment.GetEnvironmentVariable("PartnerTypesEndpoint");
appsettings.PartnerCountriesEndpoint = Environment.GetEnvironmentVariable("PartnerCountriesEndpoint");
appsettings.ReasonsForEscalationEndpoint = Environment.GetEnvironmentVariable("ReasonsForEscalationEndpoint");
appsettings.InvestigationTeamsEndpoint = Environment.GetEnvironmentVariable("InvestigationTeamsEndpoint");
appsettings.CreatePAMEndpoint = Environment.GetEnvironmentVariable("CreatePAMEndpoint");
appsettings.PAMInvestigationTeamCode = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("PAMInvestigationTeamCode")).Result?.Value.Value;
appsettings.DefaultReasonForEscalation = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("ReasonForEscalationCode")).Result?.Value.Value;
appsettings.EAProgramCode = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("EAProgramCode")).Result?.Value.Value;
appsettings.OPENProgramCode = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("OPENProgramCode")).Result?.Value.Value;
appsettings.RequestorTeamCode = secretClient.GetSecretAsync(Environment.GetEnvironmentVariable("RequestorTeamCode")).Result?.Value.Value;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureFunctionsWorkerDefaults()
                 .ConfigureServices(services =>
                 {
                     services.AddLogging();
                     services.AddHttpClient();
                     services.AddSingleton<AppSettings>(appsettings);
                     services.AddScoped<IMetadataService, MetadataService>();
                     services.AddScoped<IPAMRequestService, PAMRequestService>();
                 })
                .Build();

host.Run();