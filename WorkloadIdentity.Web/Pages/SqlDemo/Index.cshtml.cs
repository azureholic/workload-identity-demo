using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkloadIdentity.Web.Helpers;

namespace WorkloadIdentity.Web.Pages.SqlDemo
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            string keyvaultUrl = "https://rbr-kv-we.vault.azure.net/";
            string secretName = "supersecret";

            
            DefaultAzureCredentialOptions options = 
                DefaultCredentialOptions.GetDefaultAzureCredentialOptions(
                    "deb5d59e-8a1d-4860-8342-0eee384b3057", 
                    _environment);

            try {
                SecretClient client = new SecretClient(
                  new Uri(keyvaultUrl),
                  new DefaultAzureCredential(options));

                
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