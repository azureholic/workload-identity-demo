#az aks get-credentials --resource-group rg-workloadidentity-demo --name rbr-aks-demo
#az aks approuting enable --resource-group rg-workloadidentity-demo --name rbr-aks-demo

#az keyvault create --resource-group rg-workloadidentity-demo --location swedencentral --name rbr-aks-kv  --enable-rbac-authorization true
#$KEYVAULTID=$(az keyvault show --name rbr-kv-management --query "id" --output tsv)
#az aks approuting update --resource-group rg-workloadidentity-demo --name rbr-aks-demo --enable-kv --attach-kv ${KEYVAULTID}

#$ZONEID=$(az network dns zone show --resource-group rg-dns-global --name azureholic.com --query "id" --output tsv)
#az aks approuting zone add --resource-group rg-workloadidentity-demo --name rbr-aks-demo --ids=${ZONEID} --attach-zones

az aks update --resource-group rg-workloadidentity-demo --name rbr-aks-demo  --enable-oidc-issuer --enable-workload-identity
az aks show --resource-group rg-workloadidentity-demo --name rbr-aks-demo --query "oidcIssuerProfile.issuerUrl" --output tsv