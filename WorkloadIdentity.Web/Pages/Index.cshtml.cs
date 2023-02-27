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