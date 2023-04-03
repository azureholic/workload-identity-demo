using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using WorkloadIdentity.Web.Helpers;

namespace WorkloadIdentity.Web.Pages.AzureDevOps
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

        public async Task OnPost()
        {
            

            DefaultAzureCredentialOptions options =
                DefaultCredentialOptions.GetDefaultAzureCredentialOptions(
                    "deb5d59e-8a1d-4860-8342-0eee384b3057",
                    "d5080e1b-64bc-40e7-9565-31d084679242",
                    _environment);

            try
            {

                var credentials = new DefaultAzureCredential(options);
                
                var token = credentials.GetToken(new TokenRequestContext(new[] { "499b84ac-1321-427f-aa17-267ca6975798/.default" }));
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.Token);


                const String collectionUri = "https://dev.azure.com/azureholic";
                const String projectName = "managed-id-demo";
                

                // Connect to Azure DevOps Services
                VssCredentials creds = new VssBasicCredential(string.Empty, token.Token);
                VssConnection connection = new VssConnection(new Uri(collectionUri), creds);

                var buildClient = connection.GetClient<BuildHttpClient>();
                var projectClient = connection.GetClient<ProjectHttpClient>();

                var project = projectClient.GetProject(projectName).Result;
                var definition = buildClient.GetDefinitionAsync(projectName, 1).Result;


                var pipeline = new Build()
                {
                    Definition = definition,
                    Project = project
                };

                await buildClient.QueueBuildAsync(pipeline);

                ViewData["Token"] = JwtFormatter.Prettify(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }
}
