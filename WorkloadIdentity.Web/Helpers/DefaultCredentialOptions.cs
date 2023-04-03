using Azure.Identity;

namespace WorkloadIdentity.Web.Helpers;

public static class DefaultCredentialOptions {
    private static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptionsImpl(string? clientId, string? tenantId, IWebHostEnvironment environment) {

        DefaultAzureCredentialOptions credentialOptions = new() {
            Diagnostics =
            {
                LoggedHeaderNames = { "x-ms-request-id" },
                LoggedQueryParameters = { "api-version" },
                IsLoggingContentEnabled = true
            },

            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeAzureCliCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeManagedIdentityCredential = false
        };

        if (clientId is not null) {
            credentialOptions.ManagedIdentityClientId = clientId;
        }

        if (tenantId is not null)
        {
            credentialOptions.TenantId = tenantId;
        }


        if (environment.EnvironmentName == "Local Development") {
            credentialOptions.ExcludeManagedIdentityCredential = true;
            credentialOptions.ExcludeAzureCliCredential = false;
        }

        return credentialOptions;
    }

    public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(IWebHostEnvironment environment) {
        return GetDefaultAzureCredentialOptionsImpl(null, null, environment);
    }

    public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(string clientId, IWebHostEnvironment environment) {
        return GetDefaultAzureCredentialOptionsImpl(clientId, null, environment);
    }

    public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(string clientId, string tenantId, IWebHostEnvironment environment)
    {
        return GetDefaultAzureCredentialOptionsImpl(clientId, tenantId, environment);
    }
}
