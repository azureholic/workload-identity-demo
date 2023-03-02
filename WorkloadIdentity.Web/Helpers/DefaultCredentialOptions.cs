﻿using Azure.Identity;

namespace WorkloadIdentity.Web.Helpers
{
    public static class DefaultCredentialOptions
    {
        public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(string clientId, IWebHostEnvironment environment)
        {
            DefaultAzureCredentialOptions credentialOptions = new DefaultAzureCredentialOptions()
            {
                Diagnostics =
                {
                    LoggedHeaderNames = { "x-ms-request-id" },
                    LoggedQueryParameters = { "api-version" },
                    IsLoggingContentEnabled = true
                },
                ManagedIdentityClientId = clientId,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeVisualStudioCredential = true,
                ExcludeAzureCliCredential = true,
                ExcludeEnvironmentCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeManagedIdentityCredential = false
            };

            if (environment.IsDevelopment())
            {
                credentialOptions.ExcludeAzureCliCredential = false;
            }

            return credentialOptions;
        }
    }
}
