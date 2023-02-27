docker build -t aspnetapp -f .\BuildAndDeploy\dockerfile .

az acr build -r rbrcontainerregistry -t rbrcontainerregistry.azurecr.io/aspnetapp:latest --platform linux .
