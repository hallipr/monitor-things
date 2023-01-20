using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;

using MonitorThings.Api.Configuration;

public static class Startup
{
    public static void Configure(WebApplicationBuilder builder)
        {
            var settingsSection = builder.Configuration.GetSection("PipelineWitness");
            var settings = new ServiceSettings();
            settingsSection.Bind(settings);

            builder.Services.AddAzureClients(azureBuilder =>
            {
                azureBuilder.UseCredential(new DefaultAzureCredential());
                azureBuilder.AddSecretClient(new Uri(settings.KeyVaultUri));
                azureBuilder.AddBlobServiceClient(new Uri(settings.BlobStorageAccountUri));
            });

            builder.Services.AddLogging();
            builder.Services.AddMemoryCache();

            builder.Services.Configure<ServiceSettings>(settingsSection);
            builder.Services.AddSingleton<TokenCredential, DefaultAzureCredential>();
            builder.Services.AddSingleton<SecretClientProvider>();
            builder.Services.AddSingleton<IPostConfigureOptions<ServiceSettings>, PostConfigureKeyVaultSettings<ServiceSettings>>();

            // builder.Services.AddSingleton<IHostedService, ArkServerUptimeService>();
        }
}
