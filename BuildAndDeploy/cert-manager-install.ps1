$RegistryName = "rbrcontainerregistry"
$ResourceGroup = (Get-AzContainerRegistry | Where-Object { $_.name -eq $RegistryName } ).ResourceGroupName
$CertManagerTag = "v1.8.0"
$CertManagerImageController = "jetstack/cert-manager-controller"
$CertManagerImageWebhook = "jetstack/cert-manager-webhook"
$CertManagerImageCaInjector = "jetstack/cert-manager-cainjector"

# Set variable for ACR location to use for pulling images
$AcrUrl = (Get-AzContainerRegistry -ResourceGroupName $ResourceGroup -Name $RegistryName).LoginServer

# Label the ingress-basic namespace to disable resource validation
kubectl label namespace ingress-basic cert-manager.io/disable-validation=true

# Add the Jetstack Helm repository
helm repo add jetstack https://charts.jetstack.io

# Update your local Helm chart repository cache
helm repo update

# Install the cert-manager Helm chart
helm install cert-manager jetstack/cert-manager `
    --namespace ingress-basic `
    --version $CertManagerTag `
    --set installCRDs=true `
    --set nodeSelector."kubernetes\.io/os"=linux `
    --set image.repository="${AcrUrl}/${CertManagerImageController}" `
    --set image.tag=$CertManagerTag `
    --set webhook.image.repository="${AcrUrl}/${CertManagerImageWebhook}" `
    --set webhook.image.tag=$CertManagerTag `
    --set cainjector.image.repository="${AcrUrl}/${CertManagerImageCaInjector}" `
    --set cainjector.image.tag=$CertManagerTag