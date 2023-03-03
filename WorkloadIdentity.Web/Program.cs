using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using WorkloadIdentity.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

//diagnostics for troubleshooting
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

DefaultAzureCredentialOptions dataProtectionCredentialOptions =
        DefaultCredentialOptions.GetDefaultAzureCredentialOptions(builder.Environment);

var dataProtectionCredential = new DefaultAzureCredential(dataProtectionCredentialOptions);


Console.WriteLine(dataProtectionCredentialOptions.ManagedIdentityClientId);

//builder.Services.AddDataProtection()
//    .PersistKeysToAzureBlobStorage(new Uri(builder.Configuration["DataProtection:StorageAccountUri"]), dataProtectionCredential)
//    .ProtectKeysWithAzureKeyVault(new Uri(builder.Configuration["DataProtection:KeyvaultUri"]), dataProtectionCredential)
//    .SetApplicationName("WorkloadIdentityApp");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
     .AddMicrosoftIdentityWebApp(options => {
            builder.Configuration.Bind("AzureAd", options);
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = (context) =>
                {
                    if (context.Request.Headers.ContainsKey("X-Forwarded-Host"))
                    {
                        context.ProtocolMessage.RedirectUri = "https://" + context.Request.Headers["X-Forwarded-Host"] + builder.Configuration.GetSection("AzureAd").GetValue<String>("CallbackPath");
                    }
                    return Task.FromResult(0);
                }
            };
        });
  



builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

//preserve proxy protocol (e.g. K8S Ingress)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});




// Add services to the container.
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});
app.UseForwardedHeaders();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();
