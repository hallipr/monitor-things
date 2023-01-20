namespace MonitorThings.Api.Configuration
{
    public class ServiceSettings
    {
        public string KeyVaultUri { get; set; }
        
        public string BlobStorageAccountUri { get; set; }

        public string DiscordClientId { get; set; }

        public string DiscordClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}
