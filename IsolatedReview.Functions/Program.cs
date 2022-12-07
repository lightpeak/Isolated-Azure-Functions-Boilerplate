using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using LightPeak.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, builder) =>
    {
        // Store the Vault Uri in an Environment Variable, dev/test/prod can have their own KeyVault
        // After building the config, the Vault Uri will be available for configuring the Key Vault
        var config = builder.Build();
        var secretClient = new SecretClient(new Uri(config["VaultUri"]), new DefaultAzureCredential());
        builder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
    })
    .ConfigureServices((context, services) =>
    {        
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0
        services.Configure<MyOptions>(context.Configuration);

        /* Extracted:
        The options pattern uses classes to provide strongly typed access to groups of related settings.
        When configuration settings are isolated by scenario into separate classes,
        the app adheres to two important software engineering principles:

            1. Encapsulation:
                Classes that depend on configuration settings depend only on the configuration settings that they use.
            2. Separation of Concerns:
                Settings for different parts of the app aren't dependent or coupled to one another.
                Options also provide a mechanism to validate configuration data.
        */
    })
    .Build();

host.Run();