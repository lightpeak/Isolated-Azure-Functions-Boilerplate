using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using IsolatedReview.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddEnvironmentVariables();
        var builtConfig = builder.Build();
        var secretClient = new SecretClient(new Uri(builtConfig["VaultUri"]), new DefaultAzureCredential());
        builder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        builder.Build();
    })
    .ConfigureServices((context, services) =>
    {
        // options pattern
        services.Configure<MyOptions>(context.Configuration);
    })
    .Build();

host.Run();