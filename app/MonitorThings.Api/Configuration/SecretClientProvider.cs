using Azure.Core;
using Azure.Security.KeyVault.Secrets;

namespace MonitorThings.Api.Configuration
{
    public class SecretClientProvider
    {
        private readonly TokenCredential tokenCredential;

        public SecretClientProvider(TokenCredential tokenCredential)
        {
            this.tokenCredential = tokenCredential;
        }

        public virtual SecretClient GetSecretClient(Uri vaultUri)
        {
            return new SecretClient(vaultUri, this.tokenCredential);
        }
    }
}
