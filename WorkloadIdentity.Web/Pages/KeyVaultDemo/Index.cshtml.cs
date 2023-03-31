using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using WorkloadIdentity.Web.Helpers;

namespace WorkloadIdentity.Web.Pages.KeyVaultDemo
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

                var credentials = new DefaultAzureCredential(options);
                SecretClient client = new SecretClient(
                  new Uri(keyvaultUrl),
                  credentials);

                var token = credentials.GetToken(new TokenRequestContext(new[] {"https://vault.azure.net/.default"}));
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.Token);
                

                var keyvaultSecret = client.GetSecret(secretName).Value;
                ViewData["Secret"] = keyvaultSecret.Value;
                
                ViewData["Token"] = JwtFormatter.Prettify(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);   
            }
            
        }
    }
}