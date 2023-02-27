$RegistryName = "rbrcontainerregistry"
$ResourceGroup = (Get-AzContainerRegistry | Where-Object { $_.name -eq $RegistryName } ).ResourceGroupName
$CertManagerRegistry = "quay.io"
$CertManagerTag = "v1.8.0"
$CertManagerImageController = "jetstack/cert-manager-controller"
$CertManagerImageWebhook = "jetstack/cert-manager-webhook"
$CertManagerImageCaInjector = "jetstack/cert-manager-cainjector"

Import-AzContainerRegistryImage -ResourceGroupName $ResourceGroup -RegistryName $RegistryName -SourceRegistryUri $CertManagerRegistry -SourceImage "${CertManagerImageController}:${CertManagerTag}"
Import-AzContainerRegistryImage -ResourceGroupName $ResourceGroup -RegistryName $RegistryName -SourceRegistryUri $CertManagerRegistry -SourceImage "${CertManagerImageWebhook}:${CertManagerTag}"
Import-AzContainerRegistryImage -ResourceGroupName $ResourceGroup -RegistryName $RegistryName -SourceRegistryUri $CertManagerRegistry -SourceImage "${CertManagerImageCaInjector}:${CertManagerTag}"