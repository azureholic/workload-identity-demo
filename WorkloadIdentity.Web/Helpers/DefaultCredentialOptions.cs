using Azure.Identity;

namespace WorkloadIdentity.Web.Helpers;

public static class DefaultCredentialOptions {
    private static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptionsImpl(string? clientId, IWebHostEnvironment environment) {

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

        if (clientId != null) {
            credentialOptions.ManagedIdentityClientId = clientId;
        }

        if (environment.IsDevelopment()) {
            credentialOptions.ExcludeAzureCliCredential = false;
        }

        return credentialOptions;
    }

    public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(IWebHostEnvironment environment) {
        return GetDefaultAzureCredentialOptionsImpl(null, environment);
    }

    public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(string clientId, IWebHostEnvironment environment) {
        return GetDefaultAzureCredentialOptionsImpl(clientId, environment);
    }
}
