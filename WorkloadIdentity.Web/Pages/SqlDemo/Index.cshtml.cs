using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using WorkloadIdentity.Web.Helpers;

namespace WorkloadIdentity.Web.Pages.SqlDemo
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string customer01Id;
        private readonly string customer02Id;

        [BindProperty]
        public SqlDemoViewModel SqlDemoViewModel { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;

             customer01Id = _configuration["ManagedIdentities:Customer01"];
             customer02Id = _configuration["ManagedIdentities:Customer02"];

        }

        public void OnGet()
        {
            
            SqlDemoViewModel = new SqlDemoViewModel();
            SqlDemoViewModel.Identities.Add(new SelectListItem { Value = customer01Id, Text = "Customer 01 (Fruits)" });
            SqlDemoViewModel.Identities.Add(new SelectListItem { Value = customer02Id, Text = "Customer 02 (Hardware)" });
                

        }

        public void OnPost()
        {

            SqlDemoViewModel.Identities.Add(new SelectListItem { Value = customer01Id, Text = "Customer 01 (Fruits)" });
            SqlDemoViewModel.Identities.Add(new SelectListItem { Value = customer02Id, Text = "Customer 02 (Hardware)" });

            string sqlEndpoint = _configuration["Endpoints:SqlServer"];
            DefaultAzureCredentialOptions options = 
                DefaultCredentialOptions.GetDefaultAzureCredentialOptions(
                    SqlDemoViewModel.SelectedIdentity, 
                    _environment);

            
            try {
                var credentials = new DefaultAzureCredential(options);
                var token = credentials.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net//.default" }));
                SqlConnection conn = new SqlConnection($"Data Source={sqlEndpoint}; Initial Catalog={SqlDemoViewModel.SelectedDatabase}");
                conn.AccessToken = token.Token;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Orders";
                cmd.Connection = conn;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);
                                
                SqlDemoViewModel.JsonData = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.Token);
                SqlDemoViewModel.Token = JwtFormatter.Prettify(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                SqlDemoViewModel.Error =  ex.Message;
                _logger.LogError(ex.Message);   
            }
            
        }
    }
}