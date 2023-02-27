using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkloadIdentity.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            string keyvaultUrl = "https://rbr-kv-we.vault.azure.net/";
            string secretName = "supersecret";

            DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = "deb5d59e-8a1d-4860-8342-0eee384b3057"

            };

            try
            {
                SecretClient client = new SecretClient(
                  new Uri(keyvaultUrl),
                  new DefaultAzureCredential());

                // <getsecret>
                var keyvaultSecret = client.GetSecret(secretName).Value;
                ViewData["Secret"] = $"Your secret is {keyvaultSecret.Value}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);   
            }
            
        }
    }
}